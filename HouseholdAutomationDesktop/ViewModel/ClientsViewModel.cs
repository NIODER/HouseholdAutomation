using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationDesktop.Model;
using HouseholdAutomationLogic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HouseholdAutomationDesktop.ViewModel
{
    public class ClientsViewModel : ViewModelBase, IDataLoading
    {
        private Client? _chosenClient;
        private ObservableCollection<Client> _clients = new();
        public ObservableCollection<Order> ChosenClientOrders => _chosenClient == null ? new() : new(_chosenClient.Orders);
        private bool _saved = true;
        private Visibility _denyAddingButtonVisibility = Visibility.Hidden;
        private readonly IRedactor<Client> _redactor;
        private readonly ILogger _logger;
        private readonly List<Client> addedClients = new();
        private Order? _chosenOrder;

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand DenyCommand { get; private set; }
        public RelayCommand DeleteOrderCommand { get; private set; }

        public ClientsViewModel(IRedactorFactory redactorFactory, ILogger logger)
        {
            _redactor = redactorFactory.Create<Client>();
            _logger = logger;
            SaveCommand = new(OnSaveCommand);
            DeleteCommand = new(OnDeleteCommand);
            AddCommand = new(OnAddCommand);
            DenyCommand = new(OnDenyCommand);
            DeleteOrderCommand = new(OnDeleteOrderCommand);
        }

        public Order? ChosenOrder
        {
            get => _chosenOrder;
            set
            {
                _chosenOrder = value;
                OnPropertyChanged(nameof(ChosenOrder));
            }
        }

        public Client? ChosenClient
        {
            get => _chosenClient;
            set
            {
                _logger.Log<ClientsViewModel>(LogSeverety.Debug, JsonSerializer.Serialize(value));
                _chosenClient = value;
                OnPropertyChanged(nameof(ChosenClient));
                OnPropertyChanged(nameof(ChosenClientOrders));
                if (_chosenClient?.ClientId == default)
                {
                    DenyAddingButtonVisibility = Visibility.Visible;
                }
                else
                {
                    DenyAddingButtonVisibility = Visibility.Hidden;
                }
            }
        }

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }

        public Visibility DenyAddingButtonVisibility
        {
            get => _denyAddingButtonVisibility;
            set
            {
                _denyAddingButtonVisibility = value;
                OnPropertyChanged(nameof(DenyAddingButtonVisibility));
            }
        }

        private async void OnSaveCommand()
        {
            if (_saved)
            {
                MessageBox.Show("Нет изменений.");
                return;
            }
            Mouse.OverrideCursor = Cursors.Wait;
            foreach (var client in addedClients)
            {
                _redactor.InsertOneAndSave(client);
            }
            await _redactor.SaveChangesAsync();
            await LoadDataAsync();
            Mouse.OverrideCursor = null;
            _saved = true;
            MessageBox.Show("Сохранено");
            DenyAddingButtonVisibility = Visibility.Hidden;
        }

        private void OnDeleteCommand()
        {
            if (_chosenClient != null)
            {
                _redactor.DeleteOne(_chosenClient);
                Clients.Remove(_chosenClient);
                ChosenClient = Clients.FirstOrDefault();
                _saved = false;
            }
        }

        private void OnAddCommand()
        {
            _saved = false;
            ChosenClient = new Client();
            Clients.Add(ChosenClient);
            addedClients.Add(ChosenClient);
            DenyAddingButtonVisibility = Visibility.Visible;
        }

        private void OnDenyCommand()
        {
            _saved = true;
            ChosenClient = _clients.FirstOrDefault();
        }

        private void OnDeleteOrderCommand()
        {
            if (_chosenClient != null && _chosenOrder != null)
            {
                _chosenClient.Orders.Remove(_chosenOrder);
                _saved = false;
            }
        }

        public async Task LoadDataAsync()
        {
            Clients = new(await _redactor.GetAllFromDbAsync());
            ChosenClient = Clients.FirstOrDefault();
            _logger.Log<ClientsViewModel>(LogSeverety.Info, $"All clients selected from database. Count {Clients.Count}.");
        }
    }
}
