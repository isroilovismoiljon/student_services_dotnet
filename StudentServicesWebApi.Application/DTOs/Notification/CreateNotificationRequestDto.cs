using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Application.DTOs.Notification;
public class CreateNotificationRequestDto
{
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public NotificationType Type { get; set; } = NotificationType.Info;
    public int UserId { get; set; }
    public string? ActionUrl { get; set; }
    public string? IconUrl { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsGlobal { get; set; } = false;
    public string? Metadata { get; set; }
}
