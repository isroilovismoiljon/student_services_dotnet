using StudentServicesWebApi.Application.DTOs.Admin;
using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Infrastructure.Interfaces;
public interface IAdminActionService
{
    Task<AdminActionDto> UpdateUserRoleAsync(UpdateUserRoleDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default);
    Task<AdminActionDto> AddBalanceAsync(ModifyBalanceDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default);
    Task<AdminActionDto> SubtractBalanceAsync(ModifyBalanceDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default);
    Task<AdminActionDto?> GetAdminActionByIdAsync(int actionId, CancellationToken ct = default);
    Task<(List<AdminActionSummaryDto> Actions, int TotalCount)> GetPagedAdminActionsAsync(
        int pageNumber, 
        int pageSize, 
        AdminActionType? actionType = null, 
        CancellationToken ct = default);
    Task<List<AdminActionSummaryDto>> GetAdminActionsByAdminIdAsync(int adminId, AdminActionType? actionType = null, CancellationToken ct = default);
    Task<List<AdminActionSummaryDto>> GetAdminActionsByUserIdAsync(int userId, AdminActionType? actionType = null, CancellationToken ct = default);
    Task<List<AdminActionSummaryDto>> GetRecentActionsAsync(int count = 50, CancellationToken ct = default);
}
