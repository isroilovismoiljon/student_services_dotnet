namespace StudentServicesWebApi.Domain.Models;

public class PresentationIsroilov : BaseEntity
{
    public int TitleId { get; set; }
    public TextSlide Title { get; set; } = new();

    public int AuthorId { get; set; }
    public TextSlide Author { get; set; } = new();
    
    public bool WithPhoto { get; set; } = false;
    public int PageCount { get; set; } = 10;
    public bool IsActive { get; set; }
    public string FilePath { get; set; } = string.Empty;

    public int DesignId { get; set; }
    public Design Design { get; set; } = new();
    
    public int PlanId { get; set; }
    public Plan Plan { get; set; } = new();
    
    public List<PresentationPage> PresentationPages { get; set; } = new();
}
