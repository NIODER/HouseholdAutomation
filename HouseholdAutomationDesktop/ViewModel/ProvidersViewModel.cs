using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationDesktop.Utils;
using HouseholdAutomationDesktop.ViewModel.DialogsViewModel;
using HouseholdAutomationLogic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public RelayCommand AddResourceCommand { get; private set; }
		public RelayCommand DeleteResourceCommand { get; private set; }
		public RelayCommand AddProviderCommand { get; private set; }
        public RelayCommand DeleteProviderCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        private readonly IRedactor<Provider> _providersRedactor;
        private readonly IWindowPresenter _windowPresenter;

        public ProvidersViewModel(IRedactorFactory redactorFactory, IWindowPresenter windowPresenter)
        {
            _providersRedactor = redactorFactory.Create<Provider>();
            _windowPresenter = windowPresenter;
            AddResourceCommand = new(OnAddResourceCommand);
			DeleteResourceCommand = new(OnDeleteResourceCommand);
			AddProviderCommand = new(OnAddProviderCommand);
			DeleteProviderCommand = new(OnDeleteProviderCommand);
			SaveCommand = new(OnSaveCommand);
        }

        private void OnSaveCommand()
        {
            throw new NotImplementedException();
        }

        private void OnAddResourceCommand()
		{
            throw new NotImplementedException();
        }

        private void OnDeleteResourceCommand()
        {
            if (SelectedResource == null || SelectedProvider == null)
            {
                return;
            }
            var res = SelectedProvider.ProviderToResources
                .FirstOrDefault(p => p.ProviderId == SelectedResource.Provider.ProviderId && p.ResourceId == SelectedResource.Resource.ResourceId);
            if (res != null)
            {
                SelectedProvider.ProviderToResources.Remove(res);
                _providersRedactor.UpdateOne(SelectedProvider);
            }
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
            throw new NotImplementedException();
        }

        public async Task LoadDataAsync()
        {
			var providers = await _providersRedactor.GetAllFromDbAsync();
			Providers = new(providers);
			SelectedProvider = Providers.FirstOrDefault();
        }
    }
}
