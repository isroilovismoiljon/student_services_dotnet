using System.ComponentModel.DataAnnotations;
using GemBox.Presentation;
using Swashbuckle.AspNetCore.Annotations;

namespace StudentServicesWebApi.Application.DTOs.TextSlide;

/// <summary>
/// DTO for creating a new TextSlide
/// </summary>
public class CreateTextSlideDto
{
    /// <summary>
    /// Text content of the slide
    /// </summary>
    /// <example>Welcome to our presentation!</example>
    [Required(ErrorMessage = "Text is required")]
    [StringLength(5000, ErrorMessage = "Text cannot exceed 5000 characters")]
    [SwaggerSchema(Description = "The text content to display on the slide. Maximum 5000 characters.")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Font size for the text in points
    /// </summary>
    /// <example>24</example>
    [Required]
    [Range(1, 200, ErrorMessage = "Size must be between 1 and 200")]
    [SwaggerSchema(Description = "Font size for the text. Must be between 1 and 200 points.")]
    public int Size { get; set; } = 12;

    /// <summary>
    /// Font family for the text
    /// </summary>
    /// <example>Arial</example>
    [Required(ErrorMessage = "Font is required")]
    [StringLength(100, ErrorMessage = "Font name cannot exceed 100 characters")]
    [SwaggerSchema(Description = "Font family name for the text. Common examples: Arial, Times New Roman, Calibri.")]
    public string Font { get; set; } = "Arial";

    /// <summary>
    /// Whether the text should be bold
    /// </summary>
    /// <example>false</example>
    [SwaggerSchema(Description = "Apply bold formatting to the text.")]
    public bool IsBold { get; set; } = false;

    /// <summary>
    /// Whether the text should be italic
    /// </summary>
    /// <example>false</example>
    [SwaggerSchema(Description = "Apply italic formatting to the text.")]
    public bool IsItalic { get; set; } = false;

    /// <summary>
    /// Color of the text in hexadecimal format
    /// </summary>
    /// <example>#000000</example>
    [Required(ErrorMessage = "ColorHex is required")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "ColorHex must be a valid hex color (e.g., #000000, #FFF)")]
    [SwaggerSchema(Description = "Text color in hexadecimal format. Supports both 3-digit (#FFF) and 6-digit (#FFFFFF) formats.")]
    public string ColorHex { get; set; } = "#000000";

    /// <summary>
    /// Left position of the text element in centimeters
    /// </summary>
    /// <example>2.5</example>
    [Required]
    [Range(0, 33.867, ErrorMessage = "Left position must be between 0 and 33.867 centimeters")]
    [SwaggerSchema(Description = "Left position of the text element in centimeters. Must be between 0 and 33.867 cm (slide width limit).")]
    public double Left { get; set; } = 0;

    /// <summary>
    /// Top position of the text element in centimeters
    /// </summary>
    /// <example>1.5</example>
    [Required]
    [Range(0, 19.05, ErrorMessage = "Top position must be between 0 and 19.05 centimeters")]
    [SwaggerSchema(Description = "Top position of the text element in centimeters. Must be between 0 and 19.05 cm (slide height limit).")]
    public double Top { get; set; } = 0;

    /// <summary>
    /// Width of the text element in centimeters
    /// </summary>
    /// <example>10.0</example>
    [Required]
    [Range(0.1, 33.867, ErrorMessage = "Width must be between 0.1 and 33.867 centimeters")]
    [SwaggerSchema(Description = "Width of the text element in centimeters. Must be between 0.1 and 33.867 cm (slide width limit).")]
    public double Width { get; set; } = 10.0;

    /// <summary>
    /// Height of the text element in centimeters (optional, auto-calculated if not provided)
    /// </summary>
    /// <example>3.0</example>
    [Range(0.1, 19.05, ErrorMessage = "Height must be between 0.1 and 19.05 centimeters")]
    [SwaggerSchema(Description = "Height of the text element in centimeters. Optional - auto-calculated if not provided. Must be between 0.1 and 19.05 cm (slide height limit).")]
    public double? Height { get; set; }

    /// <summary>
    /// Horizontal alignment of the text
    /// </summary>
    /// <example>Left</example>
    [SwaggerSchema(Description = "Horizontal text alignment. Options: Left, Center, Right.")]
    public HorizontalAlignment Horizontal { get; set; } = HorizontalAlignment.Left;

    /// <summary>
    /// Vertical alignment of the text
    /// </summary>
    /// <example>Top</example>
    [SwaggerSchema(Description = "Vertical text alignment. Options: Top, Middle, Bottom.")]
    public VerticalAlignment Vertical { get; set; } = VerticalAlignment.Top;
}

/// <summary>
/// DTO for updating an existing TextSlide
/// </summary>
public class UpdateTextSlideDto
{
    /// <summary>
    /// Text content of the slide
    /// </summary>
    /// <example>Updated presentation text!</example>
    [StringLength(5000, ErrorMessage = "Text cannot exceed 5000 characters")]
    [SwaggerSchema(Description = "The text content to display on the slide. Maximum 5000 characters.")]
    public string? Text { get; set; }

