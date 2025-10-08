using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Repositories;

public class AdminActionRepository : GenericRepository<AdminAction>, IAdminActionRepository
{
    public AdminActionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<AdminAction>> GetByAdminIdAsync(int adminId, AdminActionType? actionType = null, CancellationToken ct = default)
    {
        var query = _context.AdminActions
            .Include(a => a.Admin)
            .Include(a => a.TargetUser)
            .Where(a => a.AdminId == adminId);

        if (actionType.HasValue)
        {
            query = query.Where(a => a.ActionType == actionType.Value);
        }

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<AdminAction>> GetByTargetUserIdAsync(int targetUserId, AdminActionType? actionType = null, CancellationToken ct = default)
    {
        var query = _context.AdminActions
            .Include(a => a.Admin)
            .Include(a => a.TargetUser)
            .Where(a => a.TargetUserId == targetUserId);

        if (actionType.HasValue)
        {
            query = query.Where(a => a.ActionType == actionType.Value);
        }

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<AdminAction?> GetWithDetailsAsync(int actionId, CancellationToken ct = default)
    {
        return await _context.AdminActions
            .Include(a => a.Admin)
            .Include(a => a.TargetUser)
            .FirstOrDefaultAsync(a => a.Id == actionId, ct);
    }

    public async Task<List<AdminAction>> GetPagedAsync(int pageNumber, int pageSize, AdminActionType? actionType = null, CancellationToken ct = default)
    {
        var query = _context.AdminActions
            .Include(a => a.Admin)
            .Include(a => a.TargetUser)
            .AsQueryable();

        if (actionType.HasValue)
        {
            query = query.Where(a => a.ActionType == actionType.Value);
        }

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> GetCountAsync(AdminActionType? actionType = null, CancellationToken ct = default)
    {
        var query = _context.AdminActions.AsQueryable();

        if (actionType.HasValue)
        {
            query = query.Where(a => a.ActionType == actionType.Value);
        }

        return await query.CountAsync(ct);
    }

    public async Task<List<AdminAction>> GetRecentActionsAsync(int count = 50, CancellationToken ct = default)
    {
        return await _context.AdminActions
            .Include(a => a.Admin)
            .Include(a => a.TargetUser)
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .ToListAsync(ct);
    }
}