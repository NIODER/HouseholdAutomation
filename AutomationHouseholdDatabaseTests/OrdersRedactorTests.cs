using AutomationHouseholdDatabase.Models;
using HouseholdAutomationDesktop.Model.DbEntityRedactors;
using HouseholdAutomationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationHouseholdDatabaseTests
{
    public class OrdersRedactorTests
    {
        private readonly IRedactor<Order> ordersRedactor;
        private readonly int clientId;

        public OrdersRedactorTests(IRedactor<Order> ordersRedactor, IRedactor<Client> clientsRedactor)
        {
            this.ordersRedactor = ordersRedactor;
            clientId = clientsRedactor.CreateAndSaveAsync(new Client()
            {
                ClientName = "orderClient",
                Address = "address",
                Phone = "phone"
            }).Result.ClientId;
        }

        [Fact]
        public void CreateTest()
        {
            var order = new Order()
            {
                ClientId = clientId
            };
            order = ordersRedactor.Create(order);
            Assert.NotNull(order);
            ordersRedactor.SaveChanges();
            Assert.NotEqual(default(long), order.OrderId);
            var order1 = ordersRedactor.GetByPredicate(o => o.OrderId == order.OrderId).FirstOrDefault();
            Assert.NotNull(order1);
            Assert.Equal(order, order1);
        }

        [Fact]
        public async void CreateAndSaveAsyncTest()
        {
            var order = new Order()
            {
                ClientId = clientId
            };
            order = await ordersRedactor.CreateAndSaveAsync(order);
            Assert.NotEqual(default, order.OrderId);
            var order1 = ordersRedactor.GetByPredicate(o => o.OrderId == order.OrderId).FirstOrDefault();
            Assert.NotNull(order1);
            Assert.Equal(order, order1);
        }

        [Fact]
        public void GetAllTest()
        {
            var orders = ordersRedactor.GetAll();
            Assert.NotNull(orders);
        }

        [Fact]
        public void GetByPredicateTest()
        {
            var orders = ordersRedactor.GetByPredicate(o => o.OrderId != 0);
            Assert.NotNull(orders);
        }

        [Fact]
        public async Task UpdateTestAsync()
        {
            var order = new Order()
            {
                ClientId = clientId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now)
            };
            order = await ordersRedactor.CreateAndSaveAsync(order);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now), order.OrderDate);
            var orderDate1 = order.OrderDate.AddDays(1);
            order.OrderDate = orderDate1;
            order = ordersRedactor.Update(order);
            ordersRedactor.SaveChanges();
            Assert.Equal(orderDate1, order.OrderDate);
        }

        [Fact]
        public async void UpdateAndSaveAsyncTest()
        {
            var order = new Order()
            {
                ClientId = clientId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now)
            };
            order = await ordersRedactor.CreateAndSaveAsync(order);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now), order.OrderDate);
            var orderDate1 = order.OrderDate.AddDays(1);
            order.OrderDate = orderDate1;
            order = await ordersRedactor.UpdateAndSaveAsync(order);
            Assert.Equal(orderDate1, order.OrderDate);
        }

        [Fact]
        public async void DeleteTest()
        {
            var order = new Order()
            {
                ClientId = clientId
            };
            order = await ordersRedactor.CreateAndSaveAsync(order);
            Assert.NotNull(order);
            ordersRedactor.Delete(order);
            ordersRedactor.SaveChanges();
            Assert.DoesNotContain(order, ordersRedactor.GetAll());
        }

        [Fact]
        public async void DeleteAndSaveAsyncTest()
        {
            var order = new Order()
            {
                ClientId = clientId
            };
            order = await ordersRedactor.CreateAndSaveAsync(order);
            Assert.NotNull(order);
            Assert.Contains(ordersRedactor.GetAll(), o => o.OrderId == order.OrderId);
            await ordersRedactor.DeleteAndSaveAsync(order);
            Assert.DoesNotContain(order, ordersRedactor.GetAll());
        }

        [Fact]
        public async void ClearChangesTest()
        {
            var order = new Order()
            {
                ClientId = clientId
            };
            order = await ordersRedactor.CreateAndSaveAsync(order);
            ordersRedactor.Delete(order);
            ordersRedactor.ClearChanges();
            Assert.Contains(ordersRedactor.GetAll(), o => o.OrderId == order.OrderId);
        }
    }
}
