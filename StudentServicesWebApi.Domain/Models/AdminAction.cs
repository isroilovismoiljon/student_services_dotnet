using System.ComponentModel.DataAnnotations;
using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Domain.Models;
public class AdminAction : BaseEntity
{
    public int AdminId { get; set; }
    public User Admin { get; set; } = null!;
    public int TargetUserId { get; set; }
    public User TargetUser { get; set; } = null!;
    public AdminActionType ActionType { get; set; }
    [MaxLength(1000)]
    public required string Description { get; set; }
    [MaxLength(100)]
    public string? PreviousValue { get; set; }
    [MaxLength(100)]
    public string? NewValue { get; set; }
    public decimal? Amount { get; set; }
    [MaxLength(500)]
    public string? Reason { get; set; }
    [MaxLength(50)]
    public string? IpAddress { get; set; }
    public bool NotificationSent { get; set; } = false;
}
