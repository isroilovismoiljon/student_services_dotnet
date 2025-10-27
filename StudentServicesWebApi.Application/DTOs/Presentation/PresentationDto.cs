using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.DTOs.Plan;
using System.Text.Json.Serialization;

namespace StudentServicesWebApi.Application.DTOs.Presentation;

// For JSON data only (text-based fields)
public class CreatePresentationDataDto
{
    [Required]
    public CreateTextSlideDto Title { get; set; } = new();
    
    [Required]
    public CreateTextSlideDto Author { get; set; } = new();
    
    [Required]
    public CreatePlanDto PlanData { get; set; } = new();
    
    [Required]
    public int DesignId { get; set; }
    
    [Required]
    [Range(1, 100, ErrorMessage = "Page count must be between 1 and 100")]
    public int PageCount { get; set; } = 10;
    
    public bool WithPhoto { get; set; } = false;
    
    [Required]
    [MinLength(1, ErrorMessage = "At least one post text is required")]
    public List<CreateTextSlideDto> PostTexts { get; set; } = new();
    
    public bool IsActive { get; set; } = true;
    public string FilePath { get; set; } = string.Empty;
    
    // Photo position data (without actual files)
    public List<PhotoPositionDto> PhotoPositions { get; set; } = new();
}

// For photo position data without files
public class PhotoPositionDto
{
    [Range(0, double.MaxValue, ErrorMessage = "Left position must be non-negative")]
    public double Left { get; set; } = 0;
    
    [Range(0, double.MaxValue, ErrorMessage = "Top position must be non-negative")]
    public double Top { get; set; } = 0;
    
    [Range(1, double.MaxValue, ErrorMessage = "Width must be positive")]
    public double Width { get; set; } = 100;
    
    [Range(1, double.MaxValue, ErrorMessage = "Height must be positive")]
    public double? Height { get; set; }
}

// For JSON-only data (no photos) - used in CreatePresentation endpoint
public class CreatePresentationJsonDto
{
    // Title properties
    [Required]
    public string TitleText { get; set; } = string.Empty;
    
    [Required]
    public string TitleFont { get; set; } = "Arial";
    
    [Required]
    [Range(1, 100, ErrorMessage = "Title font size must be between 1 and 100")]
    public int TitleSize { get; set; } = 24;
    
    [Range(0, 33.867, ErrorMessage = "Title Left position must be between 0 and 33.867 cm")]
    public double TitleLeft { get; set; } = 0;
    
    [Range(0, 19.05, ErrorMessage = "Title Top position must be between 0 and 19.05 cm")]
    public double TitleTop { get; set; } = 0;
    
    [Range(1, 33.867, ErrorMessage = "Title Width must be between 1 and 33.867 cm")]
    public double TitleWidth { get; set; } = 10;
    
    [Range(1, 19.05, ErrorMessage = "Title Height must be between 1 and 19.05 cm")]
    public double TitleHeight { get; set; } = 2;
    
    // Author properties
    [Required]
    public string AuthorText { get; set; } = string.Empty;
    
    [Required]
    public string AuthorFont { get; set; } = "Arial";
    
    [Required]
    [Range(1, 100, ErrorMessage = "Author font size must be between 1 and 100")]
    public int AuthorSize { get; set; } = 18;
    
    [Range(0, 33.867, ErrorMessage = "Author Left position must be between 0 and 33.867 cm")]
    public double AuthorLeft { get; set; } = 0;
    
    [Range(0, 19.05, ErrorMessage = "Author Top position must be between 0 and 19.05 cm")]
    public double AuthorTop { get; set; } = 0;
    
    [Range(1, 33.867, ErrorMessage = "Author Width must be between 1 and 33.867 cm")]
    public double AuthorWidth { get; set; } = 8;
    
    [Range(1, 19.05, ErrorMessage = "Author Height must be between 1 and 19.05 cm")]
    public double AuthorHeight { get; set; } = 1.5;
    
