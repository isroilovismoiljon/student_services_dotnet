# Swagger Mixed Input Implementation

This document explains how the new `CreatePresentationMixedDto` approach enables Swagger to display separate input fields instead of a single `jsonData` field.

## What Was Changed

### 1. New DTO Structure (`CreatePresentationMixedDto`)

Instead of using `[FromForm] string jsonData` and manual JSON parsing, I created a dedicated DTO with individual properties:

```csharp
public class CreatePresentationMixedDto
{
    // Title properties - these appear as separate input fields in Swagger
    [Required]
    public string TitleText { get; set; } = string.Empty;
    
    [Required]
    public string TitleFont { get; set; } = "Arial";
    
    [Required]
    [Range(1, 100, ErrorMessage = "Title font size must be between 1 and 100")]
    public int TitleSize { get; set; } = 24;
    
    [Range(0, 33.867, ErrorMessage = "Title Left position must be between 0 and 33.867 cm")]
    public double TitleLeft { get; set; } = 0;
    
    // ... (more title properties)
    
    // Author properties - separate input fields
    [Required]
    public string AuthorText { get; set; } = string.Empty;
    
    // ... (author properties)
    
    // Plan properties - separate input fields
    [Required]
    public string PlanText { get; set; } = string.Empty;
    
    // ... (plan properties)
    
    // General properties
    [Required]
    public int DesignId { get; set; }
    
    [Required]
    [Range(1, 100, ErrorMessage = "Page count must be between 1 and 100")]
    public int PageCount { get; set; } = 10;
    
    public bool WithPhoto { get; set; } = false;
    
    // Photo upload fields - appear as file upload inputs in Swagger
    public List<IFormFile> Photos { get; set; } = new();
    
    // String fields for complex data
    public string PhotoPositions { get; set; } = string.Empty; // Format: "left1,top1,width1,height1|left2,top2,width2,height2"
    public string PostTexts { get; set; } = string.Empty; // JSON array of text objects
    
    // Other properties...
}
```

### 2. Updated Controller Method

The controller method now accepts the mixed DTO directly:

```csharp
[HttpPost]
[Consumes("multipart/form-data")]
public async Task<IActionResult> CreatePresentation(
    [FromForm] CreatePresentationMixedDto mixedDto,
    CancellationToken ct = default)
{
    // Direct validation using ModelState - no manual JSON parsing needed
    if (!ModelState.IsValid)
        return BadRequest(new { success = false, message = "Invalid input data", errors = ModelState });
    
    // Parse complex fields and map to service layer DTO
    // ...
}
```

### 3. Comprehensive FluentValidation

Created `CreatePresentationMixedDtoValidator` with validation for all fields:

```csharp
public class CreatePresentationMixedDtoValidator : AbstractValidator<CreatePresentationMixedDto>
{
    public CreatePresentationMixedDtoValidator()
    {
        // Individual validation for each property
        RuleFor(x => x.TitleText).NotEmpty().WithMessage("Title text is required");
        RuleFor(x => x.TitleSize).InclusiveBetween(1, 100);
        
        // Conditional validation for photos when WithPhoto is true
        RuleFor(x => x.Photos)
            .Must((dto, photos) => 
            {
                if (!dto.WithPhoto) return true;
                var requiredCount = (dto.PageCount - 2) / 2;
                return photos != null && photos.Count == requiredCount;
            });
        // ...
    }
}
```

## How It Appears in Swagger UI

### Before (Single JSON Field)
Previously, Swagger showed:
- `jsonData` (string field) - requiring manual JSON entry
- `photos` (file array) 

### After (Separate Fields)
Now Swagger shows individual input fields:

