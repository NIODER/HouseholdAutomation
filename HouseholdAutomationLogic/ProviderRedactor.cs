using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseholdAutomationLogic
{
    public class ProviderRedactor : IRedactor<Provider>
    {
        private readonly HouseholdDbContext db;

        public ProviderRedactor(HouseholdDbContext db)
        {
            this.db = db;
        }

        public void Add(Provider entity)
        {
            db.Providers.Add(entity);
        }

        public Task DeleteManyAsync(Func<Provider, bool> predicate)
        {
            return db.Providers.Where(predicate).AsQueryable().ExecuteDeleteAsync();
        }

        public void DeleteOne(Provider entity)
        {
            db.Providers.Remove(entity);
        }

        public List<Provider> GetAllFromDb()
        {
            return db.Providers
                .AsQueryable()
                .Include(p => p.ProviderToResources)
                .ToList();
        }

        public Task<List<Provider>> GetAllFromDbAsync()
        {
            return db.Providers
                .AsQueryable()
                .Include(p => p.ProviderToResources)
                .ToListAsync();
        }

        public List<Provider> GetByPredicate(Func<Provider, bool> predicate)
        {
            return db.Providers.Where(predicate).ToList();
        }

        public Task<List<Provider>> GetByPredicateAsync(Func<Provider, bool> predicate)
        {
            return db.Providers.Where(predicate).AsQueryable().ToListAsync();
        }

        public Provider InsertOneAndSave(Provider entity)
        {
            var added = db.Providers.Add(entity);
            db.SaveChanges();
            return added.Entity;
        }

        public async Task<Provider> InsertOneAndSaveAsync(Provider entity)
        {
            var added = await db.Providers.AddAsync(entity);
            await db.SaveChangesAsync();
            return added.Entity;
        }

        public bool IsChangesSaved()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }

        public void UpdateOne(Provider entity)
        {
            db.Update(entity);
        }
    }
}
