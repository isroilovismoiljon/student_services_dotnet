namespace StudentServicesWebApi.Application.DTOs.Design;

public class DesignSummaryDto
{
    public int Id { get; set; }
    
    public string Title { get; set; } = default!;
    public string CreatedByName { get; set; } = default!;
    public string CreatedByUsername { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public string? FirstPhotoUrl { get; set; }
    
    public int PhotoCount { get; set; }
}
