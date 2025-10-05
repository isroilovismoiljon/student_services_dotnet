using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StudentServicesWebApi.Application.DTOs.PhotoSlide;

/// <summary>
/// DTO for creating a new PhotoSlide with file upload
/// </summary>
public class CreatePhotoSlideDto
{
    /// <summary>
    /// Photo file to upload
    /// </summary>
    [Required]
    public IFormFile Photo { get; set; } = null!;

    /// <summary>
    /// Left position of the photo element
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Left position must be non-negative")]
    public double Left { get; set; } = 0;

    /// <summary>
    /// Top position of the photo element
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Top position must be non-negative")]
    public double Top { get; set; } = 0;

    /// <summary>
    /// Width of the photo element
    /// </summary>
    [Range(1, double.MaxValue, ErrorMessage = "Width must be positive")]
    public double Width { get; set; } = 100;

    /// <summary>
    /// Height of the photo element (optional, auto-calculated if not provided)
    /// </summary>
    [Range(1, double.MaxValue, ErrorMessage = "Height must be positive")]
    public double? Height { get; set; }
}

/// <summary>
/// DTO for updating an existing PhotoSlide
/// </summary>
public class UpdatePhotoSlideDto
{
    /// <summary>
    /// New photo file to replace existing one (optional)
    /// </summary>
    public IFormFile? Photo { get; set; }

    /// <summary>
    /// Left position of the photo element
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Left position must be non-negative")]
    public double? Left { get; set; }

    /// <summary>
    /// Top position of the photo element
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Top position must be non-negative")]
    public double? Top { get; set; }

    /// <summary>
    /// Width of the photo element
    /// </summary>
    [Range(1, double.MaxValue, ErrorMessage = "Width must be positive")]
    public double? Width { get; set; }

    /// <summary>
    /// Height of the photo element
    /// </summary>
    [Range(1, double.MaxValue, ErrorMessage = "Height must be positive")]
    public double? Height { get; set; }
}

/// <summary>
/// DTO for viewing PhotoSlide details
/// </summary>
public class PhotoSlideDto
{
    /// <summary>
    /// Unique identifier for the photo slide
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full URL to the photo
    /// </summary>
    public string PhotoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Original filename of the uploaded photo
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// MIME type of the photo
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Left position of the photo element
    /// </summary>
    public double Left { get; set; }

    /// <summary>
    /// Top position of the photo element
    /// </summary>
    public double Top { get; set; }

    /// <summary>
    /// Width of the photo element
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Height of the photo element
    /// </summary>
    public double? Height { get; set; }

    /// <summary>
    /// When the photo slide was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the photo slide was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Simplified DTO for listing photo slides
/// </summary>
public class PhotoSlideSummaryDto
{
    /// <summary>
    /// Unique identifier for the photo slide
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Thumbnail URL for the photo (smaller version for listings)
    /// </summary>
    public string ThumbnailUrl { get; set; } = string.Empty;

    /// <summary>
    /// Full photo URL
    /// </summary>
    public string PhotoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Original filename of the uploaded photo
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Human-readable file size (e.g., "1.2 MB")
    /// </summary>
    public string FileSizeFormatted { get; set; } = string.Empty;

    /// <summary>
    /// MIME type of the photo
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Position and size summary
    /// </summary>
    public string PositionSummary { get; set; } = string.Empty;

    /// <summary>
    /// When the photo slide was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the photo slide was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// DTO for bulk operations on photo slides
/// </summary>
public class BulkPhotoSlideOperationDto
{
    /// <summary>
    /// List of photo slide IDs to operate on
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "At least one photo slide ID is required")]
    public List<int> PhotoSlideIds { get; set; } = new();
}

/// <summary>
/// DTO for bulk creating photo slides with multiple files
/// </summary>
public class BulkCreatePhotoSlideDto
{
    /// <summary>
    /// List of photos to upload
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "At least one photo is required")]
    public List<IFormFile> Photos { get; set; } = new();

    /// <summary>
    /// Default left position for all photos (can be overridden per photo)
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Left position must be non-negative")]
    public double DefaultLeft { get; set; } = 0;

    /// <summary>
    /// Default top position for all photos (can be overridden per photo)
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Top position must be non-negative")]
    public double DefaultTop { get; set; } = 0;

    /// <summary>
    /// Default width for all photos (can be overridden per photo)
    /// </summary>
    [Range(1, double.MaxValue, ErrorMessage = "Width must be positive")]
    public double DefaultWidth { get; set; } = 100;

    /// <summary>
    /// Default height for all photos (optional, can be overridden per photo)
    /// </summary>
    [Range(1, double.MaxValue, ErrorMessage = "Height must be positive")]
    public double? DefaultHeight { get; set; }

    /// <summary>
    /// Spacing between photos when positioning them automatically
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Spacing must be non-negative")]
    public double Spacing { get; set; } = 10;

    /// <summary>
    /// Layout direction for automatic positioning (Horizontal or Vertical)
    /// </summary>
    public BulkLayoutDirection LayoutDirection { get; set; } = BulkLayoutDirection.Horizontal;
}

/// <summary>
/// Layout direction for bulk photo slide creation
/// </summary>
public enum BulkLayoutDirection
{
    /// <summary>
    /// Arrange photos horizontally (left to right)
    /// </summary>
    Horizontal,

    /// <summary>
    /// Arrange photos vertically (top to bottom)
    /// </summary>
    Vertical,

    /// <summary>
    /// Arrange photos in a grid pattern
    /// </summary>
    Grid
}

/// <summary>
/// DTO for photo slide upload result
/// </summary>
public class PhotoSlideUploadResultDto
{
    /// <summary>
    /// Whether the upload was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Created photo slide (if successful)
    /// </summary>
    public PhotoSlideDto? PhotoSlide { get; set; }

    /// <summary>
    /// Error message (if failed)
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Original filename that was attempted to upload
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;
}

/// <summary>
/// DTO for bulk photo slide upload results
/// </summary>
public class BulkPhotoSlideUploadResultDto
{
    /// <summary>
    /// Total number of photos attempted
    /// </summary>
    public int TotalAttempted { get; set; }

    /// <summary>
    /// Number of successful uploads
    /// </summary>
    public int SuccessfulUploads { get; set; }

    /// <summary>
    /// Number of failed uploads
    /// </summary>
    public int FailedUploads { get; set; }

    /// <summary>
    /// List of individual upload results
    /// </summary>
    public List<PhotoSlideUploadResultDto> Results { get; set; } = new();

    /// <summary>
    /// Overall success rate as a percentage
    /// </summary>
    public double SuccessRate => TotalAttempted > 0 ? (double)SuccessfulUploads / TotalAttempted * 100 : 0;
}