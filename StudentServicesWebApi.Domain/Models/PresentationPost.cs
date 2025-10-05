namespace StudentServicesWebApi.Domain.Models;

public class PresentationPost : BaseEntity
{
    // Foreign key
    public int PresentationPageId { get; set; }
    public PresentationPage PresentationPage { get; set; } = new();
    
    // Text slide foreign keys
    public int? TitleId { get; set; }
    public TextSlide? Title { get; set; }
    
    public int TextId { get; set; }
    public TextSlide Text { get; set; } = new();
}
