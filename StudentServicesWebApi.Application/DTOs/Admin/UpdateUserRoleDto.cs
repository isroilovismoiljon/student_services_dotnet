using System.ComponentModel.DataAnnotations;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Application.DTOs.Admin;

/// <summary>
/// DTO for updating user role
/// </summary>
public class UpdateUserRoleDto
{
    /// <summary>
    /// ID of the user whose role will be updated
    /// </summary>
    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }
    
    /// <summary>
    /// New role for the user (User or Admin only)
    /// </summary>
    [Required(ErrorMessage = "New role is required")]
    public UserRole NewRole { get; set; }
    
    /// <summary>
    /// Optional reason for the role change
    /// </summary>
    [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
    public string? Reason { get; set; }
}