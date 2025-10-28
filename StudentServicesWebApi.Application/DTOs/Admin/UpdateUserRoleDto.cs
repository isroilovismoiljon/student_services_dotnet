using System.ComponentModel.DataAnnotations;
using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Application.DTOs.Admin;
public class UpdateUserRoleDto
{
    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }
    [Required(ErrorMessage = "New role is required")]
    public UserRole NewRole { get; set; }
    [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
    public string? Reason { get; set; }
}
