using StudentServicesWebApi.Application.DTOs.Admin;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface IAdminActionService
{
    /// <summary>
    /// Updates user role and logs the action
    /// </summary>
    Task<AdminActionDto> UpdateUserRoleAsync(UpdateUserRoleDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default);
    
    /// <summary>
    /// Adds balance to user account and logs the action
    /// </summary>
    Task<AdminActionDto> AddBalanceAsync(ModifyBalanceDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default);
    
    /// <summary>
    /// Subtracts balance from user account and logs the action
    /// </summary>
    Task<AdminActionDto> SubtractBalanceAsync(ModifyBalanceDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default);
    
    /// <summary>
    /// Gets admin action by ID
    /// </summary>
    Task<AdminActionDto?> GetAdminActionByIdAsync(int actionId, CancellationToken ct = default);
    
    /// <summary>
    /// Gets admin actions with pagination
    /// </summary>
    Task<(List<AdminActionSummaryDto> Actions, int TotalCount)> GetPagedAdminActionsAsync(
        int pageNumber, 
        int pageSize, 
        AdminActionType? actionType = null, 
        CancellationToken ct = default);
    
    /// <summary>
    /// Gets admin actions by admin ID
    /// </summary>
    Task<List<AdminActionSummaryDto>> GetAdminActionsByAdminIdAsync(int adminId, AdminActionType? actionType = null, CancellationToken ct = default);
    
    /// <summary>
    /// Gets admin actions by target user ID
    /// </summary>
    Task<List<AdminActionSummaryDto>> GetAdminActionsByUserIdAsync(int userId, AdminActionType? actionType = null, CancellationToken ct = default);
    
    /// <summary>
    /// Gets recent admin actions
    /// </summary>
    Task<List<AdminActionSummaryDto>> GetRecentActionsAsync(int count = 50, CancellationToken ct = default);
}