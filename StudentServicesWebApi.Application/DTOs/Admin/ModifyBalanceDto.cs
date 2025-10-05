using System.ComponentModel.DataAnnotations;

namespace StudentServicesWebApi.Application.DTOs.Admin;

/// <summary>
/// DTO for adding or subtracting balance from user account
/// </summary>
public class ModifyBalanceDto
{
    /// <summary>
    /// ID of the user whose balance will be modified
    /// </summary>
    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }
    
    /// <summary>
    /// Amount to add or subtract (positive number)
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, 1000000, ErrorMessage = "Amount must be between 0.01 and 1,000,000")]
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Optional reason for the balance modification
    /// </summary>
    [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
    public string? Reason { get; set; }
}