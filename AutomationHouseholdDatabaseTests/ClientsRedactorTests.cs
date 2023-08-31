using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Data.DbEntityRedactors;
using AutomationHouseholdDatabase.Models;
using HouseholdAutomationDesktop.Model.DbEntityRedactors;
using HouseholdAutomationLogic;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationHouseholdDatabaseTests
{
    public class ClientsRedactorTests
    {
        private readonly IRedactor<Client> clientsRedactor;

        public ClientsRedactorTests(IRedactor<Client> clientsRedactor)
        {
            this.clientsRedactor = clientsRedactor;
        }

        [Fact]
        public void CreateTest()
        {
            Client client = new Client()
            {
                ClientName = "name",
                Address = "address",
                Phone = "123"
            };
            client = clientsRedactor.Create(client);
            Assert.NotNull(client);
            clientsRedactor.SaveChanges();
            Assert.NotEqual(default(long), client.ClientId);
            var client1 = clientsRedactor.GetByPredicate(c => c.ClientId == client.ClientId).FirstOrDefault();
            Assert.NotNull(client1);
            Assert.Equal(client, client1);
        }

        [Fact]
        public async void CreateAndSaveAsyncTest()
        {
            Client client = new Client()
            {
                ClientName = "name",
                Address = "address",
                Phone = "123"
            };
            client = await clientsRedactor.CreateAndSaveAsync(client);
            Assert.NotEqual(default(long), client.ClientId);
            var client1 = clientsRedactor.GetByPredicate(c => c.ClientId == client.ClientId).FirstOrDefault();
            Assert.NotNull(client1);
            Assert.Equal(client, client1);
        }

        [Fact]
        public void GetAllTest()
        {
            var clients = clientsRedactor.GetAll();
            Assert.NotNull(clients);
        }

        [Fact]
        public void GetByPredicateTest()
        {
            var clients = clientsRedactor.GetByPredicate(c => c.ClientId != 0);
            Assert.NotNull(clients);
        }

        [Fact]
        public async Task UpdateTestAsync()
        {
            var client = new Client()
            {
                ClientName = "name",
                Address = "address",
                Phone = "123"
            };
            client = await clientsRedactor.CreateAndSaveAsync(client);
            Assert.Equal("name", client.ClientName);
            client.ClientName = "name1";
            client = clientsRedactor.Update(client);
            clientsRedactor.SaveChanges();
            Assert.Equal("name1", client.ClientName);
        }

        [Fact]
        public async void UpdateAndSaveAsyncTest()
        {
            var client = new Client()
            {
                ClientName = "name",
                Address = "address",
                Phone = "123"
            };
            client = await clientsRedactor.CreateAndSaveAsync(client);
            Assert.Equal("name", client.ClientName);
            client.ClientName = "name1";
            client = await clientsRedactor.UpdateAndSaveAsync(client);
            Assert.Equal("name1", client.ClientName);
        }

        [Fact]
        public async void DeleteTest()
        {
            var client = new Client()
            {
                ClientName = "name",
                Address = "address",
                Phone = "123"
            };
            client = await clientsRedactor.CreateAndSaveAsync(client);
            Assert.NotNull(client);
            clientsRedactor.Delete(client);
            clientsRedactor.SaveChanges();
            Assert.DoesNotContain(client, clientsRedactor.GetAll());
        }

        [Fact]
        public async void DeleteAndSaveAsyncTest()
        {
            var client = new Client()
            {
                ClientName = "name",
                Address = "address",
                Phone = "123"
            };
            client = await clientsRedactor.CreateAndSaveAsync(client);
            Assert.NotNull(client);
            Assert.Contains(clientsRedactor.GetAll(), c => c.ClientId == client.ClientId);
            await clientsRedactor.DeleteAndSaveAsync(client);
            Assert.DoesNotContain(client, clientsRedactor.GetAll());
        }

        [Fact]
        public async void ClearChangesTest()
        {
            var client = new Client()
            {
                ClientName = "name",
                Address = "address",
                Phone = "123"
            };
            client = await clientsRedactor.CreateAndSaveAsync(client);
            clientsRedactor.Delete(client);
            clientsRedactor.ClearChanges();
            Assert.Contains(clientsRedactor.GetAll(), c => c.ClientId == client.ClientId);
        }
    }
}
