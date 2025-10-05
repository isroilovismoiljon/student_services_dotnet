namespace StudentServicesWebApi.Domain.Models;

public class PresentationIsroilov : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public bool IsActive { get; set; }
    public string FilePath { get; set; } = string.Empty;

    // Foreign keys and navigation properties
    public int DesignId { get; set; }
    public Design Design { get; set; } = new();
    
    public int PlanId { get; set; }
    public Plan Plan { get; set; } = new();
    
    public List<PresentationPage> PresentationPages { get; set; } = new();
}
