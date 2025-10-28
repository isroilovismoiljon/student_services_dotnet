using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Repositories;
public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context = context;
    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _context.Set<T>().AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity;
    }
    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        await _context.Set<T>().AddRangeAsync(entities, ct);
        await _context.SaveChangesAsync(ct);
    }
    public async Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
    {
        var entity = await _context.Set<T>().FindAsync([id], ct);
        if (entity is BaseEntity baseEntity && baseEntity.IsDeleted)
            return null;
        return entity;
    }
    public async Task<List<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>>? transform = null, CancellationToken ct = default)
    {
        IQueryable<T> q = _context.Set<T>().AsQueryable();
        if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            q = q.Where(e => !((BaseEntity)(object)e).IsDeleted);
        }
        if (transform is not null) q = transform(q);
        return await q.ToListAsync(ct);
    }
    public async Task<T> UpdateAsync(T entity, CancellationToken ct = default)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }
    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.Set<T>().FindAsync([id], ct);
        if (entity == null) return false;
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = true;
            baseEntity.DeletedAt = DateTime.UtcNow;
            _context.Set<T>().Update(entity);
        }
        else
        {
            _context.Set<T>().Remove(entity);
        }
        await _context.SaveChangesAsync(ct);
        return true;
    }
    public async Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = true;
            baseEntity.DeletedAt = DateTime.UtcNow;
            _context.Set<T>().Update(entity);
        }
        else
        {
            _context.Set<T>().Remove(entity);
        }
        await _context.SaveChangesAsync(ct);
    }
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
}
