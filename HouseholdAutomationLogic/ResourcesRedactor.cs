using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseholdAutomationLogic
{
    public class ResourcesRedactor : IRedactor<Resource>
    {
        private readonly HouseholdDbContext db;

        public ResourcesRedactor(HouseholdDbContext db)
        {
            this.db = db;
        }

        public void Add(Resource entity)
        {
            db.Resources.Add(entity);
        }

        public Task DeleteManyAsync(Func<Resource, bool> predicate)
        {
            return db.Resources.Where(predicate).AsQueryable().ExecuteDeleteAsync();
        }

        public void DeleteOne(Resource entity)
        {
            db.Resources.Remove(entity);
        }

        public List<Resource> GetAllFromDb()
        {
            return db.Resources.ToList();
        }

        public Task<List<Resource>> GetAllFromDbAsync()
        {
            return db.Resources.ToListAsync();
        }

        public List<Resource> GetByPredicate(Func<Resource, bool> predicate)
        {
            return db.Resources.Where(predicate).ToList();
        }

        public Task<List<Resource>> GetByPredicateAsync(Func<Resource, bool> predicate)
        {
            return db.Resources.Where(predicate).AsQueryable().ToListAsync();
        }

        public Resource InsertOneAndSave(Resource entity)
        {
            var added = db.Resources.Add(entity);
            db.SaveChanges();
            return added.Entity;
        }

        public async Task<Resource> InsertOneAndSaveAsync(Resource entity)
        {
            var added = await db.Resources.AddAsync(entity);
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

        public void UpdateOne(Resource entity)
        {
            db.Update(entity);
        }
    }
}
