using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationDesktop.Utils;
using HouseholdAutomationDesktop.ViewModel.DialogsViewModel;
using HouseholdAutomationLogic.BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HouseholdAutomationDesktop.ViewModel
{
    public class ProvidersViewModel : ViewModelBase, IDataLoading
    {
		public class ResourceAdapter
		{
			public Provider Provider { get; set; }
			public ProviderToResource ProviderToResource { get; set; }
            public Resource Resource { get; set; }

            public ResourceAdapter(Provider provider, ProviderToResource providerToResource, Resource resource)
            {
                Provider = provider;
                ProviderToResource = providerToResource;
                Resource = resource;
            }
        }

		private ObservableCollection<Provider> _providers = new();

		public ObservableCollection<Provider> Providers
        {
            get => _providers;
            set
            {
                _providers = value;
                OnPropertyChanged(nameof(Providers));
            }
        }

        private Provider? _selectedProvider;

		public Provider? SelectedProvider
		{
			get => _selectedProvider;
			set
			{
				_selectedProvider = value;
				OnPropertyChanged(nameof(SelectedProvider));
				if (_selectedProvider != null)
				{
					ResourceAdapters = new(_selectedProvider.ProviderToResources
						.Select(ptr => new ResourceAdapter(ptr.Provider, ptr, ptr.Resource)));
				}
			}
		}

		private ObservableCollection<ResourceAdapter>? _resourceAdapters;

		public ObservableCollection<ResourceAdapter>? ResourceAdapters
		{
			get => _resourceAdapters;
			set
			{
				_resourceAdapters = value;
				OnPropertyChanged(nameof(ResourceAdapters));
			}
		}

		private ResourceAdapter? _selectedResourceAdapter;

		public ResourceAdapter? SelectedResource
		{
            get { return _selectedResourceAdapter; }
			set
			{
				_selectedResourceAdapter = value;
				OnPropertyChanged(nameof(SelectedResource));
			}
		}

        private readonly List<Provider> addedProviders = new();

		public RelayCommand AddResourceCommand { get; private set; }
		public RelayCommand DeleteResourceCommand { get; private set; }
		public RelayCommand AddProviderCommand { get; private set; }
        public RelayCommand DeleteProviderCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        private readonly ProviderBLL _providerBLL;
        private readonly IWindowPresenter _windowPresenter;

        public ProvidersViewModel(ProviderBLL providerBLL, IWindowPresenter windowPresenter)
        {
            _providerBLL = providerBLL;
            _windowPresenter = windowPresenter;
            AddResourceCommand = new(OnAddResourceCommand);
			DeleteResourceCommand = new(OnDeleteResourceCommand);
			AddProviderCommand = new(OnAddProviderCommand);
			DeleteProviderCommand = new(OnDeleteProviderCommand);
			SaveCommand = new(OnSaveCommandAsync);
        }

        private async void OnSaveCommandAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            addedProviders.ForEach(p => _providerBLL.Redactor.Create(p));
            await _providerBLL.Redactor.SaveChangesAsync();
            Mouse.OverrideCursor = null;
        }

        private void OnAddResourceCommand()
		{
            if (SelectedProvider == null)
            {
                MessageBox.Show("Выберите поставщика.");
                return;
            }
            _windowPresenter.Show<AddResourceToProviderViewModel>(OnResourceSelectedAsync);
        }

        private async void OnResourceSelectedAsync(object? sender, EventArgs e)
        {
            if (SelectedProvider == null)
            {
                MessageBox.Show("Выберите поставщика.");
                return;
            }
            if (e is AddResourceToProviderViewModel.AddResourceToProviderEventArgs addResourceToProviderEventArgs)
            {
                try
                {
                    await _providerBLL.AddResourceToProvider(SelectedProvider, addResourceToProviderEventArgs.SelectedResource, addResourceToProviderEventArgs.Cost);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void OnDeleteResourceCommand()
        {
            if (SelectedResource == null || SelectedProvider == null)
            {
                return;
            }
            await _providerBLL.RemoveResourceFromProvider(SelectedProvider, SelectedResource.Resource);
        }

        private void OnDeleteProviderCommand()
        {
			if (SelectedProvider != null)
			{
				Providers.Remove(SelectedProvider);
			}
        }

        private void OnAddProviderCommand()
        {
            SelectedProvider = new Provider();
            Providers.Add(SelectedProvider);
            addedProviders.Add(SelectedProvider);
        }

        public Task LoadDataAsync()
        {
            return Task.Run(() =>
            {
                var providers = _providerBLL.Redactor.GetAll();
                Providers = new(providers);
                SelectedProvider = Providers.FirstOrDefault();
            });
        }
    }
}
