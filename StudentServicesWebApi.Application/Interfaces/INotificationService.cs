using StudentServicesWebApi.Application.DTOs.Notification;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface INotificationService
{
    Task<NotificationResponseDto> CreateNotificationAsync(CreateNotificationRequestDto request);
    Task<NotificationResponseDto?> GetNotificationByIdAsync(int id);
    Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync(int userId);
    Task<IEnumerable<NotificationResponseDto>> GetUnreadNotificationsByUserIdAsync(int userId);
    Task<IEnumerable<NotificationResponseDto>> GetGlobalNotificationsAsync();
    Task<NotificationResponseDto?> UpdateNotificationAsync(int id, UpdateNotificationRequestDto request);
    Task<bool> DeleteNotificationAsync(int id);
    Task<bool> MarkAsReadAsync(int notificationId, int userId);
    Task<bool> MarkAllAsReadAsync(int userId);
    Task<int> GetUnreadCountAsync(int userId);
    Task<IEnumerable<NotificationResponseDto>> GetNotificationsByTypeAsync(int userId, NotificationType type);
    Task<IEnumerable<NotificationResponseDto>> GetActiveNotificationsAsync(int userId);
    Task<bool> CreateBulkNotificationsAsync(List<CreateNotificationRequestDto> requests);
}
