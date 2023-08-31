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
    public class ResourcesRedactorTests
    {
        private readonly IRedactor<Resource> resourcesRedactor;

        public ResourcesRedactorTests(IRedactor<Resource> resourcesRedactor)
        {
            this.resourcesRedactor = resourcesRedactor;
        }

        [Fact]
        public void CreateTest()
        {
            var resource = new Resource()
            {
                ResourceName = "resource"
            };
            resource = resourcesRedactor.Create(resource);
            Assert.NotNull(resource);
            resourcesRedactor.SaveChanges();
            Assert.NotEqual(default(long), resource.ResourceId);
            var resource1 = resourcesRedactor.GetByPredicate(r => r.ResourceId == resource.ResourceId).FirstOrDefault();
            Assert.NotNull(resource1);
            Assert.Equal(resource, resource1);
        }

        [Fact]
        public async void CreateAndSaveAsyncTest()
        {
            var resource = new Resource()
            {
                ResourceName = "resource"
            };
            resource = await resourcesRedactor.CreateAndSaveAsync(resource);
            Assert.NotEqual(default, resource.ResourceId);
            var resource1 = resourcesRedactor.GetByPredicate(r => r.ResourceId == resource.ResourceId).FirstOrDefault();
            Assert.NotNull(resource1);
            Assert.Equal(resource, resource1);
        }

        [Fact]
        public void GetAllTest()
        {
            var orders = resourcesRedactor.GetAll();
            Assert.NotNull(orders);
        }

        [Fact]
        public void GetByPredicateTest()
        {
            var orders = resourcesRedactor.GetByPredicate(r => r.ResourceId != 0);
            Assert.NotNull(orders);
        }

        [Fact]
        public async Task UpdateTestAsync()
        {
            var resource = new Resource()
            {
                ResourceName = "resource"
            };
            resource = await resourcesRedactor.CreateAndSaveAsync(resource);
            Assert.Equal("resource", resource.ResourceName);
            resource.ResourceName = "resource1";
            resource = resourcesRedactor.Update(resource);
            resourcesRedactor.SaveChanges();
            Assert.Equal("resource1", resource.ResourceName);
        }

        [Fact]
        public async void UpdateAndSaveAsyncTest()
        {
            var resource = new Resource()
            {
                ResourceName = "resource"
            };
            resource = await resourcesRedactor.CreateAndSaveAsync(resource);
            Assert.Equal("resource", resource.ResourceName);
            resource.ResourceName = "resource1";
            resource = await resourcesRedactor.UpdateAndSaveAsync(resource);
            Assert.Equal("resource1", resource.ResourceName);
        }

        [Fact]
        public async void DeleteTest()
        {
            var resource = new Resource()
            {
                ResourceName = "resource"
            };
            resource = await resourcesRedactor.CreateAndSaveAsync(resource);
            Assert.NotNull(resource);
            resourcesRedactor.Delete(resource);
            resourcesRedactor.SaveChanges();
            Assert.DoesNotContain(resource, resourcesRedactor.GetAll());
        }

        [Fact]
        public async void DeleteAndSaveAsyncTest()
        {
            var resource = new Resource()
            {
                ResourceName = "resource"
            };
            resource = await resourcesRedactor.CreateAndSaveAsync(resource);
            Assert.NotNull(resource);
            Assert.Contains(resourcesRedactor.GetAll(), r => r.ResourceId == resource.ResourceId);
            await resourcesRedactor.DeleteAndSaveAsync(resource);
            Assert.DoesNotContain(resource, resourcesRedactor.GetAll());
        }

        [Fact]
        public async void ClearChangesTest()
        {
            var resource = new Resource()
            {
                ResourceName = "resource"
            };
            resource = await resourcesRedactor.CreateAndSaveAsync(resource);
            resourcesRedactor.Delete(resource);
            resourcesRedactor.ClearChanges();
            Assert.Contains(resourcesRedactor.GetAll(), r => r.ResourceId == resource.ResourceId);
        }
    }
}
