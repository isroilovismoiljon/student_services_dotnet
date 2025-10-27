# Separated Presentation Workflow Implementation Summary

## Overview
This document summarizes the implementation of the separated presentation creation and photo upload workflow as requested. The system now supports creating presentations with photo positions (JSON only) and uploading actual photo files separately.

## Key Changes Made

### 1. Domain Model Updates

#### PresentationPage Entity
```csharp
public class PresentationPage : BaseEntity
{
    // ... existing properties
    public bool WithPhoto { get; set; } = false;
    // ... rest of properties
}
```
- Added `WithPhoto` property to track which pages should have photos
- Database migration applied successfully

### 2. New DTOs for Separated Workflow

#### CreatePresentationWithPositionsDto
- **Purpose**: JSON-only creation of presentations with photo positions
- **Content-Type**: `application/json`
- **Fields**:
  - All presentation text data (Title, Author, Plan properties)
  - `PhotoPositions`: Array of position objects (Left, Top, Width, Height)
  - `PageSettings`: Optional array to specify which pages should have photos
  - `PostTexts`: Array of CreateTextSlideDto objects
  - No file uploads - only position data

#### UpdatePresentationPhotosDto  
- **Purpose**: Photo file upload for existing presentations
- **Content-Type**: `multipart/form-data`
- **Fields**:
  - `Photos`: Array of IFormFile objects

### 3. API Endpoints

#### POST /api/Presentation (CreatePresentation)
- **Input**: `CreatePresentationWithPositionsDto` (JSON)
- **Functionality**:
  - Creates presentation with all text content
  - Creates placeholder PhotoSlides with positions but no actual files
  - Sets up PresentationPages with correct `WithPhoto` values
  - Validates photo position count: `(PageCount - 2) / 2` when `WithPhoto = true`
  
#### PUT /api/Presentation/{id}/photos (UpdatePresentationPhotos)
- **Input**: `UpdatePresentationPhotosDto` (multipart/form-data)
- **Functionality**:
  - Validates presentation exists and has `WithPhoto = true`
  - Uploads actual photo files
  - Links photos to existing placeholder PhotoSlides
  - Updates PhotoSlide entities with real file data

### 4. Business Logic Implementation

#### Photo Position Logic
- When `WithPhoto = false`: No photo positions required
- When `WithPhoto = true`: Exactly `(PageCount - 2) / 2` photo positions required
- First 2 pages never have photos (by default)
- Pages 3+ can have photos based on user settings

#### Page Creation Logic
```csharp
// Default behavior (if no PageSettings provided)
bool pageWithPhoto = createDto.WithPhoto && i >= 2; // i is 0-based page index

// Custom behavior (if PageSettings provided)
var pageSetting = createDto.PageSettings.FirstOrDefault(p => p.PageNumber == i + 1);
bool pageWithPhoto = pageSetting?.WithPhoto ?? false;
```

#### Placeholder PhotoSlide Creation
- Creates PhotoSlides with position data but placeholder file info
- `PhotoPath = "placeholder"`
- `OriginalFileName = "placeholder"`
- `FileSize = 0`
- Position data (Left, Top, Width, Height) preserved

### 5. Service Layer Enhancements

#### IPresentationService
```csharp
Task<PresentationDto> CreatePresentationWithPositionsAsync(CreatePresentationWithPositionsDto createDto, CancellationToken ct);
Task<PresentationDto> UpdatePresentationPhotosAsync(int presentationId, List<IFormFile> photos, CancellationToken ct);
```

#### IPhotoSlideService  
```csharp
Task<PhotoSlideDto> CreatePlaceholderPhotoSlideAsync(PhotoSlide placeholderPhoto, CancellationToken ct);
Task<PhotoSlideDto> UpdatePhotoSlideFileAsync(int id, IFormFile photoFile, CancellationToken ct);
```

#### IPresentationPageService
```csharp
Task<PresentationPage> CreatePageDirectAsync(PresentationPage presentationPage, CancellationToken ct);
```

### 6. Validation

#### CreatePresentationWithPositionsDto Validator
- Validates all text content properties (Title, Author, Plan)
- Validates position ranges (Left: 0-33.867, Top: 0-19.05, etc.)
- Validates photo position count matches business rules
- Validates PostTexts array is not empty
- Validates PageSettings don't exceed PageCount

#### UpdatePresentationPhotosDto Validator
- Validates at least one photo is provided
- Validates file formats (JPEG, PNG, GIF, BMP, WebP)
- Validates file size limit (10MB per file)

