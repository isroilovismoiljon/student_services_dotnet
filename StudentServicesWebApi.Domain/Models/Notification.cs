using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Domain.Models;

public class Notification : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public NotificationType Type { get; set; } = NotificationType.Info;
    public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public string? ActionUrl { get; set; }
    public string? IconUrl { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsGlobal { get; set; } = false;
    public string? Metadata { get; set; } // JSON string for additional data
}
