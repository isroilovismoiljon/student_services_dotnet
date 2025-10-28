using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
    Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId);
    Task<IEnumerable<Notification>> GetGlobalNotificationsAsync();
    Task<bool> MarkAsReadAsync(int notificationId, int userId);
    Task<bool> MarkAllAsReadAsync(int userId);
    Task<int> GetUnreadCountAsync(int userId);
    Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(int userId, NotificationType type);
    Task<IEnumerable<Notification>> GetActiveNotificationsAsync(int userId);
}
