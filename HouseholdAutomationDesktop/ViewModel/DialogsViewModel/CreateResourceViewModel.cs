using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationLogic;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HouseholdAutomationDesktop.ViewModel.DialogsViewModel
{
    public class CreateResourceViewModel : ViewModelBase, IDataLoading, IDialogViewModel
    {
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

        private readonly IRedactor<Resource> _redactor;
        private readonly IRedactor<Provider> _providersRedactor;

        public event EventHandler<EventArgs>? OnDialogResult;

        public CreateResourceViewModel(IRedactorFactory redactorFactory)
        {
            _providersRedactor = redactorFactory.Create<Provider>();
            _redactor = redactorFactory.Create<Resource>();
            SaveCommand = new(OnSaveCommand);
        }

        private async void OnSaveCommand()
        {
            var resource = await _redactor.InsertOneAndSaveAsync(Resource);
            if (_selectedProvider != null)
            {
                resource.ProviderToResources.Add(new()
                {
                    ResourceId = resource.ResourceId,
                    Cost = _cost,
                    ProviderId = _selectedProvider.ProviderId
                });
                _redactor.UpdateOne(resource);
                await _redactor.SaveChangesAsync();
            }
            OnDialogResult?.Invoke(this, new());
        }

        public async Task LoadDataAsync()
        {
            Providers = new(await _providersRedactor.GetAllFromDbAsync());
        }
    }
}
