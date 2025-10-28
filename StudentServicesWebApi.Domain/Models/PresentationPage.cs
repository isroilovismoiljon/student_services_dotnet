namespace StudentServicesWebApi.Domain.Models;
public class PresentationPage : BaseEntity
{
    public int? PhotoId { get; set; }
    public PhotoSlide? Photo { get; set; }
    public int? BackgroundPhotoId { get; set; }
    public PhotoSlide? BackgroundPhoto { get; set; }
    public int PresentationIsroilovId { get; set; }
    public PresentationIsroilov PresentationIsroilov { get; set; } = new();
    public bool WithPhoto { get; set; } = false;
    public List<PresentationPost> PresentationPosts { get; set; } = new();
}
