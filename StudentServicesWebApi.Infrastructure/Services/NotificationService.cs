using AutoMapper;
using StudentServicesWebApi.Application.DTOs.Notification;
using StudentServicesWebApi.Infrastructure.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Services;
public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;
    public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }
    public async Task<NotificationResponseDto> CreateNotificationAsync(CreateNotificationRequestDto request)
    {
        var notification = _mapper.Map<Notification>(request);
        notification.CreatedAt = DateTime.UtcNow;
        notification.UpdatedAt = DateTime.UtcNow;
        var createdNotification = await _notificationRepository.AddAsync(notification);
        return _mapper.Map<NotificationResponseDto>(createdNotification);
    }
    public async Task<NotificationResponseDto?> GetNotificationByIdAsync(int id)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        return notification != null ? _mapper.Map<NotificationResponseDto>(notification) : null;
    }
    public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync(int userId)
    {
        var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<NotificationResponseDto>>(notifications);
    }
    public async Task<IEnumerable<NotificationResponseDto>> GetUnreadNotificationsByUserIdAsync(int userId)
    {
        var notifications = await _notificationRepository.GetUnreadNotificationsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<NotificationResponseDto>>(notifications);
    }
    public async Task<IEnumerable<NotificationResponseDto>> GetGlobalNotificationsAsync()
    {
        var notifications = await _notificationRepository.GetGlobalNotificationsAsync();
        return _mapper.Map<IEnumerable<NotificationResponseDto>>(notifications);
    }
    public async Task<NotificationResponseDto?> UpdateNotificationAsync(int id, UpdateNotificationRequestDto request)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        if (notification == null) return null;
        if (!string.IsNullOrEmpty(request.Title))
            notification.Title = request.Title;
        if (!string.IsNullOrEmpty(request.Message))
            notification.Message = request.Message;
        if (request.Type.HasValue)
            notification.Type = request.Type.Value;
        if (request.Status.HasValue)
        {
            notification.Status = request.Status.Value;
            if (request.Status.Value == NotificationStatus.Read && !notification.ReadAt.HasValue)
                notification.ReadAt = DateTime.UtcNow;
        }
        if (request.ActionUrl != null)
            notification.ActionUrl = request.ActionUrl;
        if (request.IconUrl != null)
            notification.IconUrl = request.IconUrl;
        if (request.ExpiresAt.HasValue)
            notification.ExpiresAt = request.ExpiresAt.Value;
        if (request.Metadata != null)
            notification.Metadata = request.Metadata;
        notification.UpdatedAt = DateTime.UtcNow;
        var updatedNotification = await _notificationRepository.UpdateAsync(notification);
        return _mapper.Map<NotificationResponseDto>(updatedNotification);
    }
    public async Task<bool> DeleteNotificationAsync(int id)
    {
        return await _notificationRepository.DeleteAsync(id);
    }
    public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
    {
        return await _notificationRepository.MarkAsReadAsync(notificationId, userId);
    }
    public async Task<bool> MarkAllAsReadAsync(int userId)
    {
        return await _notificationRepository.MarkAllAsReadAsync(userId);
    }
    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _notificationRepository.GetUnreadCountAsync(userId);
    }
    public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByTypeAsync(int userId, NotificationType type)
    {
        var notifications = await _notificationRepository.GetNotificationsByTypeAsync(userId, type);
        return _mapper.Map<IEnumerable<NotificationResponseDto>>(notifications);
    }
    public async Task<IEnumerable<NotificationResponseDto>> GetActiveNotificationsAsync(int userId)
    {
        var notifications = await _notificationRepository.GetActiveNotificationsAsync(userId);
        return _mapper.Map<IEnumerable<NotificationResponseDto>>(notifications);
    }
    public async Task<bool> CreateBulkNotificationsAsync(List<CreateNotificationRequestDto> requests)
    {
        try
        {
            var notifications = _mapper.Map<List<Notification>>(requests);
            foreach (var notification in notifications)
            {
                notification.CreatedAt = DateTime.UtcNow;
                notification.UpdatedAt = DateTime.UtcNow;
            }
            await _notificationRepository.AddRangeAsync(notifications);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
