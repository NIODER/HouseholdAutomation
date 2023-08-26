using HouseholdAutomationDesktop.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HouseholdAutomationDesktop.Utils
{
    public class WindowPresenter : IWindowPresenter
    {
        private static readonly Dictionary<Type, Type> _mappings = new();

        public static void Register<IView, IViewModel>()
            where IView : Window
            where IViewModel : ViewModelBase
        {
            _mappings.Add(typeof(IViewModel), typeof(IView));
        }

        public IViewModel Show<IViewModel>() where IViewModel : ViewModelBase
        {
            if (_mappings.TryGetValue(typeof(IViewModel), out Type? viewType))
            {
                if (App.host.Services.GetService(viewType) is not Window view)
                {
                    throw new TypeUnloadedException($"No service loaded for {typeof(IViewModel).FullName} or its view");
                }
                if (view.DataContext is IDataLoading dataLoading)
                {
                    _ = dataLoading.LoadDataAsync();
                }
                view.ShowDialog();
                return (IViewModel)view.DataContext;
            }
            else
            {
                throw new TypeUnloadedException($"No mapping for {typeof(IViewModel).FullName}.");
            }
        }

        public void Show(ViewModelBase viewModel)
        {
            if (_mappings.TryGetValue(viewModel.GetType(), out Type? viewType))
            {
                if (App.host.Services.GetService(viewType) is not Window view)
                {
                    throw new TypeUnloadedException($"No service loaded for view of {viewModel.GetType().FullName}");
                }
                view.DataContext = viewModel;
                if (view.DataContext is IDataLoading dataLoading)
                {
                    _ = dataLoading.LoadDataAsync();
                }
                view.ShowDialog();
            }
            else
            {
                throw new TypeUnloadedException($"No mapping for {viewModel.GetType().FullName}.");
            }
        }

        public IViewModel Show<IViewModel>(EventHandler<EventArgs> callback) where IViewModel : ViewModelBase, IDialogViewModel
        {
            if (_mappings.TryGetValue(typeof(IViewModel), out Type? viewType))
            {
                if (App.host.Services.GetService(viewType) is not Window view)
                {
                    throw new TypeUnloadedException($"No service loaded for {typeof(IViewModel).FullName} or its view");
                }
                ((IDialogViewModel)view.DataContext).OnDialogResult += callback;
                if (view.DataContext is IDataLoading dataLoading)
                {
                    _ = dataLoading.LoadDataAsync();
                }
                view.ShowDialog();
                return (IViewModel)view.DataContext;
            }
            else
            {
                throw new TypeUnloadedException($"No mapping for {typeof(IViewModel).FullName}.");
            }
        }
    }
}
