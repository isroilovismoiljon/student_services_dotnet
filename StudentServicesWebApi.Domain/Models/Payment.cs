using System.ComponentModel.DataAnnotations;
using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Domain.Models;
public class Payment : BaseEntity
{
    #region User-Submitted Information
    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;
    [Range(0.01, double.MaxValue, ErrorMessage = "Requested amount must be greater than 0")]
    public decimal RequestedAmount { get; set; }
    [Required(ErrorMessage = "Receipt photo is required")]
    public required string Photo { get; set; }
    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }
    #endregion
    #region Admin-Processed Information
    [Range(0.01, double.MaxValue, ErrorMessage = "Approved amount must be greater than 0")]
    public decimal? ApprovedAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Waiting;
    [MaxLength(1000, ErrorMessage = "Reject reason cannot exceed 1000 characters")]
    public string? RejectReason { get; set; }
    public int? ProcessedByAdminId { get; set; }
    public User? ProcessedByAdmin { get; set; }
    public DateTime? ProcessedAt { get; set; }
    [MaxLength(1000, ErrorMessage = "Admin notes cannot exceed 1000 characters")]
    public string? AdminNotes { get; set; }
    #endregion
    #region Business Logic Methods
    public bool CanBeProcessed => PaymentStatus == PaymentStatus.Waiting;
    public bool IsProcessed => PaymentStatus != PaymentStatus.Waiting;
    public decimal FinalAmount => ApprovedAmount ?? RequestedAmount;
    public bool AmountWasAdjusted => ApprovedAmount.HasValue && ApprovedAmount != RequestedAmount;
    #endregion
}
