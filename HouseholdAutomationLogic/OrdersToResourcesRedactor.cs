using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseholdAutomationLogic
{
    public class OrdersToResourcesRedactor : IRedactor<OrdersToResource>
    {
        private readonly HouseholdDbContext db;
        private bool _saved = true;

        public OrdersToResourcesRedactor(HouseholdDbContext db)
        {
            this.db = db;
        }

        public void Add(OrdersToResource entity)
        {
            db.OrdersToResources.Add(entity);
            _saved = false;
        }

        public Task DeleteManyAsync(Func<OrdersToResource, bool> predicate)
        {
            return db.OrdersToResources
                .AsQueryable()
                .Where(predicate)
                .AsQueryable()
                .ExecuteDeleteAsync();
        }

        public void DeleteOne(OrdersToResource entity)
        {
            db.OrdersToResources.Remove(entity);
            _saved = false;
        }

        public List<OrdersToResource> GetAllFromDb()
        {
            return db.OrdersToResources.ToList();
        }

        public Task<List<OrdersToResource>> GetAllFromDbAsync()
        {
            return db.OrdersToResources.AsQueryable().ToListAsync();
        }

        public List<OrdersToResource> GetByPredicate(Func<OrdersToResource, bool> predicate)
        {
            return db.OrdersToResources.Where(predicate).ToList();
        }

        public OrdersToResource InsertOneAndSave(OrdersToResource entity)
        {
            var orderToResource = db.OrdersToResources.Add(entity);
            _saved = false;
            db.SaveChanges();
            _saved = true;
            return orderToResource.Entity;
        }

        public async Task<OrdersToResource> InsertOneAndSaveAsync(OrdersToResource entity)
        {
            var orderToResource = await db.OrdersToResources.AddAsync(entity);
            _saved = false;
            await db.SaveChangesAsync();
            return orderToResource.Entity;
        }

        public bool IsChangesSaved()
        {
            return _saved;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
            _saved = true;
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
            _saved = true;
        }

        public void UpdateOne(OrdersToResource entity)
        {
            db.OrdersToResources.Update(entity);
            _saved = false;
        }
    }
}
