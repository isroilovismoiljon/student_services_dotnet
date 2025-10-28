using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace StudentServicesWebApi.Application.DTOs.PhotoSlide;
public class CreatePhotoSlideDto
{
    [Required]
    public IFormFile Photo { get; set; } = null!;
    [Range(0, double.MaxValue, ErrorMessage = "Left position must be non-negative")]
    public double Left { get; set; } = 0;
    [Range(0, double.MaxValue, ErrorMessage = "Top position must be non-negative")]
    public double Top { get; set; } = 0;
    [Range(1, double.MaxValue, ErrorMessage = "Width must be positive")]
    public double Width { get; set; } = 100;
    [Range(1, double.MaxValue, ErrorMessage = "Height must be positive")]
    public double? Height { get; set; }
}
public class UpdatePhotoSlideDto
{
    public IFormFile? Photo { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Left position must be non-negative")]
    public double? Left { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Top position must be non-negative")]
    public double? Top { get; set; }
    [Range(1, double.MaxValue, ErrorMessage = "Width must be positive")]
    public double? Width { get; set; }
    public double? Height { get; set; }
}
public class PhotoSlideDto
{
    public int Id { get; set; }
    public string PhotoPath { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FileSizeFormatted { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public double Left { get; set; }
    public double Top { get; set; }
    public double Width { get; set; }
    public double? Height { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
public class PhotoSlideSummaryDto
{
    public int Id { get; set; }
    public string PhotoPath { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FileSizeFormatted { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string PositionSummary { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
public class BulkPhotoSlideOperationDto
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one photo slide ID is required")]
    public List<int> PhotoSlideIds { get; set; } = new();
}
public class BulkCreatePhotoSlideDto
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one photo is required")]
    public List<IFormFile> Photos { get; set; } = new();
    [Range(0, double.MaxValue, ErrorMessage = "Left position must be non-negative")]
    public double DefaultLeft { get; set; } = 0;
    [Range(0, double.MaxValue, ErrorMessage = "Top position must be non-negative")]
    public double DefaultTop { get; set; } = 0;
    [Range(1, double.MaxValue, ErrorMessage = "Width must be positive")]
    public double DefaultWidth { get; set; } = 100;
    [Range(1, double.MaxValue, ErrorMessage = "Height must be positive")]
    public double? DefaultHeight { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Spacing must be non-negative")]
    public double Spacing { get; set; } = 10;
    public BulkLayoutDirection LayoutDirection { get; set; } = BulkLayoutDirection.Horizontal;
}
public enum BulkLayoutDirection
{
    Horizontal,
    Vertical,
    Grid
}
public class PhotoSlideUploadResultDto
{
    public bool Success { get; set; }
    public PhotoSlideDto? PhotoSlide { get; set; }
    public string? ErrorMessage { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
}
public class BulkPhotoSlideUploadResultDto
{
    public int TotalAttempted { get; set; }
    public int SuccessfulUploads { get; set; }
    public int FailedUploads { get; set; }
    public List<PhotoSlideUploadResultDto> Results { get; set; } = new();
    public double SuccessRate => TotalAttempted > 0 ? (double)SuccessfulUploads / TotalAttempted * 100 : 0;
}
