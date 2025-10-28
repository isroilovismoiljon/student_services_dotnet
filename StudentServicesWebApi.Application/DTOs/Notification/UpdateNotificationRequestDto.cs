using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Application.DTOs.Notification;
public class UpdateNotificationRequestDto
{
    public string? Title { get; set; }
    public string? Message { get; set; }
    public NotificationType? Type { get; set; }
    public NotificationStatus? Status { get; set; }
    public string? ActionUrl { get; set; }
    public string? IconUrl { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? Metadata { get; set; }
}
