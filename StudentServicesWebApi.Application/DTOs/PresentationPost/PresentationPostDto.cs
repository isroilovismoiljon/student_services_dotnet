using System.ComponentModel.DataAnnotations;
namespace StudentServicesWebApi.Application.DTOs.PresentationPost;
public class CreatePresentationPostDto
{
    [Required]
    public int PresentationPageId { get; set; }
    public int? TitleId { get; set; }
    [Required]
    public int TextId { get; set; }
}
public class UpdatePresentationPostDto
{
    public int? PresentationPageId { get; set; }
    public int? TitleId { get; set; }
    public int? TextId { get; set; }
}
public class PresentationPostDto
{
    public int Id { get; set; }
    public int PresentationPageId { get; set; }
    public int? TitleId { get; set; }
    public int TextId { get; set; }
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
