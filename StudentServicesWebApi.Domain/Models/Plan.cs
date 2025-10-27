namespace StudentServicesWebApi.Domain.Models;

public class Plan : BaseEntity
{
    public TextSlide PlanText { get; set; } = new TextSlide();
    public TextSlide Plans { get; set; } = new();
}
