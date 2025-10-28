using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Application.DTOs.Admin;
public class AdminActionDto
{
    public int Id { get; set; }
    public UserResponseDto Admin { get; set; } = new();
    public UserResponseDto TargetUser { get; set; } = new();
    public AdminActionType ActionType { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? PreviousValue { get; set; }
    public string? NewValue { get; set; }
    public decimal? Amount { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool NotificationSent { get; set; }
}
public class AdminActionSummaryDto
{
    public int Id { get; set; }
    public string AdminName { get; set; } = string.Empty;
    public string AdminUsername { get; set; } = string.Empty;
    public string TargetUserFullName { get; set; } = string.Empty;
    public string TargetUserUsername { get; set; } = string.Empty;
    public AdminActionType ActionType { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool NotificationSent { get; set; }
}
