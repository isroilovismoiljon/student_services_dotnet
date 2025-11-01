using StudentServicesWebApi.Application.DTOs.TextSlide;

namespace StudentServicesWebApi.Application.DTOs.PresentationIsroilov;

public class PresentationIsroilovDto
{
    public int Id { get; set; }
    public TextSlideDto Title { get; set; } = default!;
    public TextSlideDto Author { get; set; } = default!;
    public bool WithPhoto { get; set; }
    public int PageCount { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
