# PresentationIsroilov Implementation

## Overview
Complete CRUD implementation for PresentationIsroilov with automatic page generation and photo management.

## Files Created

### DTOs
- `StudentServicesWebApi.Application/DTOs/PresentationIsroilov/CreatePresentationIsroilovDto.cs`
- `StudentServicesWebApi.Application/DTOs/PresentationIsroilov/PresentationIsroilovDto.cs`
- `StudentServicesWebApi.Application/DTOs/PresentationIsroilov/UpdatePresentationIsroilovPhotosDto.cs`

### Validators
- `StudentServicesWebApi.Application/Validators/PresentationIsroilov/CreatePresentationIsroilovDtoValidator.cs`

### Interfaces
- `StudentServicesWebApi.Domain/Interfaces/IPresentationIsroilovRepository.cs`
- `StudentServicesWebApi.Application/Interfaces/IPresentationIsroilovService.cs`

### Implementations
- `StudentServicesWebApi.Infrastructure/Repositories/PresentationIsroilovRepository.cs`
- `StudentServicesWebApi.Infrastructure/Services/PresentationIsroilovService.cs`

### Controller
- `StudentServicesWebApi/Controllers/PresentationIsroilovController.cs`

## Features

### Create Presentation (POST /api/PresentationIsroilov)

**Required Fields:**
- `Title` (CreateTextSlideDto) - Title slide configuration
- `Author` (CreateTextSlideDto) - Author slide configuration  
- `WithPhoto` (bool) - Whether presentation includes user photos
- `PageCount` (int) - Number of pages (minimum 5)
- `DesignId` (int) - Design template to use
- `Plan` (CreatePlanDto) - Plan configuration

**Behavior:**
1. Creates Title and Author text slides
2. Creates Plan from provided data
3. Creates PresentationIsroilov record
4. Auto-generates PresentationPages based on rules:
   - **Pages 1-2**: Always without user photos
   - **Page 3**: With photo (if WithPhoto=true)
   - **Pages 4-5**: Without user photos
   - **Page 6**: With photo (if WithPhoto=true)
   - **Pages 7-8**: Without user photos
   - **Pattern continues**: Every 3rd page after page 2 has photos
5. Sets BackgroundPhoto from Design.Photos (cycling through available backgrounds)
6. All user photos (PhotoId) are initially null

### Update Photos (PUT /api/PresentationIsroilov/{id}/photos)

**Endpoint:** `/api/PresentationIsroilov/{id}/photos`  
**Content-Type:** `multipart/form-data`  
**Parameters:**
- `Photos` (List<IFormFile>) - Photo files for pages with WithPhoto=true

**Behavior:**
1. Validates presentation exists
2. Checks WithPhoto=true (cannot update if false)
3. Gets all pages where WithPhoto=true
4. Requires exact number of photos matching pages with photos
5. Creates/updates PhotoSlide for each page
6. Cannot update if presentation was created with WithPhoto=false

### Get All Presentations (GET /api/PresentationIsroilov)

Returns list of all presentations with full details including:
- Title, Author slides
- Plan details
- Design information
- All PresentationPages with photos

### Get By ID (GET /api/PresentationIsroilov/{id})

Returns single presentation with complete details.

### Delete (DELETE /api/PresentationIsroilov/{id})

Soft-deletes the presentation.

## Page Photo Pattern

For a presentation with `WithPhoto=true` and `PageCount=10`:

| Page | Has User Photo | BackgroundPhoto |
|------|----------------|-----------------|
| 1    | ❌ No          | ✅ Yes (Design) |
| 2    | ❌ No          | ✅ Yes (Design) |
| 3    | ✅ Yes         | ✅ Yes (Design) |
| 4    | ❌ No          | ✅ Yes (Design) |
| 5    | ❌ No          | ✅ Yes (Design) |
| 6    | ✅ Yes         | ✅ Yes (Design) |
| 7    | ❌ No          | ✅ Yes (Design) |
| 8    | ❌ No          | ✅ Yes (Design) |
| 9    | ✅ Yes         | ✅ Yes (Design) |
| 10   | ❌ No          | ✅ Yes (Design) |

For `WithPhoto=false`, all pages have no user photos.

## Validation Rules

- **PageCount**: Minimum 5 pages
- **Title**: Required
- **Author**: Required
- **DesignId**: Must be > 0 and exist in database
- **Plan**: Required
- **Photos Update**: 
  - Only allowed if WithPhoto=true
  - Must provide exact number of photos for pages with photos
  - Cannot update after initial upload (design decision)

## API Examples

### Create Presentation

```json
POST /api/PresentationIsroilov
{
  "title": {
    "text": "My Presentation",
    "font": "Arial",
    "size": 24,
    "colorHex": "#000000",
    "left": 10,
    "top": 10,
    "width": 80,
    "height": 10
  },
  "author": {
    "text": "John Doe",
    "font": "Arial",
    "size": 18,
    "colorHex": "#666666",
    "left": 10,
    "top": 25,
    "width": 80,
    "height": 8
  },
  "withPhoto": true,
  "pageCount": 8,
  "designId": 1,
  "plan": {
    "planText": {
      "text": "Plan",
      "font": "Arial",
      "size": 20,
      "colorHex": "#000000",
      "left": 10,
      "top": 10,
      "width": 80,
      "height": 10
    },
    "plans": {
      "text": "1. Introduction\n2. Main Content\n3. Conclusion",
      "font": "Arial",
      "size": 16,
      "colorHex": "#333333",
      "left": 10,
      "top": 22,
      "width": 80,
      "height": 60
    }
  }
}
```

### Update Photos

```
PUT /api/PresentationIsroilov/1/photos
Content-Type: multipart/form-data

Photos: [file1.jpg, file2.jpg, file3.jpg]
```

For a presentation with 8 pages and WithPhoto=true, you need 3 photos (for pages 3, 6, and 9 if it existed).

## Dependencies

- TextSlideService - Creates title and author slides
- PlanService - Creates plan
- DesignService - Validates design and provides backgrounds
- PhotoSlideService - Manages photo uploads
- PresentationPageRepository - Creates and updates pages
- DtoMappingService - Maps entities to DTOs

## Database Impact

- Uses existing `PresentationIsroilov` table (model not changed)
- Uses existing `PresentationPage` table with `PresentationIsroilovId` foreign key
- No migrations needed - works with current schema
