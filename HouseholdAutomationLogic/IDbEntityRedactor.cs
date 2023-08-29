namespace HouseholdAutomationLogic;

public interface IDbEntityRedactor<T> where T : class
{
    T Create(T client);
    Task<T> CreateAndSaveAsync(T entity, CancellationToken cancellationToken = default);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetByPredicate(Func<T, bool> predicate);
    T Update(T entity);
    Task<T> UpdateAndSaveAsync(T entity, CancellationToken cancellationToken = default);
    void Delete(T entity);
    Task DeleteAndSaveAsync(T entity, CancellationToken cancellationToken = default);
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void ClearChanges();
}
