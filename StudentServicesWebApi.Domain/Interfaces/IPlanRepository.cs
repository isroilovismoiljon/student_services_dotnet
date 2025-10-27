using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Domain.Interfaces;

public interface IPlanRepository
{
    Task<Plan> CreateAsync(Plan plan, CancellationToken cancellationToken = default);
    Task<Plan?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Plan>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(List<Plan> Plans, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<Plan?> UpdateAsync(Plan plan, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}