using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace StudentServicesWebApi.Application.DTOs.Payment;

/// <summary>
/// DTO for creating a new payment by a user
/// </summary>
public class CreatePaymentDto
{
    /// <summary>
    /// The amount requested by the user
    /// </summary>
    [Required(ErrorMessage = "Requested amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Requested amount must be greater than 0")]
    public decimal RequestedAmount { get; set; }

    /// <summary>
    /// Receipt photo/screenshot file uploaded by the user
    /// </summary>
    [Required(ErrorMessage = "Receipt photo is required")]
    public IFormFile Photo { get; set; } = null!;

    /// <summary>
    /// Optional description or note from the user
    /// </summary>
    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }
}