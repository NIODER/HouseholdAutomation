using HouseholdAutomationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutomationHouseholdDatabase.Data
{
    public abstract class Redactor<T> : IRedactor<T> where T : class
    {
        protected readonly HouseholdDbContext db;

        public Redactor(HouseholdDbContext db)
        {
            this.db = db;
        }

        public void ClearChanges()
        {
            db.ChangeTracker.Clear();
        }

        public T Create(T entity)
        {
            return db.Set<T>().Add(entity).Entity;
        }

        public async Task<T> CreateAndSaveAsync(T entity, CancellationToken cancellationToken = default)
        {
            var dbEntity = await db.Set<T>().AddAsync(entity, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            return dbEntity.Entity;
        }

        public void Delete(T entity)
        {
            db.Set<T>().Remove(entity);
        }

        public Task DeleteAndSaveAsync(T entity, CancellationToken cancellationToken = default)
        {
            db.Set<T>().Remove(entity);
            return db.SaveChangesAsync(cancellationToken);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return db.Set<T>().ToList();
        }

        public virtual IEnumerable<T> GetByPredicate(Func<T, bool> predicate)
        {
            return db.Set<T>().Where(predicate).ToList();
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return db.SaveChangesAsync(cancellationToken);
        }

        public T Update(T entity)
        {
            return db.Set<T>().Update(entity).Entity;
        }

        public async Task<T> UpdateAndSaveAsync(T entity, CancellationToken cancellationToken = default)
        {
            var updatedEntity = db.Set<T>().Update(entity);
            await db.SaveChangesAsync(cancellationToken);
            return updatedEntity.Entity;
        }
    }
}
