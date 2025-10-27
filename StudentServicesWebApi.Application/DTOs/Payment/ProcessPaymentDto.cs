using System.ComponentModel.DataAnnotations;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Application.DTOs.Payment;
public class ProcessPaymentDto
{
    [Required(ErrorMessage = "Payment status is required")]
    public PaymentStatus PaymentStatus { get; set; }
    public decimal? ApprovedAmount { get; set; }
    [MaxLength(1000, ErrorMessage = "Reject reason cannot exceed 1000 characters")]
    public string? RejectReason { get; set; }
    [MaxLength(1000, ErrorMessage = "Admin notes cannot exceed 1000 characters")]
    public string? AdminNotes { get; set; }
}