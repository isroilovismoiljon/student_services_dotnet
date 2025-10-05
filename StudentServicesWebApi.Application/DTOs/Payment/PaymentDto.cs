using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Application.DTOs.Payment;

/// <summary>
/// DTO for viewing payment details
/// </summary>
public class PaymentDto
{
    public int Id { get; set; }
    
    /// <summary>
    /// User who created the payment
    /// </summary>
    public UserResponseDto Sender { get; set; } = new();
    
    
    /// <summary>
    /// Amount originally requested by the user
    /// </summary>
    public decimal RequestedAmount { get; set; }
    
    /// <summary>
    /// Amount approved by admin (if approved)
    /// </summary>
    public decimal? ApprovedAmount { get; set; }
    
    /// <summary>
    /// Final amount (approved amount if available, otherwise requested amount)
    /// </summary>
    public decimal FinalAmount { get; set; }
    
    /// <summary>
    /// Receipt photo URL/path
    /// </summary>
    public string Photo { get; set; } = string.Empty;
    
    /// <summary>
    /// Description provided by user
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Current payment status
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; }
    
    /// <summary>
    /// Rejection reason (if rejected)
    /// </summary>
    public string? RejectReason { get; set; }
    
    /// <summary>
    /// Admin who processed the payment
    /// </summary>
    public UserResponseDto? ProcessedByAdmin { get; set; }
    
    /// <summary>
    /// When the payment was processed
    /// </summary>
    public DateTime? ProcessedAt { get; set; }
    
    /// <summary>
    /// Additional notes from admin
    /// </summary>
    public string? AdminNotes { get; set; }
    
    /// <summary>
    /// When the payment was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// When the payment was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Whether the amount was adjusted by admin
    /// </summary>
    public bool AmountWasAdjusted { get; set; }
}

/// <summary>
/// Simplified DTO for listing payments
/// </summary>
public class PaymentSummaryDto
{
    public int Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderUsername { get; set; } = string.Empty;
    public string ProcessedByAdminName { get; set; } = string.Empty;
    public decimal RequestedAmount { get; set; }
    public string Photo { get; set; } = string.Empty;
    public decimal? ApprovedAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public bool AmountWasAdjusted { get; set; }
}