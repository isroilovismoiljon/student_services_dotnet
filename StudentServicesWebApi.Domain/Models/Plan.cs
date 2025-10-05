namespace StudentServicesWebApi.Domain.Models;

public class Plan : BaseEntity
{
    public TextSlide Title { get; set; } = new TextSlide();
    public TextSlide Plan_1 { get; set; } = new();
    public TextSlide Plan_2 { get; set; } = new();
    public TextSlide Plan_3 { get; set; } = new();
    public TextSlide? Plan_4 { get; set; }
    public TextSlide? Plan_5 { get; set; }
}