    // Plan properties
    [Required]
    public string PlanText { get; set; } = string.Empty;
    
    [Required]
    public string PlanFont { get; set; } = "Arial";
    
    [Required]
    [Range(1, 100, ErrorMessage = "Plan font size must be between 1 and 100")]
    public int PlanSize { get; set; } = 14;
    
    [Range(0, 33.867, ErrorMessage = "Plan Left position must be between 0 and 33.867 cm")]
    public double PlanLeft { get; set; } = 0;
    
    [Range(0, 19.05, ErrorMessage = "Plan Top position must be between 0 and 19.05 cm")]
    public double PlanTop { get; set; } = 0;
    
    [Range(1, 33.867, ErrorMessage = "Plan Width must be between 1 and 33.867 cm")]
    public double PlanWidth { get; set; } = 15;
    
    [Range(1, 19.05, ErrorMessage = "Plan Height must be between 1 and 19.05 cm")]
    public double PlanHeight { get; set; } = 8;
    
    // General properties
    [Required]
    public int DesignId { get; set; }
    
    [Required]
    [Range(1, 100, ErrorMessage = "Page count must be between 1 and 100")]
    public int PageCount { get; set; } = 10;
    
    public bool WithPhoto { get; set; } = false;
    
    public bool IsActive { get; set; } = true;
    
    // FilePath is optional
    public string FilePath { get; set; } = string.Empty;
    
    // Photo positions as object array (not files)
    public List<PhotoPositionDto> PhotoPositions { get; set; } = new();
    
    // Post texts as object array (not JSON string)
    [Required]
    public List<CreateTextSlideDto> PostTexts { get; set; } = new();
}

// For mixed model binding with separate Swagger fields (kept for UpdatePhotos endpoint)
public class CreatePresentationMixedDto
{
    // Title properties - separate input fields in Swagger
    [Required]
    public string TitleText { get; set; } = string.Empty;
    
    [Required]
    public string TitleFont { get; set; } = "Arial";
    
    [Required]
    [Range(1, 100, ErrorMessage = "Title font size must be between 1 and 100")]
    public int TitleSize { get; set; } = 24;
    
    [Range(0, 33.867, ErrorMessage = "Title Left position must be between 0 and 33.867 cm")]
    public double TitleLeft { get; set; } = 0;
    
    [Range(0, 19.05, ErrorMessage = "Title Top position must be between 0 and 19.05 cm")]
    public double TitleTop { get; set; } = 0;
    
    [Range(1, 33.867, ErrorMessage = "Title Width must be between 1 and 33.867 cm")]
    public double TitleWidth { get; set; } = 10;
    
    [Range(1, 19.05, ErrorMessage = "Title Height must be between 1 and 19.05 cm")]
    public double TitleHeight { get; set; } = 2;
    
    // Author properties - separate input fields in Swagger
    [Required]
    public string AuthorText { get; set; } = string.Empty;
    
    [Required]
    public string AuthorFont { get; set; } = "Arial";
    
    [Required]
    [Range(1, 100, ErrorMessage = "Author font size must be between 1 and 100")]
    public int AuthorSize { get; set; } = 18;
    
    [Range(0, 33.867, ErrorMessage = "Author Left position must be between 0 and 33.867 cm")]
    public double AuthorLeft { get; set; } = 0;
    
    [Range(0, 19.05, ErrorMessage = "Author Top position must be between 0 and 19.05 cm")]
    public double AuthorTop { get; set; } = 0;
    
    [Range(1, 33.867, ErrorMessage = "Author Width must be between 1 and 33.867 cm")]
    public double AuthorWidth { get; set; } = 8;
    
    [Range(1, 19.05, ErrorMessage = "Author Height must be between 1 and 19.05 cm")]
    public double AuthorHeight { get; set; } = 1.5;
    
    // Plan properties - separate input fields in Swagger
    [Required]
    public string PlanText { get; set; } = string.Empty;
    
