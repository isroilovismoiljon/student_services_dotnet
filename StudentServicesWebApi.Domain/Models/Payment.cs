using System.ComponentModel.DataAnnotations;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Domain.Models;

/// <summary>
/// Represents a payment transaction initiated by a User and processed by an Admin/SuperAdmin.
/// Business Rules:
/// - Sender must always be a User (UserRole.User)
/// - ProcessedByAdmin processes the payment when approving/rejecting
/// - User provides RequestedAmount and Photo (receipt)
/// - Admin can edit amount and approve/reject
/// - Rejection requires RejectReason
/// </summary>
public class Payment : BaseEntity
{
    #region User-Submitted Information
    
    /// <summary>
    /// The user who initiated the payment (must have UserRole.User)
    /// </summary>
    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;
    
    
    /// <summary>
    /// The amount originally requested by the user
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Requested amount must be greater than 0")]
    public decimal RequestedAmount { get; set; }
    
    /// <summary>
    /// Receipt photo/screenshot uploaded by the user (required)
    /// Can be a file path, URL, or base64 encoded image
    /// </summary>
    [Required(ErrorMessage = "Receipt photo is required")]
    public required string Photo { get; set; }
    
    /// <summary>
    /// Optional description or note provided by the user
    /// </summary>
    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }
    
    #endregion
    
    #region Admin-Processed Information
    
    /// <summary>
    /// The amount approved by the admin (can be different from RequestedAmount)
    /// Only set when payment is approved
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Approved amount must be greater than 0")]
    public decimal? ApprovedAmount { get; set; }
    
    /// <summary>
    /// Current status of the payment
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Waiting;
    
    /// <summary>
    /// Reason provided by admin if payment is rejected (required when status is Rejected)
    /// </summary>
    [MaxLength(1000, ErrorMessage = "Reject reason cannot exceed 1000 characters")]
    public string? RejectReason { get; set; }
    
    /// <summary>
    /// The admin who processed this payment (approved or rejected)
    /// </summary>
    public int? ProcessedByAdminId { get; set; }
    public User? ProcessedByAdmin { get; set; }
    
    /// <summary>
    /// When the payment was processed by admin (approved or rejected)
    /// </summary>
    public DateTime? ProcessedAt { get; set; }
    
    /// <summary>
    /// Additional notes from the admin during processing
    /// </summary>
    [MaxLength(1000, ErrorMessage = "Admin notes cannot exceed 1000 characters")]
    public string? AdminNotes { get; set; }
    
    #endregion
    
    #region Business Logic Methods
    
    /// <summary>
    /// Checks if the payment can be processed (is in Waiting status)
    /// </summary>
    public bool CanBeProcessed => PaymentStatus == PaymentStatus.Waiting;
    
    /// <summary>
    /// Checks if the payment has been processed (approved or rejected)
    /// </summary>
    public bool IsProcessed => PaymentStatus != PaymentStatus.Waiting;
    
    /// <summary>
    /// Gets the final amount (ApprovedAmount if approved, otherwise RequestedAmount)
    /// </summary>
    public decimal FinalAmount => ApprovedAmount ?? RequestedAmount;
    
    /// <summary>
    /// Checks if the approved amount differs from the requested amount
    /// </summary>
    public bool AmountWasAdjusted => ApprovedAmount.HasValue && ApprovedAmount != RequestedAmount;
    
    #endregion
}
