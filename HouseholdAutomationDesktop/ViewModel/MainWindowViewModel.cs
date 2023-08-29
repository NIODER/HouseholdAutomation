using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationDesktop.Model;
using System.ComponentModel;
using System.Windows.Input;

namespace HouseholdAutomationDesktop.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ILogger _logger;

        private readonly OrdersViewModel _ordersViewModel;
        private readonly ClientsViewModel _clientsViewModel;
        private readonly ProvidersViewModel _providersViewModel;
        private readonly ResourcesViewModel _resourcesViewModel;

        private ViewModelBase? _selectedRedactor;

        public ViewModelBase? SelectedRedactor
        {
            get => _selectedRedactor;
            set
            {
                _selectedRedactor = value;
                OnPropertyChanged(nameof(SelectedRedactor));
            }
        }

        public RelayCommand OrdersCommand { get; private set; }
        public RelayCommand ClientsCommand { get; private set; }
        public RelayCommand ProvidersCommand { get; private set; }
        public RelayCommand ResourcesCommand { get; private set; }

        public MainWindowViewModel(ILogger logger, Locator locator)
        {
            _logger = logger;
            _ordersViewModel = locator.OrdersViewModel;
            _clientsViewModel = locator.ClientsViewModel;
            _providersViewModel = locator.ProvidersViewModel;
            _resourcesViewModel = locator.ResourcesViewModel;
            OrdersCommand = new(OnOrdersCommandClick);
            ClientsCommand = new(OnClientsCommandClick);
            ProvidersCommand = new(OnProvidersCommandClick);
            ResourcesCommand = new(OnResourcesCommandClick);
            _selectedRedactor = null;
        }

        private void OnOrdersCommandClick()
        {
            _logger.Log<MainWindowViewModel>(new LogMessage(LogSeverety.Info, "Open orders redactor."));
            SelectedRedactor = _ordersViewModel;
            LoadVMData(_ordersViewModel);
        }

        private void OnClientsCommandClick()
        {
            _logger.Log<MainWindowViewModel>(new LogMessage(LogSeverety.Info, "Open clients redactor."));
            SelectedRedactor = _clientsViewModel;
            LoadVMData(_clientsViewModel);
        }

        private void OnProvidersCommandClick()
        {
            _logger.Log<MainWindowViewModel>(new LogMessage(LogSeverety.Info, "Open providers redactor."));
            SelectedRedactor = _providersViewModel;
            LoadVMData(_providersViewModel);
        }

        private void OnResourcesCommandClick()
        {
            _logger.Log<MainWindowViewModel>(new LogMessage(LogSeverety.Info, "Open resources redactor."));
            SelectedRedactor = _resourcesViewModel;
            LoadVMData(_resourcesViewModel);
        }

        private async void LoadVMData(IDataLoading dataLoading)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            await dataLoading.LoadDataAsync();
            Mouse.OverrideCursor = null;
        }
    }
}
