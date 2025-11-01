using StudentServicesWebApi.Application.DTOs.TextSlide;

namespace StudentServicesWebApi.Application.DTOs.Presentation;

public class CreatePresentationDto
{
    public CreateTextSlideDto Title { get; set; } = default!;
    public CreateTextSlideDto Author { get; set; } = default!;
    public bool WithPhoto { get; set; }
    public int PageCount { get; set; } = 10;
}
