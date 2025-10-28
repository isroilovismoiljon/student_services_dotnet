using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Repositories;
public class OpenaiKeyRepository(AppDbContext context) : IOpenaiKeyRepository
{
    private readonly AppDbContext _context = context;
    public async Task<OpenaiKey> CreateAsync(OpenaiKey openaiKey, CancellationToken cancellationToken = default)
    {
        _context.OpenaiKeys.Add(openaiKey);
        await _context.SaveChangesAsync(cancellationToken);
        return openaiKey;
    }
    public async Task<OpenaiKey?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.OpenaiKeys.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    public async Task<List<OpenaiKey>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OpenaiKeys.OrderBy(x => x.Id).ToListAsync(cancellationToken);
    }
    public async Task<(List<OpenaiKey> Keys, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.OpenaiKeys.AsQueryable();
        var totalCount = await query.CountAsync(cancellationToken);
        var keys = await query
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (keys, totalCount);
    }
    public async Task<OpenaiKey?> UpdateAsync(OpenaiKey openaiKey, CancellationToken cancellationToken = default)
    {
        var existingKey = await GetByIdAsync(openaiKey.Id, cancellationToken);
        if (existingKey == null) return null;
        existingKey.Key = openaiKey.Key;
        existingKey.UseCount = openaiKey.UseCount;
        existingKey.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return existingKey;
    }
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var openaiKey = await GetByIdAsync(id, cancellationToken);
        if (openaiKey == null) return false;
        _context.OpenaiKeys.Remove(openaiKey);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.OpenaiKeys.AnyAsync(x => x.Id == id, cancellationToken);
    }
    public async Task<bool> KeyExistsAsync(string key, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.OpenaiKeys.Where(x => x.Key == key);
        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);
        return await query.AnyAsync(cancellationToken);
    }
    public async Task<List<OpenaiKey>> CreateBulkAsync(List<OpenaiKey> openaiKeys, CancellationToken cancellationToken = default)
    {
        _context.OpenaiKeys.AddRange(openaiKeys);
        await _context.SaveChangesAsync(cancellationToken);
        return openaiKeys;
    }
    public async Task<bool> DeleteBulkAsync(List<int> ids, CancellationToken cancellationToken = default)
    {
        var keysToDelete = await _context.OpenaiKeys.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        if (!keysToDelete.Any()) return false;
        _context.OpenaiKeys.RemoveRange(keysToDelete);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<OpenaiKey?> GetLeastUsedKeyAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OpenaiKeys
            .OrderBy(x => x.UseCount)
            .ThenBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<List<OpenaiKey>> GetKeysByUsageRangeAsync(int minUsage, int maxUsage, CancellationToken cancellationToken = default)
    {
        return await _context.OpenaiKeys
            .Where(x => x.UseCount >= minUsage && x.UseCount <= maxUsage)
            .OrderBy(x => x.UseCount)
            .ToListAsync(cancellationToken);
    }
    public async Task<List<OpenaiKey>> GetHighUsageKeysAsync(int usageThreshold, CancellationToken cancellationToken = default)
    {
        return await _context.OpenaiKeys
            .Where(x => x.UseCount >= usageThreshold)
            .OrderByDescending(x => x.UseCount)
            .ToListAsync(cancellationToken);
    }
    public async Task<OpenaiKey?> IncrementUsageAsync(int id, int incrementBy = 1, CancellationToken cancellationToken = default)
    {
        var key = await GetByIdAsync(id, cancellationToken);
        if (key == null) return null;
        key.UseCount += incrementBy;
        key.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return key;
    }
    public async Task<List<OpenaiKey>> GetKeysOrderedByUsageAsync(bool ascending = true, CancellationToken cancellationToken = default)
    {
        var query = _context.OpenaiKeys.AsQueryable();
        return ascending 
            ? await query.OrderBy(x => x.UseCount).ThenBy(x => x.Id).ToListAsync(cancellationToken)
            : await query.OrderByDescending(x => x.UseCount).ThenBy(x => x.Id).ToListAsync(cancellationToken);
    }
    public async Task<int> GetTotalUsageAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OpenaiKeys.SumAsync(x => x.UseCount, cancellationToken);
    }
    public async Task<double> GetAverageUsageAsync(CancellationToken cancellationToken = default)
    {
        var keys = await _context.OpenaiKeys.ToListAsync(cancellationToken);
        return keys.Any() ? keys.Average(x => x.UseCount) : 0.0;
    }
    public async Task<List<OpenaiKey>> SearchByKeyPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        return await _context.OpenaiKeys
            .Where(x => x.Key.Contains(pattern))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }
    public async Task<(List<OpenaiKey> Keys, int TotalCount)> GetPagedWithFiltersAsync(
        int pageNumber, 
        int pageSize, 
        int? minUsage = null, 
        int? maxUsage = null, 
        DateTime? createdAfter = null, 
        DateTime? createdBefore = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.OpenaiKeys.AsQueryable();
        if (minUsage.HasValue)
            query = query.Where(x => x.UseCount >= minUsage.Value);
        if (maxUsage.HasValue)
            query = query.Where(x => x.UseCount <= maxUsage.Value);
        if (createdAfter.HasValue)
            query = query.Where(x => x.CreatedAt >= createdAfter.Value);
        if (createdBefore.HasValue)
            query = query.Where(x => x.CreatedAt <= createdBefore.Value);
        var totalCount = await query.CountAsync(cancellationToken);
        var keys = await query
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (keys, totalCount);
    }
    public async Task<bool> ResetUsageCountAsync(int id, CancellationToken cancellationToken = default)
    {
        var key = await GetByIdAsync(id, cancellationToken);
        if (key == null) return false;
        key.UseCount = 0;
        key.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<bool> ResetAllUsageCountsAsync(CancellationToken cancellationToken = default)
    {
        await _context.OpenaiKeys.ExecuteUpdateAsync(
            x => x.SetProperty(k => k.UseCount, 0)
                  .SetProperty(k => k.UpdatedAt, DateTime.UtcNow),
            cancellationToken);
        return true;
    }
}
