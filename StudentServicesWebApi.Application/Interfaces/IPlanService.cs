using StudentServicesWebApi.Application.DTOs.Plan;

namespace StudentServicesWebApi.Application.Interfaces;

public interface IPlanService
{
    Task<PlanDto> CreateAsync(CreatePlanDto createDto, CancellationToken cancellationToken = default);
    Task<PlanDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<PlanDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(List<PlanSummaryDto> Plans, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<PlanDto?> UpdateAsync(int id, UpdatePlanDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}