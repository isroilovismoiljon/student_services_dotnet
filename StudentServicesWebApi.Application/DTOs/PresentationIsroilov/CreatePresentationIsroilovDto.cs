using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.DTOs.Plan;
using StudentServicesWebApi.Application.DTOs.PresentationPage;

namespace StudentServicesWebApi.Application.DTOs.PresentationIsroilov;

public class CreatePresentationIsroilovDto
{
    public CreateTextSlideDto Title { get; set; } = default!;
    public CreateTextSlideDto Author { get; set; } = default!;
    public bool WithPhoto { get; set; }
    public int PageCount { get; set; }
    public int DesignId { get; set; }
    public CreatePlanDto Plan { get; set; } = default!;
    public List<CreatePresentationPageDto> PresentationPages { get; set; } = default!;
}
