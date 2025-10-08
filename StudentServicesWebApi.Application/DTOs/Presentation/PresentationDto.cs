using System.ComponentModel.DataAnnotations;

namespace StudentServicesWebApi.Application.DTOs.Presentation;

public class CreatePresentationDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Author { get; set; } = string.Empty;
    
    [Required]
    public int DesignId { get; set; }
    
    [Required]
    public int PlanId { get; set; }
    
    public bool IsActive { get; set; } = true;
    public string FilePath { get; set; } = string.Empty;
}

public class UpdatePresentationDto
{
    [StringLength(200)]
    public string? Title { get; set; }
    
    [StringLength(100)]
    public string? Author { get; set; }
    
    public int? DesignId { get; set; }
    public int? PlanId { get; set; }
    public bool? IsActive { get; set; }
    public string? FilePath { get; set; }
}

public class PresentationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public bool IsActive { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public int DesignId { get; set; }
    public int PlanId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<PresentationPageSummaryDto> Pages { get; set; } = new();
}

public class PresentationSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PresentationPageSummaryDto
{
    public int Id { get; set; }
    public int PostCount { get; set; }
    public DateTime CreatedAt { get; set; }
}