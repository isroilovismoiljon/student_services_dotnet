using StudentServicesWebApi.Application.DTOs.Design;
using StudentServicesWebApi.Application.DTOs.Plan;
using StudentServicesWebApi.Application.DTOs.PresentationPage;
using StudentServicesWebApi.Application.DTOs.TextSlide;

namespace StudentServicesWebApi.Application.DTOs.PresentationIsroilov;

public class PresentationIsroilovDto
{
    public int Id { get; set; }
    public TextSlideDto Title { get; set; } = default!;
    public TextSlideDto Author { get; set; } = default!;
    public int PlanId { get; set; }
    public PlanDto Plan { get; set; } = default!;
    public int DesignId { get; set; }
    public DesignDto Design { get; set; } = default!;
    public bool WithPhoto { get; set; }
    public int PageCount { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<PresentationPageDto> PresentationPages { get; set; } = new();
}
