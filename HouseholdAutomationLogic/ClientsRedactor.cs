using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseholdAutomationLogic
{
    public class ClientsRedactor : IRedactor<Client>
    {
        private readonly HouseholdDbContext db;
        private bool _saved = true;

        public ClientsRedactor(HouseholdDbContext householdDbContext)
        {
            db = householdDbContext;
        }

        public List<Client> GetAllFromDb()
        {
            return db.Clients.AsQueryable()
                .Include(c => c.Orders)
                .ToList();
        }

        public Task<List<Client>> GetAllFromDbAsync()
        {
            return db.Clients.AsQueryable()
                .Include(c => c.Orders)
                .ToListAsync();
        }

        public void DeleteOne(Client entity)
        {
            db.Clients.Remove(entity);
        }

        public void UpdateOne(Client entity)
        {
            db.Clients.Update(entity);
        }

        public Task DeleteManyAsync(Func<Client, bool> predicate)
        {
            return db.Clients.Where(predicate).AsQueryable().ExecuteDeleteAsync();
        }

        public Client InsertOneAndSave(Client entity)
        {
            var added = db.Clients.Add(entity);
            db.SaveChanges();
            return added.Entity;
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

        public List<Client> GetByPredicate(Func<Client, bool> predicate)
        {
            return db.Clients.Where(predicate).ToList();
        }

        public Task<List<Client>> GetByPredicateAsync(Func<Client, bool> predicate)
        {
            return db.Clients.Where(predicate).AsQueryable().ToListAsync();
        }

        public async Task<Client> InsertOneAndSaveAsync(Client entity)
        {
            var added = await db.Clients.AddAsync(entity);
            await db.SaveChangesAsync();
            return added.Entity;
        }

        public bool IsChangesSaved()
        {
            return _saved;
        }

        public void Add(Client entity)
        {
            db.Clients.Add(entity);
            _saved = false;
        }
    }
}