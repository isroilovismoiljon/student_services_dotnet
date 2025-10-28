using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace StudentServicesWebApi.Application.DTOs.Design;
public class DesignCreateWithPhotosDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = default!;
    [Required]
    [MinLength(4, ErrorMessage = "At least 4 photos are required")]
    public IFormFile[] Photos { get; set; } = default!;
}
