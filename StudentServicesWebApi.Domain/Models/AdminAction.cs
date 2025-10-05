using System.ComponentModel.DataAnnotations;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Domain.Models;

/// <summary>
/// Represents an action performed by an admin (role changes, balance modifications, etc.)
/// </summary>
public class AdminAction : BaseEntity
{
    /// <summary>
    /// The admin who performed the action
    /// </summary>
    public int AdminId { get; set; }
    public User Admin { get; set; } = null!;
    
    /// <summary>
    /// The user who was affected by the action
    /// </summary>
    public int TargetUserId { get; set; }
    public User TargetUser { get; set; } = null!;
    
    /// <summary>
    /// Type of action performed
    /// </summary>
    public AdminActionType ActionType { get; set; }
    
    /// <summary>
    /// Description of the action
    /// </summary>
    [MaxLength(1000)]
    public required string Description { get; set; }
    
    /// <summary>
    /// Previous value (for role changes: previous role, for balance: previous balance)
    /// </summary>
    [MaxLength(100)]
    public string? PreviousValue { get; set; }
    
    /// <summary>
    /// New value (for role changes: new role, for balance: new balance)
    /// </summary>
    [MaxLength(100)]
    public string? NewValue { get; set; }
    
    /// <summary>
    /// Amount changed (for balance modifications)
    /// </summary>
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// Optional reason provided by the admin
    /// </summary>
    [MaxLength(500)]
    public string? Reason { get; set; }
    
    /// <summary>
    /// IP Address from which the action was performed
    /// </summary>
    [MaxLength(50)]
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// Whether telegram notification was sent
    /// </summary>
    public bool NotificationSent { get; set; } = false;
}