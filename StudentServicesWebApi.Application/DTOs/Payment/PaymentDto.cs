using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Application.DTOs.Payment;
public class PaymentDto
{
    public int Id { get; set; }
    public UserResponseDto Sender { get; set; } = new();
    public decimal RequestedAmount { get; set; }
    public decimal? ApprovedAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public string Photo { get; set; } = string.Empty;
    public string? Description { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? RejectReason { get; set; }
    public UserResponseDto? ProcessedByAdmin { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? AdminNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool AmountWasAdjusted { get; set; }
}
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