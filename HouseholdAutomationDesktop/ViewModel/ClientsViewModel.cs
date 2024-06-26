﻿using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationDesktop.Model;
using HouseholdAutomationLogic.BLL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HouseholdAutomationDesktop.ViewModel
{
    public class ClientsViewModel : SaveableViewModelBase, IDataLoading
    {
        private Client? _chosenClient;
        private ObservableCollection<Client> _clients = new();
        public ObservableCollection<Order> ChosenClientOrders => _chosenClient == null ? new() : new(_chosenClient.Orders);
        private Visibility _denyAddingButtonVisibility = Visibility.Hidden;
        private readonly ClientsBLL _clientsBLL;
        private readonly ILogger _logger;
        private readonly List<Client> addedClients = new();
        private Order? _chosenOrder;

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand DenyCommand { get; private set; }
        public RelayCommand DeleteOrderCommand { get; private set; }

        public ClientsViewModel(ClientsBLL clientsBLL, ILogger logger)
        {
            _clientsBLL = clientsBLL;
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
            if (IsSaved)
            {
                MessageBox.Show("Нет изменений.");
                return;
            }
            Mouse.OverrideCursor = Cursors.Wait;
            foreach (var client in addedClients)
            {
                _clientsBLL.Redactor.Create(client);
            }
            await _clientsBLL.Redactor.SaveChangesAsync();
            IsSaved = true;
            await LoadDataAsync();
            Mouse.OverrideCursor = null;
            MessageBox.Show("Сохранено");
            DenyAddingButtonVisibility = Visibility.Hidden;
        }

        private void OnDeleteCommand()
        {
            if (_chosenClient != null)
            {
                _clientsBLL.Redactor.Delete(_chosenClient);
                Clients.Remove(_chosenClient);
                ChosenClient = Clients.FirstOrDefault();
                IsSaved = false;
            }
        }

        private void OnAddCommand()
        {
            ChosenClient = new Client();
            Clients.Add(ChosenClient);
            addedClients.Add(ChosenClient);
            DenyAddingButtonVisibility = Visibility.Visible;
            IsSaved = false;
        }

        private void OnDenyCommand()
        {
            ChosenClient = _clients.FirstOrDefault();
            _clientsBLL.Redactor.ClearChanges();
            IsSaved = true;
        }

        private void OnDeleteOrderCommand()
        {
            if (_chosenClient != null && _chosenOrder != null)
            {
                _chosenClient.Orders.Remove(_chosenOrder);
                IsSaved = false;
            }
        }

        public Task LoadDataAsync()
        {
            return Task.Run(() =>
            {
                Clients = new(_clientsBLL.Redactor.GetAll());
                ChosenClient = Clients.FirstOrDefault();
                _logger.Log<ClientsViewModel>(LogSeverety.Info, $"All clients selected from database. Count {Clients.Count}.");
            });
        }
    }
}
