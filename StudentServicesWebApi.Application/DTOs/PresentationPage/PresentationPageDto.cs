using System.ComponentModel.DataAnnotations;
using StudentServicesWebApi.Application.DTOs.TextSlide;

namespace StudentServicesWebApi.Application.DTOs.PresentationPage;

public class CreatePresentationPagePostDto
{
    [Required]
    public CreateTextSlideDto Title { get; set; } = default!;
    [Required]
    public CreateTextSlideDto Text { get; set; } = default!;
}

public class CreatePresentationPageDto
{
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
    public List<PresentationPostDetailsDto> PresentationPosts { get; set; } = new();
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
public class PresentationPostDetailsDto
{
    public int Id { get; set; }
    public TextSlideDto? Title { get; set; }
    public TextSlideDto Text { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
