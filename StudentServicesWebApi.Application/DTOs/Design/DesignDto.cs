using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Application.DTOs.PhotoSlide;

namespace StudentServicesWebApi.Application.DTOs.Design;

public class DesignDto
{
    public int Id { get; set; }
    
    public string Title { get; set; } = default!;
    
    public UserResponseDto CreatedBy { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public List<PhotoSlideDto> Photos { get; set; } = new();
    
    public int PhotoCount => Photos.Count;
    
    public string? FirstPhotoUrl { get; set; }
}