    [Required]
    public string PlanFont { get; set; } = "Arial";
    
    [Required]
    [Range(1, 100, ErrorMessage = "Plan font size must be between 1 and 100")]
    public int PlanSize { get; set; } = 14;
    
    [Range(0, 33.867, ErrorMessage = "Plan Left position must be between 0 and 33.867 cm")]
    public double PlanLeft { get; set; } = 0;
    
    [Range(0, 19.05, ErrorMessage = "Plan Top position must be between 0 and 19.05 cm")]
    public double PlanTop { get; set; } = 0;
    
    [Range(1, 33.867, ErrorMessage = "Plan Width must be between 1 and 33.867 cm")]
    public double PlanWidth { get; set; } = 15;
    
    [Range(1, 19.05, ErrorMessage = "Plan Height must be between 1 and 19.05 cm")]
    public double PlanHeight { get; set; } = 8;
    
    // General properties
    [Required]
    public int DesignId { get; set; }
    
    [Required]
    [Range(1, 100, ErrorMessage = "Page count must be between 1 and 100")]
    public int PageCount { get; set; } = 10;
    
    public bool WithPhoto { get; set; } = false;
    
    public bool IsActive { get; set; } = true;
    
    // FilePath is optional (not required)
    public string FilePath { get; set; } = string.Empty;
    
    // Photo upload fields - appear as file upload inputs in Swagger
    public List<IFormFile> Photos { get; set; } = new();
    
    // Photo positions as JSON string to support unlimited photos
    public string PhotoPositions { get; set; } = string.Empty; // JSON array of PhotoPositionDto objects
    
    // Post text fields - JSON string of CreateTextSlideDto array
    [Required]
    public string PostTexts { get; set; } = string.Empty; // JSON array of CreateTextSlideDto objects
}


// For separated workflow - JSON-only creation with photo positions
public class CreatePresentationWithPositionsDto
{
    // Title properties
    [Required]
    public string TitleText { get; set; } = string.Empty;
    
    [Required]
    public string TitleFont { get; set; } = "Arial";
    
    [Required]
    [Range(1, 100, ErrorMessage = "Title font size must be between 1 and 100")]
    public int TitleSize { get; set; } = 24;
    
    [Range(0, 33.867, ErrorMessage = "Title Left position must be between 0 and 33.867 cm")]
    public double TitleLeft { get; set; } = 0;
    
    [Range(0, 19.05, ErrorMessage = "Title Top position must be between 0 and 19.05 cm")]
    public double TitleTop { get; set; } = 0;
    
    [Range(1, 33.867, ErrorMessage = "Title Width must be between 1 and 33.867 cm")]
    public double TitleWidth { get; set; } = 10;
    
    [Range(1, 19.05, ErrorMessage = "Title Height must be between 1 and 19.05 cm")]
    public double TitleHeight { get; set; } = 2;
    
    // Author properties
    [Required]
    public string AuthorText { get; set; } = string.Empty;
    
    [Required]
    public string AuthorFont { get; set; } = "Arial";
    
    [Required]
    [Range(1, 100, ErrorMessage = "Author font size must be between 1 and 100")]
    public int AuthorSize { get; set; } = 18;
    
    [Range(0, 33.867, ErrorMessage = "Author Left position must be between 0 and 33.867 cm")]
    public double AuthorLeft { get; set; } = 0;
    
    [Range(0, 19.05, ErrorMessage = "Author Top position must be between 0 and 19.05 cm")]
    public double AuthorTop { get; set; } = 0;
    
    [Range(1, 33.867, ErrorMessage = "Author Width must be between 1 and 33.867 cm")]
    public double AuthorWidth { get; set; } = 8;
    
    [Range(1, 19.05, ErrorMessage = "Author Height must be between 1 and 19.05 cm")]
    public double AuthorHeight { get; set; } = 1.5;
    
