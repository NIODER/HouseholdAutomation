using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HouseholdAutomationDesktop.ViewModel.DialogsViewModel
{
    public class ChoseClientViewModel : ViewModelBase, IDialogViewModel, IDataLoading
    {
        public class ChoseClientEventArgs : EventArgs
        {
            public Client SelectedClient { get; set; }

            public ChoseClientEventArgs(Client selectedClient)
            {
                SelectedClient = selectedClient;
            }
        }

        private readonly IRedactor<Client> _clientRedactor;

        public event EventHandler<EventArgs>? OnDialogResult;

        private Client? _selectedClient;
        private List<Client> _dbClients = new();
        private ObservableCollection<Client> _clients = new();
        private string? _searchQuery; 

        public RelayCommand ChoseClientCommand { get; private set; }
        public Client? SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
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

        public string? SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                if (string.IsNullOrEmpty(_searchQuery))
                {
                    Clients = new(_dbClients);
                }
                else
                {
                    Clients = new(_dbClients.Where(c =>
                    {
                        return c.ClientId.ToString().StartsWith(_searchQuery) ||
                               c.ClientName.StartsWith(_searchQuery) ||
                               c.Phone.StartsWith(_searchQuery) ||
                               c.Address.StartsWith(_searchQuery);
                    }));
                }
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        public ChoseClientViewModel(IRedactor<Client> clientsRedactor)
        {
            _clientRedactor = clientsRedactor;
            ChoseClientCommand = new(OnChoseClientCommand);
        }

        private void OnChoseClientCommand()
        {
            if (SelectedClient == null)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }
            OnDialogResult?.Invoke(this, new ChoseClientEventArgs(SelectedClient));
        }

        public async Task LoadDataAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            await Task.Run(() =>
            {
                _dbClients = _clientRedactor.GetAll().ToList();
                Clients = new(_dbClients);
                SelectedClient = Clients.FirstOrDefault();
            });
            Mouse.OverrideCursor = null;
        }
    }
}
