using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
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

    public Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
        => _context.Set<T>().FindAsync([id], ct).AsTask();

    public async Task<List<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>>? transform = null, CancellationToken ct = default)
    {
        IQueryable<T> q = _context.Set<T>().AsQueryable();
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
        var entity = await GetByIdAsync(id, ct);
        if (entity == null) return false;
        
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync(ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
}
