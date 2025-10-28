using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface IAdminActionRepository : IGenericRepository<AdminAction>
{
    Task<List<AdminAction>> GetByAdminIdAsync(int adminId, AdminActionType? actionType = null, CancellationToken ct = default);
    Task<List<AdminAction>> GetByTargetUserIdAsync(int targetUserId, AdminActionType? actionType = null, CancellationToken ct = default);
    Task<AdminAction?> GetWithDetailsAsync(int actionId, CancellationToken ct = default);
    Task<List<AdminAction>> GetPagedAsync(int pageNumber, int pageSize, AdminActionType? actionType = null, CancellationToken ct = default);
    Task<int> GetCountAsync(AdminActionType? actionType = null, CancellationToken ct = default);
    Task<List<AdminAction>> GetRecentActionsAsync(int count = 50, CancellationToken ct = default);
}
