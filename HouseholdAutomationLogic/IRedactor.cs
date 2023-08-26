namespace HouseholdAutomationLogic
{
    public interface IRedactor<T> where T : class
    {
        public List<T> GetAllFromDb();
        public Task<List<T>> GetAllFromDbAsync();
        public List<T> GetByPredicate(Func<T, bool> predicate);
        public void DeleteOne(T entity);
        public Task DeleteManyAsync(Func<T, bool> predicate);
        public void UpdateOne(T entity);
        public T InsertOneAndSave(T entity);
        public Task<T> InsertOneAndSaveAsync(T entity);
        public void Add(T entity);
        public void SaveChanges();
        public Task SaveChangesAsync();
        public bool IsChangesSaved();
    }
}
