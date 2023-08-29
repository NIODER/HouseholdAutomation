using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationLogic.BLL;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HouseholdAutomationDesktop.ViewModel.DialogsViewModel
{
    public class CreateResourceViewModel : ViewModelBase, IDataLoading, IDialogViewModel
    {
        public class CreateResourceEventArgs : EventArgs
        {
            public Resource CreatedResource { get; set; }

            public CreateResourceEventArgs(Resource createdResource)
            {
                CreatedResource = createdResource;
            }
        }

        private Resource _resource = new();

        public Resource Resource
        {
            get { return _resource; }
            set
            {
                _resource = value;
                OnPropertyChanged(nameof(Resource));
            }
        }

        private int _cost;

        public int Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                OnPropertyChanged(nameof(Cost));
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
            get { return _selectedProvider; }
            set
            {
                _selectedProvider = value;
                OnPropertyChanged(nameof(SelectedProvider));
            }
        }

        public RelayCommand SaveCommand { get; private set; }

        private readonly ResourceBLL _resourceBLL;
        private readonly ProviderBLL _providerBLL;

        public event EventHandler<EventArgs>? OnDialogResult;

        public CreateResourceViewModel(ResourceBLL resourceBLL, ProviderBLL providerBLL)
        {
            _resourceBLL = resourceBLL;
            _providerBLL = providerBLL;
            SaveCommand = new(OnSaveCommand);
        }

        private async void OnSaveCommand()
        {
            var resource = await _resourceBLL.Redactor.CreateAndSaveAsync(Resource);
            if (_selectedProvider != null)
            {
                resource.ProviderToResources.Add(new()
                {
                    ResourceId = resource.ResourceId,
                    Cost = _cost,
                    ProviderId = _selectedProvider.ProviderId
                });
                resource = await _resourceBLL.Redactor.UpdateAndSaveAsync(resource);
            }
            OnDialogResult?.Invoke(this, new());
        }

        public Task LoadDataAsync() => Task.Run(() =>
        {
            Providers = new(_providerBLL.Redactor.GetAll());
        });
    }
}
