using System.ComponentModel.DataAnnotations;

namespace StudentServicesWebApi.Application.DTOs.PresentationPage;

public class CreatePresentationPageDto
{
    [Required]
    public int PresentationIsroilovId { get; set; }
    
    public int? PhotoId { get; set; }
    public int? BackgroundPhotoId { get; set; }
}

public class UpdatePresentationPageDto
{
    public int? PresentationIsroilovId { get; set; }
    public int? PhotoId { get; set; }
    public int? BackgroundPhotoId { get; set; }
}

public class PresentationPageDto
{
    public int Id { get; set; }
    public int PresentationIsroilovId { get; set; }
    public int? PhotoId { get; set; }
    public int? BackgroundPhotoId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<PresentationPostSummaryDto> Posts { get; set; } = new();
}

public class PresentationPageSummaryDto
{
    public int Id { get; set; }
    public int PresentationIsroilovId { get; set; }
    public int PostCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PresentationPostSummaryDto
{
    public int Id { get; set; }
    public int? TitleId { get; set; }
    public int TextId { get; set; }
    public DateTime CreatedAt { get; set; }
}