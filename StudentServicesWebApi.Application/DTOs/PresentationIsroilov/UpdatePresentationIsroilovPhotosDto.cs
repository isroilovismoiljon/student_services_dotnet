using Microsoft.AspNetCore.Http;

namespace StudentServicesWebApi.Application.DTOs.PresentationIsroilov;

public class UpdatePresentationIsroilovPhotosDto
{
    public List<IFormFile> Photos { get; set; } = new();
}
