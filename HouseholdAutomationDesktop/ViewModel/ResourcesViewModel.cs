using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationDesktop.Model;
using HouseholdAutomationDesktop.Utils;
using HouseholdAutomationDesktop.ViewModel.DialogsViewModel;
using HouseholdAutomationLogic.BLL;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HouseholdAutomationDesktop.ViewModel
{
    public class ResourcesViewModel : SaveableViewModelBase, IDataLoading
    {
		private Resource? _chosenResource;

		public Resource? ChosenResource
        {
			get { return _chosenResource; }
			set
			{
				_chosenResource = value;
				OnPropertyChanged(nameof(ChosenResource));
				if (ChosenResource != null)
				{
                    ResourceProviders = new(_resourceBLL.GetResourceProviders(ChosenResource).Select(p => p.Provider));
                }
            }
		}

		private ObservableCollection<Resource> _resources = new();

		public ObservableCollection<Resource> Resources
		{
			get { return _resources; }
			set
			{
				_resources = value;
				OnPropertyChanged(nameof(Resources));
			}
		}

		private ObservableCollection<Provider> _resourceProviders = new();

		public ObservableCollection<Provider> ResourceProviders
		{
			get => _resourceProviders;
			set
			{
				_resourceProviders = value;
				OnPropertyChanged(nameof(ResourceProviders));
			}
		}

		public RelayCommand AddResourceCommand { get; private set; }
        public RelayCommand DeleteResouceCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        private readonly ResourceBLL _resourceBLL;
		private readonly ILogger _logger;
		private readonly IWindowPresenter _windowPresenter;

        public ResourcesViewModel(ResourceBLL resourceBLL, ILogger logger, IWindowPresenter windowPresenter)
        {
            _resourceBLL = resourceBLL;
			AddResourceCommand = new(OnAddResourceCommand);
			DeleteResouceCommand = new(OnDeleteResourceCommand);
			SaveCommand = new(OnSaveCommand);
			_logger = logger;
			_windowPresenter = windowPresenter;
		}

        private async void OnSaveCommand()
        {
			try
			{
				await _resourceBLL.Redactor.SaveChangesAsync();
				IsSaved = true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
        }

        private void OnDeleteResourceCommand()
        {
			if (ChosenResource != null)
			{
				_resourceBLL.Redactor.Delete(ChosenResource);
				_resources.Remove(ChosenResource);
				IsSaved = false;
            }
        }

        private void OnAddResourceCommand()
		{
			_windowPresenter.Show<CreateResourceViewModel>(OnResourceAdded);
		}

        private async void OnResourceAdded(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

		public Task LoadDataAsync() => Task.Run(() =>
		{
            Resources = new(_resourceBLL.Redactor.GetAll());
            _logger.Log<ResourcesViewModel>(LogSeverety.Info, "Data loaded");
        });
    }
}
