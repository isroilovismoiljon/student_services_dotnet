using System.ComponentModel.DataAnnotations;
namespace StudentServicesWebApi.Application.DTOs.Admin;
public class ModifyBalanceDto
{
    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }
    [Required(ErrorMessage = "Amount is required")]
    [Range(1000, 500000, ErrorMessage = "Amount must be between 1000 and 500,000")]
    public decimal Amount { get; set; }
    [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
    public string? Reason { get; set; }
}
