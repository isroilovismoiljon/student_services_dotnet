namespace StudentServicesWebApi.Domain.Models;
public class PresentationPost : BaseEntity
{
    public int PresentationPageId { get; set; }
    public PresentationPage PresentationPage { get; set; } = new();
    public int? TitleId { get; set; }
    public TextSlide? Title { get; set; }
    public int TextId { get; set; }
    public TextSlide Text { get; set; } = new();
}
