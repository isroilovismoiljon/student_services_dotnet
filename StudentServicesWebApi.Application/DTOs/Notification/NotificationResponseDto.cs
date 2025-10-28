using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Application.DTOs.Notification;
public class NotificationResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }
    public int UserId { get; set; }
    public string? ActionUrl { get; set; }
    public string? IconUrl { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsGlobal { get; set; }
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
