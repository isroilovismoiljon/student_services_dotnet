using System.ComponentModel.DataAnnotations;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Application.DTOs.Payment;

/// <summary>
/// DTO for admin to process (approve/reject) a payment
/// </summary>
public class ProcessPaymentDto
{
    /// <summary>
    /// The new status for the payment (Success or Rejected)
    /// </summary>
    [Required(ErrorMessage = "Payment status is required")]
    public PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// The approved amount (can be different from requested amount)
    /// Optional - if null when status is Success, defaults to RequestedAmount
    /// </summary>
    public decimal? ApprovedAmount { get; set; }

    /// <summary>
    /// Reason for rejection (required when status is Rejected)
    /// </summary>
    [MaxLength(1000, ErrorMessage = "Reject reason cannot exceed 1000 characters")]
    public string? RejectReason { get; set; }

    /// <summary>
    /// Additional notes from the admin
    /// </summary>
    [MaxLength(1000, ErrorMessage = "Admin notes cannot exceed 1000 characters")]
    public string? AdminNotes { get; set; }
}