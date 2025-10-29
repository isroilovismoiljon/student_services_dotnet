using System.ComponentModel.DataAnnotations;
using StudentServicesWebApi.Application.DTOs.TextSlide;

namespace StudentServicesWebApi.Application.DTOs.PresentationPage;

public class CreatePresentationPagePostDto
{
    public CreateTextSlideDto? Title { get; set; }
    [Required]
    public CreateTextSlideDto Text { get; set; } = default!;
}

public class CreatePresentationPageDto
{
    public int? PhotoId { get; set; }
    public int? BackgroundPhotoId { get; set; }
    public List<CreatePresentationPagePostDto> PresentationPosts { get; set; } = default!;
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
    public bool WithPhoto { get; set; }
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
