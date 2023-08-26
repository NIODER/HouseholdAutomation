using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationDesktop.Model;
using HouseholdAutomationDesktop.Utils;
using HouseholdAutomationDesktop.ViewModel.DialogsViewModel;
using HouseholdAutomationLogic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HouseholdAutomationDesktop.ViewModel
{
    public class ResourcesViewModel : ViewModelBase, IDataLoading
    {
		private Resource? _chosenResource;

		public Resource? ChosenResource
        {
			get { return _chosenResource; }
			set
			{
				_chosenResource = value;
				OnPropertyChanged(nameof(ChosenResource));
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

        public RelayCommand LoadProvidersCommand { get; private set; }
		public RelayCommand AddResourceCommand { get; private set; }
        public RelayCommand DeleteResouceCommand { get; private set; }

        private readonly IRedactor<Resource> _resourcesRedactor;
		private readonly IRedactor<Provider> _providersRedactor;
		private readonly ILogger _logger;
		private readonly IWindowPresenter _windowPresenter;

        public ResourcesViewModel(IRedactorFactory redactorFactory, ILogger logger, IWindowPresenter windowPresenter)
        {
			_resourcesRedactor = redactorFactory.Create<Resource>();
			_providersRedactor = redactorFactory.Create<Provider>();
			LoadProvidersCommand = new(OnLoadProvidersCommand);
			AddResourceCommand = new(OnAddResourceCommand);
			DeleteResouceCommand = new(OnDeleteResourceCommand);
			_logger = logger;
			_windowPresenter = windowPresenter;
		}

		private void OnDeleteResourceCommand()
        {
			if (ChosenResource != null)
			{
                _resourcesRedactor.DeleteOne(ChosenResource);
            }
        }

        private void OnAddResourceCommand()
		{
			var createResourceViewModel = _windowPresenter.Show<CreateResourceViewModel>(OnResourceAdded);
		}

        private async void OnResourceAdded(object? sender, EventArgs e)
        {
			await LoadDataAsync();
        }

        private async void OnLoadProvidersCommand()
        {
			Mouse.OverrideCursor = Cursors.Wait;
			await LoadResourceProviders();
			Mouse.OverrideCursor = null;
        }

        public async Task LoadDataAsync()
        {
			Resources = new(await _resourcesRedactor.GetAllFromDbAsync());
			_logger.Log<ResourcesViewModel>(LogSeverety.Info, "Data loaded");
			await LoadResourceProviders();
        }

		public Task LoadResourceProviders()
		{
			ResourceProviders = new(_providersRedactor.GetByPredicate(p => p.ProviderToResources.Any(ptr => ptr.ResourceId == _chosenResource?.ResourceId)));
			_logger.Log<ResourcesViewModel>(LogSeverety.Info, "ResourcesProvidersLoaded.");
			return Task.CompletedTask;
		}
    }
}