    /// <summary>
    /// Font size for the text in points
    /// </summary>
    /// <example>28</example>
    [Range(1, 200, ErrorMessage = "Size must be between 1 and 200")]
    [SwaggerSchema(Description = "Font size for the text. Must be between 1 and 200 points.")]
    public int? Size { get; set; }

    /// <summary>
    /// Font family for the text
    /// </summary>
    /// <example>Times New Roman</example>
    [StringLength(100, ErrorMessage = "Font name cannot exceed 100 characters")]
    [SwaggerSchema(Description = "Font family name for the text. Common examples: Arial, Times New Roman, Calibri.")]
    public string? Font { get; set; }

    /// <summary>
    /// Whether the text should be bold
    /// </summary>
    /// <example>true</example>
    [SwaggerSchema(Description = "Apply bold formatting to the text.")]
    public bool? IsBold { get; set; }

    /// <summary>
    /// Whether the text should be italic
    /// </summary>
    /// <example>false</example>
    [SwaggerSchema(Description = "Apply italic formatting to the text.")]
    public bool? IsItalic { get; set; }

    /// <summary>
    /// Color of the text in hexadecimal format
    /// </summary>
    /// <example>#FF5733</example>
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "ColorHex must be a valid hex color (e.g., #000000, #FFF)")]
    [SwaggerSchema(Description = "Text color in hexadecimal format. Supports both 3-digit (#FFF) and 6-digit (#FFFFFF) formats.")]
    public string? ColorHex { get; set; }

    /// <summary>
    /// Left position of the text element in centimeters
    /// </summary>
    /// <example>5.0</example>
    [Range(0, 33.867, ErrorMessage = "Left position must be between 0 and 33.867 centimeters")]
    [SwaggerSchema(Description = "Left position of the text element in centimeters. Must be between 0 and 33.867 cm (slide width limit).")]
    public double? Left { get; set; }

    /// <summary>
    /// Top position of the text element in centimeters
    /// </summary>
    /// <example>3.0</example>
    [Range(0, 19.05, ErrorMessage = "Top position must be between 0 and 19.05 centimeters")]
    [SwaggerSchema(Description = "Top position of the text element in centimeters. Must be between 0 and 19.05 cm (slide height limit).")]
    public double? Top { get; set; }

    /// <summary>
    /// Width of the text element in centimeters
    /// </summary>
    /// <example>15.0</example>
    [Range(0.1, 33.867, ErrorMessage = "Width must be between 0.1 and 33.867 centimeters")]
    [SwaggerSchema(Description = "Width of the text element in centimeters. Must be between 0.1 and 33.867 cm (slide width limit).")]
    public double? Width { get; set; }

    /// <summary>
    /// Height of the text element in centimeters
    /// </summary>
    /// <example>2.5</example>
    [Range(0.1, 19.05, ErrorMessage = "Height must be between 0.1 and 19.05 centimeters")]
    [SwaggerSchema(Description = "Height of the text element in centimeters. Must be between 0.1 and 19.05 cm (slide height limit).")]
    public double? Height { get; set; }

    /// <summary>
    /// Horizontal alignment of the text
    /// </summary>
    /// <example>Center</example>
    [SwaggerSchema(Description = "Horizontal text alignment. Options: Left, Center, Right.")]
    public HorizontalAlignment? Horizontal { get; set; }

    /// <summary>
    /// Vertical alignment of the text
    /// </summary>
    /// <example>Middle</example>
    [SwaggerSchema(Description = "Vertical text alignment. Options: Top, Middle, Bottom.")]
    public VerticalAlignment? Vertical { get; set; }
}

/// <summary>
/// DTO for viewing TextSlide details
/// </summary>
public class TextSlideDto
{
    /// <summary>
    /// Unique identifier for the text slide
    /// </summary>
    /// <example>123</example>
    [SwaggerSchema(Description = "Unique identifier for the text slide.")]
    public int Id { get; set; }

