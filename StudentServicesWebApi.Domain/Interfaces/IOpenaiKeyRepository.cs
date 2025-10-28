using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface IOpenaiKeyRepository
{
    Task<OpenaiKey> CreateAsync(OpenaiKey openaiKey, CancellationToken cancellationToken = default);
    Task<OpenaiKey?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<OpenaiKey>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(List<OpenaiKey> Keys, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<OpenaiKey?> UpdateAsync(OpenaiKey openaiKey, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> KeyExistsAsync(string key, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<List<OpenaiKey>> CreateBulkAsync(List<OpenaiKey> openaiKeys, CancellationToken cancellationToken = default);
    Task<bool> DeleteBulkAsync(List<int> ids, CancellationToken cancellationToken = default);
    Task<OpenaiKey?> GetLeastUsedKeyAsync(CancellationToken cancellationToken = default);
    Task<List<OpenaiKey>> GetKeysByUsageRangeAsync(int minUsage, int maxUsage, CancellationToken cancellationToken = default);
    Task<List<OpenaiKey>> GetHighUsageKeysAsync(int usageThreshold, CancellationToken cancellationToken = default);
    Task<OpenaiKey?> IncrementUsageAsync(int id, int incrementBy = 1, CancellationToken cancellationToken = default);
    Task<List<OpenaiKey>> GetKeysOrderedByUsageAsync(bool ascending = true, CancellationToken cancellationToken = default);
    Task<int> GetTotalUsageAsync(CancellationToken cancellationToken = default);
    Task<double> GetAverageUsageAsync(CancellationToken cancellationToken = default);
    Task<List<OpenaiKey>> SearchByKeyPatternAsync(string pattern, CancellationToken cancellationToken = default);
    Task<(List<OpenaiKey> Keys, int TotalCount)> GetPagedWithFiltersAsync(
        int pageNumber, 
        int pageSize, 
        int? minUsage = null, 
        int? maxUsage = null, 
        DateTime? createdAfter = null, 
        DateTime? createdBefore = null,
        CancellationToken cancellationToken = default);
    Task<bool> ResetUsageCountAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ResetAllUsageCountsAsync(CancellationToken cancellationToken = default);
}
