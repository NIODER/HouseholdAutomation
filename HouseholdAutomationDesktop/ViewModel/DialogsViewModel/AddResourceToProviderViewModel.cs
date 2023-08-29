using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationLogic;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HouseholdAutomationDesktop.ViewModel.DialogsViewModel
{
    public class AddResourceToProviderViewModel : ViewModelBase, IDialogViewModel, IDataLoading
    {
        private Resource? selectedResource;
        private int cost;
        private ObservableCollection<Resource> resources = new();
        private readonly IDbEntityRedactor<Resource> _resourcesRedactor;

        public class AddResourceToProviderEventArgs : EventArgs
        {
            public Resource SelectedResource { get; set; }
            public int Cost { get; set; }

            public AddResourceToProviderEventArgs(Resource selectedResource, int cost)
            {
                SelectedResource = selectedResource;
                Cost = cost;
            }
        }

        public event EventHandler<EventArgs>? OnDialogResult;

        public RelayCommand AddCommand { get; set; }

        public ObservableCollection<Resource> Resources
        {
            get => resources;
            set
            {
                resources = value;
                OnPropertyChanged(nameof(Resources));
            }
        }

        public Resource? SelectedResource
        {
            get => selectedResource;
            set
            {
                selectedResource = value;
                OnPropertyChanged(nameof(SelectedResource));
            }
        }

        public int Cost
        {
            get => cost;
            set
            {
                cost = value;
                OnPropertyChanged(nameof(Cost));
            }
        }

        public AddResourceToProviderViewModel(IDbEntityRedactor<Resource> resourcesRedactor)
        {
            _resourcesRedactor = resourcesRedactor;
            AddCommand = new(OnAddCommand);
        }

        private void OnAddCommand()
        {
            if (SelectedResource == null)
            {
                MessageBox.Show("Выберите ресурс.");
                return;
            }
            OnDialogResult?.Invoke(this, new AddResourceToProviderEventArgs(SelectedResource, Cost));
        }

        public async Task LoadDataAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            await Task.Run(() =>
            {
                Resources = new(_resourcesRedactor.GetAll());
            });
            Mouse.OverrideCursor = null;
        }
    }
}