**Text Fields:**
- `TitleText` (text input)
- `TitleFont` (text input)  
- `TitleSize` (number input, 1-100)
- `TitleLeft` (number input, 0-33.867)
- `TitleTop` (number input, 0-19.05)
- `TitleWidth` (number input, 1-33.867)
- `TitleHeight` (number input, 1-19.05)
- `AuthorText` (text input)
- `AuthorFont` (text input)
- `AuthorSize` (number input, 1-100)
- `AuthorLeft` (number input, 0-33.867)
- `AuthorTop` (number input, 0-19.05)
- `AuthorWidth` (number input, 1-33.867)
- `AuthorHeight` (number input, 1-19.05)
- `PlanText` (text input)
- `PlanFont` (text input)
- `PlanSize` (number input, 1-100)
- `PlanLeft` (number input, 0-33.867)
- `PlanTop` (number input, 0-19.05)
- `PlanWidth` (number input, 1-33.867)
- `PlanHeight` (number input, 1-19.05)
- `DesignId` (number input)
- `PageCount` (number input, 1-100)
- `WithPhoto` (boolean checkbox)
- `IsActive` (boolean checkbox)
- `FilePath` (text input, optional)
- `Photo1Left`, `Photo1Top`, `Photo1Width`, `Photo1Height` (number inputs for Photo 1 position)
- `Photo2Left`, `Photo2Top`, `Photo2Width`, `Photo2Height` (number inputs for Photo 2 position)
- `Photo3Left`, `Photo3Top`, `Photo3Width`, `Photo3Height` (number inputs for Photo 3 position)
- `Photo4Left`, `Photo4Top`, `Photo4Width`, `Photo4Height` (number inputs for Photo 4 position)
- `Photo5Left`, `Photo5Top`, `Photo5Width`, `Photo5Height` (number inputs for Photo 5 position)
- `PostTexts` (text input, required JSON array of CreateTextSlideDto objects)

**File Upload Fields:**
- `Photos` (file array upload) - Multiple file selection

## Benefits

1. **User-Friendly**: Each property has its own labeled input field
2. **Type Validation**: Number fields have min/max constraints shown in UI
3. **Required Fields**: Clearly marked with asterisks
4. **File Uploads**: Proper file selection UI for photos
5. **Validation Messages**: Field-specific error messages
6. **No Manual JSON**: Users don't need to construct JSON manually

## Validation Rules

All validation is handled automatically by:
- Data annotations on DTO properties
- FluentValidation rules
- ASP.NET Core model binding
- Custom business logic for photo count validation

## Backend Processing

The controller:
1. Accepts the mixed DTO via model binding
2. Validates all fields automatically
3. Parses complex string fields (PhotoPositions, PostTexts) 
4. Maps to the existing service layer DTO structure
5. Calls the existing service method without changes

## Usage Example

Frontend can now send a `multipart/form-data` request with individual fields:

```
Content-Type: multipart/form-data

TitleText: "My Presentation"
TitleFont: "Arial"  
TitleSize: 24
TitleLeft: 2.5
TitleTop: 1.0
TitleWidth: 10.0
TitleHeight: 2.0
AuthorText: "John Doe"
AuthorFont: "Arial"
AuthorSize: 18
PlanText: "My Plan Content"
PlanFont: "Arial"
PlanSize: 14
DesignId: 1
PageCount: 10
WithPhoto: true
Photo1Left: 5.0
Photo1Top: 5.0
Photo1Width: 100.0
Photo1Height: 50.0
Photo2Left: 10.0
Photo2Top: 10.0
Photo2Width: 90.0
Photo2Height: 45.0
PostTexts: [{"text":"Post 1","font":"Arial","size":14,"left":0,"top":0,"width":10,"height":2},{"text":"Post 2","font":"Arial","size":12,"left":0,"top":3,"width":8,"height":1.5}]
Photos: [file1.jpg, file2.jpg]
```

This provides a much cleaner and more intuitive API interface for frontend developers and API consumers.

## Migration Path

The implementation maintains backward compatibility by:
- Keeping the existing service layer unchanged
- Mapping the new DTO structure to the existing `CreatePresentationDto`
- Preserving all existing business logic and validation rules
- Maintaining the same response format

This approach successfully addresses your requirement to display each property separately in Swagger while maintaining full functionality.