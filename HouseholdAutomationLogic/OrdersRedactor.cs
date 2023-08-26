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
    public class OrdersRedactor : IRedactor<Order>
    {
        private readonly HouseholdDbContext db;
        private bool _saved = true;

        public OrdersRedactor(HouseholdDbContext db)
        {
            this.db = db;
        }

        public void Add(Order entity)
        {
            if (entity.ClientId == default)
            {
                throw new NotSupportedException("ClientId must be setted");
            }
            if (!db.Clients.Any(c => c.ClientId == entity.ClientId))
            {
                throw new NullReferenceException($"There is not client with id {entity.ClientId}");
            }
            db.Orders.Add(entity);
            _saved = false;
        }

        public Task DeleteManyAsync(Func<Order, bool> predicate)
        {
            return db.Orders.AsQueryable().Where(predicate).AsQueryable().ExecuteDeleteAsync();
        }

        public void DeleteOne(Order entity)
        {
            if (db.Orders.Any(e => e.OrderId == entity.OrderId))
            {
                db.Orders.Remove(entity);
            }
            _saved = false;
        }

        public List<Order> GetAllFromDb()
        {
            return db.Orders.ToList();
        }

        public Task<List<Order>> GetAllFromDbAsync()
        {
            return db.Orders.ToListAsync();
        }

        public List<Order> GetByPredicate(Func<Order, bool> predicate)
        {
            return db.Orders.AsQueryable().Where(predicate).ToList();
        }

        public Order InsertOneAndSave(Order entity)
        {
            var order = db.Orders.Add(entity);
            _saved = false;
            db.SaveChanges();
            _saved = true;
            return order.Entity;
        }

        public async Task<Order> InsertOneAndSaveAsync(Order entity)
        {
            var order = db.Orders.Add(entity);
            _saved = false;
            await db.SaveChangesAsync();
            _saved = true;
            return order.Entity;
        }

        public bool IsChangesSaved()
        {
            return _saved;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }

        public void UpdateOne(Order entity)
        {
            db.Orders.Update(entity);
            _saved = false;
        }
    }
}
