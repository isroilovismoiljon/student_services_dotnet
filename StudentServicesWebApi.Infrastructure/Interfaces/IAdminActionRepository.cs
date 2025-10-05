using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface IAdminActionRepository : IGenericRepository<AdminAction>
{
    /// <summary>
    /// Gets admin actions by admin ID with optional action type filter
    /// </summary>
    Task<List<AdminAction>> GetByAdminIdAsync(int adminId, AdminActionType? actionType = null, CancellationToken ct = default);
    
    /// <summary>
    /// Gets admin actions by target user ID with optional action type filter
    /// </summary>
    Task<List<AdminAction>> GetByTargetUserIdAsync(int targetUserId, AdminActionType? actionType = null, CancellationToken ct = default);
    
    /// <summary>
    /// Gets admin actions with full details (includes navigation properties)
    /// </summary>
    Task<AdminAction?> GetWithDetailsAsync(int actionId, CancellationToken ct = default);
    
    /// <summary>
    /// Gets admin actions with pagination
    /// </summary>
    Task<List<AdminAction>> GetPagedAsync(int pageNumber, int pageSize, AdminActionType? actionType = null, CancellationToken ct = default);
    
    /// <summary>
    /// Gets total count of admin actions with optional action type filter
    /// </summary>
    Task<int> GetCountAsync(AdminActionType? actionType = null, CancellationToken ct = default);
    
    /// <summary>
    /// Gets recent admin actions (last N actions)
    /// </summary>
    Task<List<AdminAction>> GetRecentActionsAsync(int count = 50, CancellationToken ct = default);
}