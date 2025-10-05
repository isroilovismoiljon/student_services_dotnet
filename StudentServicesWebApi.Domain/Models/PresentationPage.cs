namespace StudentServicesWebApi.Domain.Models;

public class PresentationPage : BaseEntity
{
    // Asosiy rasm (URL ko'rinishida saqlash mumkin)
    public PhotoSlide? Photo { get; set; }
    public PhotoSlide BackgroundPhoto { get; set; } = new();

    // Foreign key
    public int PresentationIsroilovId { get; set; }
    public PresentationIsroilov PresentationIsroilov { get; set; } = new();

    // Postlar ro'yxati (3 tagacha)
    public List<PresentationPost> PresentationPosts { get; set; } = new();
}
