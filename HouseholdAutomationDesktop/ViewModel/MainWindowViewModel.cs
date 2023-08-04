using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationDesktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseholdAutomationDesktop.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ILogger _logger;

        private readonly OrdersViewModel _ordersViewModel;
        private readonly ClientsViewModel _clientsViewModel;
        private readonly ProvidersViewModel _providersViewModel;
        private readonly ResourcesViewModel _resourcesViewModel;

        private ViewModelBase _selectedRedactor;

        public ViewModelBase SelectedRedactor
        {
            get => _selectedRedactor;
            set
            {
                OnPropertyChanged(nameof(SelectedRedactor));
                _selectedRedactor = value;
            }
        }

        public RelayCommand OrdersCommand { get; private set; }
        public RelayCommand ClientsCommand { get; private set; }
        public RelayCommand ProvidersCommand { get; private set; }
        public RelayCommand ResourcesCommand { get; private set; }

        public MainWindowViewModel(ILogger logger,
            OrdersViewModel ordersViewModel,
            ClientsViewModel clientsViewModel,
            ProvidersViewModel providersViewModel,
            ResourcesViewModel resourcesViewModel)
        {
            _logger = logger;
            _ordersViewModel = ordersViewModel;
            _clientsViewModel = clientsViewModel;
            _providersViewModel = providersViewModel;
            _resourcesViewModel = resourcesViewModel;
            OrdersCommand = new(OnOrdersCommandClick);
            ClientsCommand = new(OnClientsCommandClick);
            ProvidersCommand = new(OnProvidersCommandClick);
            ResourcesCommand = new(OnResourcesCommandClick);
            _selectedRedactor = ordersViewModel;
        }

        private void OnOrdersCommandClick()
        {
            _logger.Log<MainWindowViewModel>(new LogMessage(LogSeverety.Info, "Open orders redactor."));
            SelectedRedactor = _ordersViewModel;
        }

        private void OnClientsCommandClick()
        {
            _logger.Log<MainWindowViewModel>(new LogMessage(LogSeverety.Info, "Open clients redactor."));
            SelectedRedactor = _clientsViewModel;
        }

        private void OnProvidersCommandClick()
        {
            _logger.Log<MainWindowViewModel>(new LogMessage(LogSeverety.Info, "Open providers redactor."));
            SelectedRedactor = _providersViewModel;
        }

        private void OnResourcesCommandClick()
        {
            _logger.Log<MainWindowViewModel>(new LogMessage(LogSeverety.Info, "Open resources redactor."));
            SelectedRedactor = _resourcesViewModel;
        }
    }
}
