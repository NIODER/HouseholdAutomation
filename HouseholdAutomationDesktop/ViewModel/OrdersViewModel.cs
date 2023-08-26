using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationDesktop.Utils;
using HouseholdAutomationDesktop.ViewModel.DialogsViewModel;
using HouseholdAutomationLogic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseholdAutomationDesktop.ViewModel
{
    public class OrdersViewModel : ViewModelBase
    {
        private ObservableCollection<Order> orders = new();
        private Order? selectedOrder;
        private ObservableCollection<OrdersToResource> resources = new();
        private OrdersToResource? selectedResource;

        private readonly IRedactor<Order> _ordersRedactor;
        private readonly IRedactor<OrdersToResource> _ordersToResourceRedactor;
        private readonly IWindowPresenter _windowPresenter;
        private bool saved = true;

        public RelayCommand AddResourceCommand { get; private set; }
        public RelayCommand RemoveResourceCommand { get; private set; }
        public RelayCommand AddOrderCommand { get; private set; }
        public RelayCommand DeleteOrderCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

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
                if (!IsSaved())
                {
                    _windowPresenter.Show<SaveChangesWarningViewModel>(OnSaveChangesDialogResult);
                }
                selectedResource = value;
                OnPropertyChanged(nameof(SelectedResource));
            }
        }


        public OrdersViewModel(IRedactorFactory redactorFactory, IWindowPresenter windowPresenter)
        {
            _windowPresenter = windowPresenter;
            _ordersRedactor = redactorFactory.Create<Order>();
            _ordersToResourceRedactor = redactorFactory.Create<OrdersToResource>();
            AddResourceCommand = new(OnAddResourceCommand);
            RemoveResourceCommand = new(OnRemoveResourceCommand);
            AddOrderCommand = new(OnAddOrderCommand);
            DeleteOrderCommand = new(OnDeleteOrderCommand);
            SaveCommand = new(OnSaveCommand);
        }

        private bool IsSaved()
        {
            return _ordersRedactor.IsChangesSaved() && _ordersToResourceRedactor.IsChangesSaved() && saved;
        }

        private async void OnSaveCommand()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SelectedOrder != null && !_ordersRedactor.GetAllFromDb().Contains(SelectedOrder))
            {
                try
                {
                    _ordersRedactor.Add(SelectedOrder);
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
                await _ordersRedactor.SaveChangesAsync();
                await _ordersToResourceRedactor.SaveChangesAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            saved = true;
            Mouse.OverrideCursor = null;
        }

        private void OnDeleteOrderCommand()
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }
            _ordersRedactor.DeleteOne(SelectedOrder);
            Orders.Remove(SelectedOrder);
            OnPropertyChanged(nameof(Orders));
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
        }

        private void OnRemoveResourceCommand()
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
            _ordersToResourceRedactor.DeleteOne(SelectedResource);
            Resources.Remove(SelectedResource);
            OnPropertyChanged(nameof(Resources));
        }

        private void OnAddResourceCommand()
        {
            _windowPresenter.Show<SelectResourceViewModel>(OnResourceSelected);
        }

        private void OnResourceSelected(object? sender, EventArgs e)
        {
            if (e is not SelectResourceViewModel.SelectedResourceEventArgs selectedResourceEventArgs)
            {
                throw new InvalidCastException($"EventArgs has type {e.GetType()}, not {typeof(SelectResourceViewModel)}");
            }
            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }
            var ordersToResource = new OrdersToResource()
            {
                OrderId = SelectedOrder.OrderId,
                ResourceId = selectedResourceEventArgs.SelectedResource.ResourceId,
                Count = selectedResourceEventArgs.Count
            };
            _ordersToResourceRedactor.Add(ordersToResource);
            Resources.Add(ordersToResource);
            OnPropertyChanged(nameof(Resources));
        }
    }
}
