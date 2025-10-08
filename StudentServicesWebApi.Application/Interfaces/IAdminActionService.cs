using StudentServicesWebApi.Application.DTOs.Admin;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface IAdminActionService
{
    /// Updates user role and logs the action
    Task<AdminActionDto> UpdateUserRoleAsync(UpdateUserRoleDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default);
    
    /// Adds balance to user account and logs the action
    Task<AdminActionDto> AddBalanceAsync(ModifyBalanceDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default);
    
    /// <summary>
    /// Subtracts balance from user account and logs the action
    /// </summary>
    Task<AdminActionDto> SubtractBalanceAsync(ModifyBalanceDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default);
    
    /// Gets admin action by ID
    Task<AdminActionDto?> GetAdminActionByIdAsync(int actionId, CancellationToken ct = default);
    
    /// Gets admin actions with pagination
    Task<(List<AdminActionSummaryDto> Actions, int TotalCount)> GetPagedAdminActionsAsync(
        int pageNumber, 
        int pageSize, 
        AdminActionType? actionType = null, 
        CancellationToken ct = default);
    
    /// Gets admin actions by admin ID
    Task<List<AdminActionSummaryDto>> GetAdminActionsByAdminIdAsync(int adminId, AdminActionType? actionType = null, CancellationToken ct = default);
    
    /// Gets admin actions by target user ID
    Task<List<AdminActionSummaryDto>> GetAdminActionsByUserIdAsync(int userId, AdminActionType? actionType = null, CancellationToken ct = default);
    
    /// Gets recent admin actions
    Task<List<AdminActionSummaryDto>> GetRecentActionsAsync(int count = 50, CancellationToken ct = default);
}
