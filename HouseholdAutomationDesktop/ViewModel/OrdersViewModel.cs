using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationDesktop.Utils;
using HouseholdAutomationDesktop.ViewModel.DialogsViewModel;
using HouseholdAutomationLogic;
using HouseholdAutomationLogic.BLL;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HouseholdAutomationDesktop.ViewModel
{
    public class OrdersViewModel : SaveableViewModelBase, IDataLoading
    {
        private ObservableCollection<Order> orders = new();
        private Order? selectedOrder;
        private ObservableCollection<OrdersToResource> resources = new();
        private OrdersToResource? selectedResource;

        private readonly OrdersBLL _ordersBLL;
        private readonly IDbEntityRedactor<OrdersToResource> _ordersToResourcesRedactor;
        private readonly IWindowPresenter _windowPresenter;

        public RelayCommand AddResourceCommand { get; private set; }
        public RelayCommand RemoveResourceCommand { get; private set; }
        public RelayCommand AddOrderCommand { get; private set; }
        public RelayCommand DeleteOrderCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand ChoseClientCommand { get; private set; }

        public ObservableCollection<Order> Orders
        {
            get => orders;
            set
            {
                orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        private void OnSaveChangesDialogResult(object? sender, EventArgs e)
        {
            if (e is SaveChangesWarningViewModel.DialogWindowEventArgs dialogResult)
            {
                if (dialogResult.SaveChanges)
                {
                    OnSaveCommand();
                }
            }
        }

        public Order? SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
                _ = LoadResourcesAsync();
            }
        }

        public ObservableCollection<OrdersToResource> Resources 
        {
            get => resources;
            set
            {
                resources = value;
                OnPropertyChanged(nameof(Resources));
            }
        }

        public OrdersToResource? SelectedResource
        {
            get => selectedResource;
            set
            {
                if (!IsSaved)
                {
                    _windowPresenter.Show<SaveChangesWarningViewModel>(OnSaveChangesDialogResult);
                }
                selectedResource = value;
                OnPropertyChanged(nameof(SelectedResource));
            }
        }

        public OrdersViewModel(OrdersBLL ordersBLL, IDbEntityRedactor<OrdersToResource> ordersToResourcesRedactor, IWindowPresenter windowPresenter)
        {
            _windowPresenter = windowPresenter;
            _ordersBLL = ordersBLL;
            _ordersToResourcesRedactor = ordersToResourcesRedactor;
            AddResourceCommand = new(OnAddResourceCommand);
            RemoveResourceCommand = new(OnRemoveResourceCommandAsync);
            AddOrderCommand = new(OnAddOrderCommand);
            DeleteOrderCommand = new(OnDeleteOrderCommand);
            SaveCommand = new(OnSaveCommand);
            ChoseClientCommand = new(OnChoseClientCommand);
        }

        private void OnChoseClientCommand()
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }
            _windowPresenter.Show<ChoseClientViewModel>(OnClientChosen);
        }

        private void OnClientChosen(object? sender, EventArgs e)
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }
            if (e is ChoseClientViewModel.ChoseClientEventArgs choseClientEventArgs)
            {
                SelectedOrder.ClientId = choseClientEventArgs.SelectedClient.ClientId;
                SelectedOrder = _ordersBLL.Redactor.Update(SelectedOrder);
                IsSaved = false;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        private async void OnSaveCommand()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SelectedOrder != null && !_ordersBLL.Redactor.GetAll().Contains(SelectedOrder))
            {
                try
                {
                    _ordersBLL.Redactor.Create(SelectedOrder);
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show($"Нужно указать клиента.");
                    Mouse.OverrideCursor = null;
                    return;
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show($"Не найдено клиента с id {SelectedOrder.ClientId}");
                    Mouse.OverrideCursor = null;
                    return;
                }
            }
            try
            {
                await _ordersBLL.Redactor.SaveChangesAsync();
                IsSaved = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void OnDeleteOrderCommand()
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }
            SelectedOrder = _ordersBLL.Redactor.Create(SelectedOrder);
            Orders.Remove(SelectedOrder);
            OnPropertyChanged(nameof(Orders));
            IsSaved = false;
        }

        private void OnAddOrderCommand()
        {
            
            var order = new Order()
            {
                OrderDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };
            Orders.Add(order);
            SelectedOrder = order;
            OnPropertyChanged(nameof(Orders));
            OnPropertyChanged(nameof(Resources));
            OnPropertyChanged(nameof(SelectedResource));
            IsSaved = false;
        }

        private async void OnRemoveResourceCommandAsync()
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }
            if (SelectedResource == null)
            {
                MessageBox.Show("Выберите ресурс.");
                return;
            }
            Resources.Remove(SelectedResource);
            await _ordersBLL.RemoveResourceAsync(SelectedOrder, SelectedResource.Resource);
            OnPropertyChanged(nameof(Resources));
            IsSaved = true;
        }

        private void OnAddResourceCommand()
        {
            _windowPresenter.Show<SelectResourceViewModel>(OnResourceSelectedAsync);
        }

        private async void OnResourceSelectedAsync(object? sender, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (e is not SelectResourceViewModel.SelectedResourceEventArgs selectedResourceEventArgs)
            {
                throw new InvalidCastException($"EventArgs has type {e.GetType()}, not {typeof(SelectResourceViewModel)}");
            }
            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }
            if (!_ordersBLL.Redactor.GetByPredicate(o => o.OrderId == SelectedOrder.OrderId).Any())
            {
                SelectedOrder = _ordersBLL.Redactor.Create(SelectedOrder);
            }
            var ordersToResources = await _ordersBLL.AddResourceAsync(SelectedOrder, selectedResourceEventArgs.SelectedResource, selectedResourceEventArgs.Count);
            Mouse.OverrideCursor = null;
            Resources.Add(ordersToResources);
            IsSaved = false;
            OnPropertyChanged(nameof(Resources));
        }

        public async Task LoadDataAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            await Task.Run(() =>
            {
                Orders = new(_ordersBLL.Redactor.GetAll());
                SelectedOrder = Orders.FirstOrDefault();
            });
            Mouse.OverrideCursor = null;
        }

        private async Task LoadResourcesAsync()
        {
            Mouse.OverrideCursor = Cursors.ArrowCD;
            await Task.Run(() =>
            {
                Resources = new(_ordersToResourcesRedactor.GetByPredicate(r => r.OrderId == SelectedOrder?.OrderId));
            });
            Mouse.OverrideCursor = null;
        }
    }
}
