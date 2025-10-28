using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Infrastructure.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Repositories;
public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
    {
        return await _context.Set<Notification>()
            .Where(n => !n.IsDeleted && (n.UserId == userId || n.IsGlobal))
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
    public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId)
    {
        return await _context.Set<Notification>()
            .Where(n => !n.IsDeleted && (n.UserId == userId || n.IsGlobal) && n.Status == NotificationStatus.Unread)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
    public async Task<IEnumerable<Notification>> GetGlobalNotificationsAsync()
    {
        return await _context.Set<Notification>()
            .Where(n => !n.IsDeleted && n.IsGlobal)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
    public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
    {
        var notification = await _context.Set<Notification>()
            .Where(n => !n.IsDeleted)
            .FirstOrDefaultAsync(n => n.Id == notificationId && (n.UserId == userId || n.IsGlobal));
        if (notification == null) return false;
        notification.Status = NotificationStatus.Read;
        notification.ReadAt = DateTime.UtcNow;
        notification.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> MarkAllAsReadAsync(int userId)
    {
        var notifications = await _context.Set<Notification>()
            .Where(n => !n.IsDeleted && (n.UserId == userId || n.IsGlobal) && n.Status == NotificationStatus.Unread)
            .ToListAsync();
        foreach (var notification in notifications)
        {
            notification.Status = NotificationStatus.Read;
            notification.ReadAt = DateTime.UtcNow;
            notification.UpdatedAt = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _context.Set<Notification>()
            .Where(n => !n.IsDeleted)
            .CountAsync(n => (n.UserId == userId || n.IsGlobal) && n.Status == NotificationStatus.Unread);
    }
    public async Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(int userId, NotificationType type)
    {
        return await _context.Set<Notification>()
            .Where(n => !n.IsDeleted && (n.UserId == userId || n.IsGlobal) && n.Type == type)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
    public async Task<IEnumerable<Notification>> GetActiveNotificationsAsync(int userId)
    {
        var now = DateTime.UtcNow;
        return await _context.Set<Notification>()
            .Where(n => !n.IsDeleted && (n.UserId == userId || n.IsGlobal) && 
                       (n.ExpiresAt == null || n.ExpiresAt > now))
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
}
