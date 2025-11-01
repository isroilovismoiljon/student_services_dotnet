using StudentServicesWebApi.Application.DTOs.Plan;
using StudentServicesWebApi.Application.DTOs.PresentationPage;
using StudentServicesWebApi.Application.DTOs.TextSlide;

namespace StudentServicesWebApi.Application.DTOs.PresentationIsroilov;

public class CreatePresentationIsroilovDto
{
    public CreateTextSlideDto Title { get; set; } = default!;
    public CreateTextSlideDto Author { get; set; } = default!;
    public CreatePlanDto Plan { get; set; } = default!;
    public int DesignId { get; set; }
    public bool WithPhoto { get; set; }
    public int PageCount { get; set; } = 10;
    public List<CreatePresentationPageDto> PresentationPages { get; set; } = new();
}
