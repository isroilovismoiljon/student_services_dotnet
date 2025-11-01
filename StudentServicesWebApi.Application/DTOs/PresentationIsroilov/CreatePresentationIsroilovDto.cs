using StudentServicesWebApi.Application.DTOs.TextSlide;

namespace StudentServicesWebApi.Application.DTOs.PresentationIsroilov;

public class CreatePresentationIsroilovDto
{
    public CreateTextSlideDto Title { get; set; } = default!;
    public CreateTextSlideDto Author { get; set; } = default!;
    public bool WithPhoto { get; set; }
    public int PageCount { get; set; } = 10;
}
