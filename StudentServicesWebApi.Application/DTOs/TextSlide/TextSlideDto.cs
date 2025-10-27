using System.ComponentModel.DataAnnotations;
using GemBox.Presentation;
using Swashbuckle.AspNetCore.Annotations;

namespace StudentServicesWebApi.Application.DTOs.TextSlide;

public class CreateTextSlideDto
{
    [Required(ErrorMessage = "Text is required")]
    [StringLength(5000, ErrorMessage = "Text cannot exceed 5000 characters")]
    [SwaggerSchema(Description = "The text content to display on the slide. Maximum 5000 characters.")]
    public string Text { get; set; } = string.Empty;

    [Required]
    [Range(1, 200, ErrorMessage = "Size must be between 1 and 200")]
    [SwaggerSchema(Description = "Font size for the text. Must be between 1 and 200 points.")]
    public int Size { get; set; } = 12;
    [Required(ErrorMessage = "Font is required")]
    [StringLength(100, ErrorMessage = "Font name cannot exceed 100 characters")]
    [SwaggerSchema(Description = "Font family name for the text. Common examples: Arial, Times New Roman, Calibri.")]
    public string Font { get; set; } = "Arial";
    [SwaggerSchema(Description = "Apply bold formatting to the text.")]
    public bool IsBold { get; set; } = false;
    [SwaggerSchema(Description = "Apply italic formatting to the text.")]
    public bool IsItalic { get; set; } = false;
    [Required(ErrorMessage = "ColorHex is required")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "ColorHex must be a valid hex color (e.g., #000000, #FFF)")]
    [SwaggerSchema(Description = "Text color in hexadecimal format. Supports both 3-digit (#FFF) and 6-digit (#FFFFFF) formats.")]
    public string ColorHex { get; set; } = "#000000";
    [Required]
    [Range(0, 33.867, ErrorMessage = "Left position must be between 0 and 33.867 centimeters")]
    [SwaggerSchema(Description = "Left position of the text element in centimeters. Must be between 0 and 33.867 cm (slide width limit).")]
    public double Left { get; set; } = 0;
    [Required]
    [Range(0, 19.05, ErrorMessage = "Top position must be between 0 and 19.05 centimeters")]
    [SwaggerSchema(Description = "Top position of the text element in centimeters. Must be between 0 and 19.05 cm (slide height limit).")]
    public double Top { get; set; } = 0;
    [Required]
    [Range(0.1, 33.867, ErrorMessage = "Width must be between 0.1 and 33.867 centimeters")]
    [SwaggerSchema(Description = "Width of the text element in centimeters. Must be between 0.1 and 33.867 cm (slide width limit).")]
    public double Width { get; set; } = 10.0;
    [Range(0.1, 19.05, ErrorMessage = "Height must be between 0.1 and 19.05 centimeters")]
    [SwaggerSchema(Description = "Height of the text element in centimeters. Optional - auto-calculated if not provided. Must be between 0.1 and 19.05 cm (slide height limit).")]
    public double? Height { get; set; }
    [SwaggerSchema(Description = "Horizontal text alignment. Options: Left, Center, Right.")]
    public HorizontalAlignment Horizontal { get; set; } = HorizontalAlignment.Left;
    [SwaggerSchema(Description = "Vertical text alignment. Options: Top, Middle, Bottom.")]
    public VerticalAlignment Vertical { get; set; } = VerticalAlignment.Top;
}
public class UpdateTextSlideDto
{
    [StringLength(5000, ErrorMessage = "Text cannot exceed 5000 characters")]
    [SwaggerSchema(Description = "The text content to display on the slide. Maximum 5000 characters.")]
    public string? Text { get; set; }
    [Range(1, 200, ErrorMessage = "Size must be between 1 and 200")]
    [SwaggerSchema(Description = "Font size for the text. Must be between 1 and 200 points.")]
    public int? Size { get; set; }
    [StringLength(100, ErrorMessage = "Font name cannot exceed 100 characters")]
    [SwaggerSchema(Description = "Font family name for the text. Common examples: Arial, Times New Roman, Calibri.")]
    public string? Font { get; set; }
    [SwaggerSchema(Description = "Apply bold formatting to the text.")]
    public bool? IsBold { get; set; }
    [SwaggerSchema(Description = "Apply italic formatting to the text.")]
    public bool? IsItalic { get; set; }
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "ColorHex must be a valid hex color (e.g., #000000, #FFF)")]
    [SwaggerSchema(Description = "Text color in hexadecimal format. Supports both 3-digit (#FFF) and 6-digit (#FFFFFF) formats.")]
    public string? ColorHex { get; set; }
    [Range(0, 33.867, ErrorMessage = "Left position must be between 0 and 33.867 centimeters")]
    [SwaggerSchema(Description = "Left position of the text element in centimeters. Must be between 0 and 33.867 cm (slide width limit).")]
    public double? Left { get; set; }
    [Range(0, 19.05, ErrorMessage = "Top position must be between 0 and 19.05 centimeters")]
    [SwaggerSchema(Description = "Top position of the text element in centimeters. Must be between 0 and 19.05 cm (slide height limit).")]
    public double? Top { get; set; }
    [Range(0.1, 33.867, ErrorMessage = "Width must be between 0.1 and 33.867 centimeters")]
    [SwaggerSchema(Description = "Width of the text element in centimeters. Must be between 0.1 and 33.867 cm (slide width limit).")]
    public double? Width { get; set; }
    [Range(0.1, 19.05, ErrorMessage = "Height must be between 0.1 and 19.05 centimeters")]
    [SwaggerSchema(Description = "Height of the text element in centimeters. Must be between 0.1 and 19.05 cm (slide height limit).")]
    public double? Height { get; set; }
    [SwaggerSchema(Description = "Horizontal text alignment. Options: Left, Center, Right.")]
    public HorizontalAlignment? Horizontal { get; set; }
    [SwaggerSchema(Description = "Vertical text alignment. Options: Top, Middle, Bottom.")]
    public VerticalAlignment? Vertical { get; set; }
}
public class TextSlideDto
{
    [SwaggerSchema(Description = "Unique identifier for the text slide.")]
    public int Id { get; set; }
    [SwaggerSchema(Description = "Text content of the slide.")]
    public string Text { get; set; } = string.Empty;
    [SwaggerSchema(Description = "Font size for the text in points.")]
    public int Size { get; set; }
    [SwaggerSchema(Description = "Font family name for the text.")]
    public string Font { get; set; } = string.Empty;
    [SwaggerSchema(Description = "Whether the text has bold formatting.")]
    public bool IsBold { get; set; }
    [SwaggerSchema(Description = "Whether the text has italic formatting.")]
    public bool IsItalic { get; set; }
    [SwaggerSchema(Description = "Color of the text in hexadecimal format.")]
    public string ColorHex { get; set; } = string.Empty;
    [SwaggerSchema(Description = "Left position of the text element in centimeters.")]
    public double Left { get; set; }
    [SwaggerSchema(Description = "Top position of the text element in centimeters.")]
    public double Top { get; set; }
    [SwaggerSchema(Description = "Width of the text element in centimeters.")]
    public double Width { get; set; }
    [SwaggerSchema(Description = "Height of the text element in centimeters.")]
    public double? Height { get; set; }
    [SwaggerSchema(Description = "Horizontal text alignment.")]
    public HorizontalAlignment Horizontal { get; set; }
    [SwaggerSchema(Description = "Vertical text alignment.")]
    public VerticalAlignment Vertical { get; set; }
    [SwaggerSchema(Description = "When the text slide was created.")]
    public DateTime CreatedAt { get; set; }
    [SwaggerSchema(Description = "When the text slide was last updated.")]
    public DateTime UpdatedAt { get; set; }
}
public class TextSlideSummaryDto
{
    [SwaggerSchema(Description = "Unique identifier for the text slide.")]
    public int Id { get; set; }
    [SwaggerSchema(Description = "Preview of the text content (first 100 characters).")]
    public string TextPreview { get; set; } = string.Empty;
    [SwaggerSchema(Description = "Font size for the text in points.")]
    public int Size { get; set; }
    [SwaggerSchema(Description = "Font family name for the text.")]
    public string Font { get; set; } = string.Empty;
    [SwaggerSchema(Description = "Whether the text has bold formatting.")]
    public bool IsBold { get; set; }
    [SwaggerSchema(Description = "Whether the text has italic formatting.")]
    public bool IsItalic { get; set; }
    [SwaggerSchema(Description = "Color of the text in hexadecimal format.")]
    public string ColorHex { get; set; } = string.Empty;
    [SwaggerSchema(Description = "Summary of the text element's position and dimensions.")]
    public string PositionSummary { get; set; } = string.Empty;
    [SwaggerSchema(Description = "When the text slide was created.")]
    public DateTime CreatedAt { get; set; }
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