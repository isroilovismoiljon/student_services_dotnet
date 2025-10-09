using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StudentServicesWebApi.Application.DTOs.PhotoSlide;

public class AddPhotoToDesignDto
{
    [Required(ErrorMessage = "Photo file is required")]
    public IFormFile Photo { get; set; } = default!;
}