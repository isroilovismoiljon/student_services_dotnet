namespace StudentServicesWebApi.Domain.Interfaces;
public interface IGenericRepository<T> where T : class
{
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    Task<T?> GetByIdAsync(object id, CancellationToken ct = default);
    Task<List<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>>? transform = null, CancellationToken ct = default);
    Task<T> UpdateAsync(T entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
