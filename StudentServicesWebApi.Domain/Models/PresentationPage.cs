namespace StudentServicesWebApi.Domain.Models;

public class PresentationPage : BaseEntity
{
    // Foreign keys for photos
    public int? PhotoId { get; set; }
    public PhotoSlide? Photo { get; set; }
    
    public int? BackgroundPhotoId { get; set; }
    public PhotoSlide? BackgroundPhoto { get; set; }

    // Foreign key for presentation
    public int PresentationIsroilovId { get; set; }
    public PresentationIsroilov PresentationIsroilov { get; set; } = new();

    // Indicates if this page should have a photo
    public bool WithPhoto { get; set; } = false;

    // Postlar ro'yxati (3 tagacha)
    public List<PresentationPost> PresentationPosts { get; set; } = new();
}