    /// <summary>
    /// Text content of the slide
    /// </summary>
    /// <example>Welcome to our presentation!</example>
    [SwaggerSchema(Description = "Text content of the slide.")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Font size for the text in points
    /// </summary>
    /// <example>24</example>
    [SwaggerSchema(Description = "Font size for the text in points.")]
    public int Size { get; set; }

    /// <summary>
    /// Font family for the text
    /// </summary>
    /// <example>Arial</example>
    [SwaggerSchema(Description = "Font family name for the text.")]
    public string Font { get; set; } = string.Empty;

    /// <summary>
    /// Whether the text is bold
    /// </summary>
    /// <example>false</example>
    [SwaggerSchema(Description = "Whether the text has bold formatting.")]
    public bool IsBold { get; set; }

    /// <summary>
    /// Whether the text is italic
    /// </summary>
    /// <example>false</example>
    [SwaggerSchema(Description = "Whether the text has italic formatting.")]
    public bool IsItalic { get; set; }

    /// <summary>
    /// Color of the text in hexadecimal format
    /// </summary>
    /// <example>#000000</example>
    [SwaggerSchema(Description = "Color of the text in hexadecimal format.")]
    public string ColorHex { get; set; } = string.Empty;

    /// <summary>
    /// Left position of the text element in centimeters
    /// </summary>
    /// <example>2.5</example>
    [SwaggerSchema(Description = "Left position of the text element in centimeters.")]
    public double Left { get; set; }

    /// <summary>
    /// Top position of the text element in centimeters
    /// </summary>
    /// <example>1.5</example>
    [SwaggerSchema(Description = "Top position of the text element in centimeters.")]
    public double Top { get; set; }

    /// <summary>
    /// Width of the text element in centimeters
    /// </summary>
    /// <example>10.0</example>
    [SwaggerSchema(Description = "Width of the text element in centimeters.")]
    public double Width { get; set; }

    /// <summary>
    /// Height of the text element in centimeters
    /// </summary>
    /// <example>3.0</example>
    [SwaggerSchema(Description = "Height of the text element in centimeters.")]
    public double? Height { get; set; }

    /// <summary>
    /// Horizontal alignment of the text
    /// </summary>
    /// <example>Left</example>
    [SwaggerSchema(Description = "Horizontal text alignment.")]
    public HorizontalAlignment Horizontal { get; set; }

    /// <summary>
    /// Vertical alignment of the text
    /// </summary>
    /// <example>Top</example>
    [SwaggerSchema(Description = "Vertical text alignment.")]
    public VerticalAlignment Vertical { get; set; }

    /// <summary>
    /// When the text slide was created
    /// </summary>
    /// <example>2024-10-04T06:00:00Z</example>
    [SwaggerSchema(Description = "When the text slide was created.")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the text slide was last updated
    /// </summary>
    /// <example>2024-10-04T08:30:00Z</example>
    [SwaggerSchema(Description = "When the text slide was last updated.")]
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Simplified DTO for listing text slides
/// </summary>
public class TextSlideSummaryDto
{
    /// <summary>
    /// Unique identifier for the text slide
    /// </summary>
    /// <example>456</example>
    [SwaggerSchema(Description = "Unique identifier for the text slide.")]
    public int Id { get; set; }

    /// <summary>
    /// Preview of the text content (first 100 characters)
    /// </summary>
    /// <example>Welcome to our presentation! This is a sample...</example>
    [SwaggerSchema(Description = "Preview of the text content (first 100 characters).")]
    public string TextPreview { get; set; } = string.Empty;

    /// <summary>
    /// Font size for the text in points
    /// </summary>
    /// <example>18</example>
    [SwaggerSchema(Description = "Font size for the text in points.")]
    public int Size { get; set; }

    /// <summary>
    /// Font family for the text
    /// </summary>
    /// <example>Calibri</example>
    [SwaggerSchema(Description = "Font family name for the text.")]
    public string Font { get; set; } = string.Empty;

    /// <summary>
    /// Whether the text has bold formatting
    /// </summary>
    /// <example>true</example>
    [SwaggerSchema(Description = "Whether the text has bold formatting.")]
    public bool IsBold { get; set; }

    /// <summary>
    /// Whether the text has italic formatting
    /// </summary>
    /// <example>false</example>
    [SwaggerSchema(Description = "Whether the text has italic formatting.")]
    public bool IsItalic { get; set; }

    /// <summary>
    /// Color of the text in hexadecimal format
    /// </summary>
    /// <example>#2E86AB</example>
    [SwaggerSchema(Description = "Color of the text in hexadecimal format.")]
    public string ColorHex { get; set; } = string.Empty;

    /// <summary>
    /// Position and size summary
    /// </summary>
    /// <example>Left: 2.5cm, Top: 1.5cm, Size: 10.0x3.0cm</example>
    [SwaggerSchema(Description = "Summary of the text element's position and dimensions.")]
    public string PositionSummary { get; set; } = string.Empty;

    /// <summary>
    /// When the text slide was created
    /// </summary>
    /// <example>2024-10-04T06:00:00Z</example>
    [SwaggerSchema(Description = "When the text slide was created.")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the text slide was last updated
    /// </summary>
    /// <example>2024-10-04T08:30:00Z</example>
    [SwaggerSchema(Description = "When the text slide was last updated.")]
    public DateTime UpdatedAt { get; set; }
}

public class BulkTextSlideOperationDto
{
    [Required(ErrorMessage = "TextSlideIds is required")]
    [MinLength(1, ErrorMessage = "At least one text slide ID is required")]
    [SwaggerSchema(Description = "List of text slide IDs to operate on. Must contain at least one ID.")]
    public List<int> TextSlideIds { get; set; } = new();
}

public class BulkCreateTextSlideDto
{
    [Required(ErrorMessage = "TextSlides is required")]
    [MinLength(1, ErrorMessage = "At least one text slide is required")]
    [SwaggerSchema(Description = "List of text slides to create in bulk. Must contain at least one slide definition.")]
    public List<CreateTextSlideDto> TextSlides { get; set; } = new();
}