### 7. Database Schema
- `PresentationPages` table now has `WithPhoto` boolean column
- Migration applied successfully
- Existing data preserved

## API Usage Examples

### 1. Create Presentation (Step 1)
```http
POST /api/Presentation
Content-Type: application/json

{
  "TitleText": "My Presentation",
  "TitleFont": "Arial",
  "TitleSize": 24,
  "TitleLeft": 5.0,
  "TitleTop": 2.0,
  "TitleWidth": 20.0,
  "TitleHeight": 3.0,
  
  "AuthorText": "John Doe",
  "AuthorFont": "Arial", 
  "AuthorSize": 18,
  "AuthorLeft": 5.0,
  "AuthorTop": 6.0,
  "AuthorWidth": 15.0,
  "AuthorHeight": 2.0,
  
  "PlanText": "Introduction to the topic",
  "PlanFont": "Arial",
  "PlanSize": 14,
  "PlanLeft": 5.0,
  "PlanTop": 9.0,
  "PlanWidth": 25.0,
  "PlanHeight": 5.0,
  
  "DesignId": 1,
  "PageCount": 10,
  "WithPhoto": true,
  "IsActive": true,
  "FilePath": "",
  
  "PhotoPositions": [
    {"Left": 2.0, "Top": 3.0, "Width": 10.0, "Height": 8.0},
    {"Left": 15.0, "Top": 3.0, "Width": 10.0, "Height": 8.0},
    {"Left": 2.0, "Top": 12.0, "Width": 10.0, "Height": 8.0},
    {"Left": 15.0, "Top": 12.0, "Width": 10.0, "Height": 8.0}
  ],
  
  "PageSettings": [
    {"PageNumber": 3, "WithPhoto": true},
    {"PageNumber": 4, "WithPhoto": true},
    {"PageNumber": 7, "WithPhoto": true},
    {"PageNumber": 8, "WithPhoto": true}
  ],
  
  "PostTexts": [
    {
      "Text": "First slide content",
      "Font": "Arial",
      "Size": 16,
      "Left": 1.0,
      "Top": 1.0,
      "Width": 30.0,
      "Height": 4.0
    }
    // ... more post texts
  ]
}
```

### 2. Upload Photos (Step 2)
```http
PUT /api/Presentation/1/photos
Content-Type: multipart/form-data

Photos[0]: image1.jpg
Photos[1]: image2.png  
Photos[2]: image3.jpg
Photos[3]: image4.png
```

## Benefits of the Separated Workflow

### 1. **Clear Separation of Concerns**
- Text content handled separately from file uploads
- Better error handling for each step
- Easier to retry individual operations

### 2. **Better Performance**
- Create presentation quickly without waiting for file uploads
- Upload photos separately when ready
- Smaller JSON payloads for text content

### 3. **Improved User Experience**
- Users can create presentations immediately
- Photos can be uploaded later or in batches
- Better progress tracking for large file uploads

### 4. **Validation Clarity**
- Separate validation for text content and photos
- Clear error messages for each workflow step
- Position validation happens before file upload

### 5. **Flexibility**
- Users can update photos independently
- Different pages can have different photo requirements
- Easy to extend for additional file types later

## Error Scenarios and Handling

### Creation Phase Errors
- Invalid DesignId → 400 Bad Request
- Wrong photo position count → 400 Bad Request  
- Invalid text content → 400 Bad Request with validation details

### Photo Upload Phase Errors
- Presentation not found → 404 Not Found
- Presentation has WithPhoto=false → 400 Bad Request
- Wrong photo count → 400 Bad Request
- Invalid file formats → 400 Bad Request
- File size too large → 400 Bad Request

## Database State Flow

1. **After CreatePresentation**:
   - PresentationIsroilov created
   - PresentationPages created with WithPhoto flags
   - PhotoSlides created as placeholders
   - TextSlides created with actual content

2. **After UpdatePresentationPhotos**:
   - PhotoSlides updated with real file data
   - Placeholder data replaced with actual file info
   - Files stored in `wwwroot/uploads/presentation-files/`

## Security Considerations
- File type validation prevents malicious uploads
- File size limits prevent DoS attacks
- Authorization can be added to both endpoints independently
- Placeholder PhotoSlides can't be accessed until real files uploaded

This implementation fully satisfies your requirements for separating presentation creation from photo uploads while maintaining data consistency and providing clear workflow steps.