    // Plan properties
    [Required]
    public string PlanText { get; set; } = string.Empty;
    
    [Required]
    public string PlanFont { get; set; } = "Arial";
    
    [Required]
    [Range(1, 100, ErrorMessage = "Plan font size must be between 1 and 100")]
    public int PlanSize { get; set; } = 14;
    
    [Range(0, 33.867, ErrorMessage = "Plan Left position must be between 0 and 33.867 cm")]
    public double PlanLeft { get; set; } = 0;
    
    [Range(0, 19.05, ErrorMessage = "Plan Top position must be between 0 and 19.05 cm")]
    public double PlanTop { get; set; } = 0;
    
    [Range(1, 33.867, ErrorMessage = "Plan Width must be between 1 and 33.867 cm")]
    public double PlanWidth { get; set; } = 15;
    
    [Range(1, 19.05, ErrorMessage = "Plan Height must be between 1 and 19.05 cm")]
    public double PlanHeight { get; set; } = 8;
    
    // General properties
    [Required]
    public int DesignId { get; set; }
    
    [Required]
    [Range(1, 100, ErrorMessage = "Page count must be between 1 and 100")]
    public int PageCount { get; set; } = 10;
    
    public bool WithPhoto { get; set; } = false;
    
    public bool IsActive { get; set; } = true;
    
    // FilePath is optional
    public string FilePath { get; set; } = string.Empty;
    
    // Photo positions (no actual files) - required when WithPhoto is true
    public List<PhotoPositionDto> PhotoPositions { get; set; } = new();
    
    // Page-specific WithPhoto settings - determines which pages should have photos
    // If empty, defaults will be applied: first 2 pages = false, rest based on WithPhoto flag
    public List<PageWithPhotoDto> PageSettings { get; set; } = new();
    
    // Post texts as objects (not JSON string)
    [Required]
    public List<CreateTextSlideDto> PostTexts { get; set; } = new();
}

// DTO for specifying which pages should have photos
public class PageWithPhotoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be positive")]
    public int PageNumber { get; set; }
    
    public bool WithPhoto { get; set; }
}

// For photo upload endpoint
public class UpdatePresentationPhotosDto
{
    [Required]
    public List<IFormFile> Photos { get; set; } = new();
}

// Keep original for backward compatibility if needed
public class CreatePresentationDto
{
    [Required]
    public CreateTextSlideDto Title { get; set; } = new();
    
    [Required]
    public CreateTextSlideDto Author { get; set; } = new();
    
    [Required]
    public CreatePlanDto PlanData { get; set; } = new();
    
    [Required]
    public int DesignId { get; set; }
    
    [Required]
    [Range(1, 100, ErrorMessage = "Page count must be between 1 and 100")]
    public int PageCount { get; set; } = 10;
    
    public bool WithPhoto { get; set; } = false;
    
    public List<CreatePhotoSlideWithPositionDto> Photos { get; set; } = new();
    
    [Required]
    [MinLength(1, ErrorMessage = "At least one post text is required")]
    public List<CreateTextSlideDto> PostTexts { get; set; } = new();
    
    public bool IsActive { get; set; } = true;
    public string FilePath { get; set; } = string.Empty;
}

public class CreatePhotoSlideWithPositionDto
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

public class UpdatePresentationDto
{
    public UpdateTextSlideDto? Title { get; set; }
    
    public UpdateTextSlideDto? Author { get; set; }
    
    public int? DesignId { get; set; }
    public int? PlanId { get; set; }
    public bool? IsActive { get; set; }
    public string? FilePath { get; set; }
}

public class PresentationDto
{
    public int Id { get; set; }
    public TextSlideDto Title { get; set; } = new();
    public TextSlideDto Author { get; set; } = new();
    public bool WithPhoto { get; set; }
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
    public TextSlideSummaryDto Title { get; set; } = new();
    public TextSlideSummaryDto Author { get; set; } = new();
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