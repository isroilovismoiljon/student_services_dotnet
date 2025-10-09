using System.ComponentModel.DataAnnotations;

namespace StudentServicesWebApi.Application.DTOs.Design;

public class UpdateDesignDto
{
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string? Title { get; set; }
}
