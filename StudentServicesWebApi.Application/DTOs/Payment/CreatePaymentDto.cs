using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace StudentServicesWebApi.Application.DTOs.Payment;
public class CreatePaymentDto
{
    [Required(ErrorMessage = "Requested amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Requested amount must be greater than 0")]
    public decimal RequestedAmount { get; set; }
    [Required(ErrorMessage = "Receipt photo is required")]
    public IFormFile Photo { get; set; } = null!;
    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }
}
