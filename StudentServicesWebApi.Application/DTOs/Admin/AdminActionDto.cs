using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Application.DTOs.Admin;

/// <summary>
/// DTO for admin action response
/// </summary>
public class AdminActionDto
{
    public int Id { get; set; }
    
    /// <summary>
    /// The admin who performed the action
    /// </summary>
    public UserResponseDto Admin { get; set; } = new();
    
    /// <summary>
    /// The user who was affected by the action
    /// </summary>
    public UserResponseDto TargetUser { get; set; } = new();
    
    /// <summary>
    /// Type of action performed
    /// </summary>
    public AdminActionType ActionType { get; set; }
    
    /// <summary>
    /// Description of the action
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Previous value
    /// </summary>
    public string? PreviousValue { get; set; }
    
    /// <summary>
    /// New value
    /// </summary>
    public string? NewValue { get; set; }
    
    /// <summary>
    /// Amount changed (for balance modifications)
    /// </summary>
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// Reason provided by admin
    /// </summary>
    public string? Reason { get; set; }
    
    /// <summary>
    /// When the action was performed
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Whether telegram notification was sent
    /// </summary>
    public bool NotificationSent { get; set; }
}

/// <summary>
/// DTO for admin action summary in lists
/// </summary>
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