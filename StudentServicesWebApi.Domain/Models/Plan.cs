namespace StudentServicesWebApi.Domain.Models;
public class Plan : BaseEntity
{
    public TextSlide PlanText { get; set; } = new();
    public TextSlide Plans { get; set; } = new();
}
