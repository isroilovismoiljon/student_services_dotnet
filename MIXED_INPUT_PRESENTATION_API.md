# Mixed Input Presentation API

This document describes the refactored presentation creation endpoint that supports mixed input: JSON data for text fields and file uploads for photos.

## Overview

The presentation creation endpoint has been refactored to handle:
- **JSON data**: Title, Author, PlanData, DesignId, PageCount, PostTexts, etc.
- **File uploads**: Photos as multipart/form-data

## API Endpoint

### POST `/api/Presentation`

**Content-Type**: `multipart/form-data`

**Parameters**:
- `jsonData` (form field): JSON string containing text-based presentation data
- `photos` (form files): Array of uploaded photo files

## Request Format

### JSON Data Structure (jsonData field)

```json
{
  "title": {
    "text": "Presentation Title",
    "font": "Arial",
    "size": 24,
    "top": 2.5,
    "left": 5.0,
    "width": 25.0,
    "height": 3.0
  },
  "author": {
    "text": "Author Name",
    "font": "Arial",
    "size": 18,
    "top": 15.0,
    "left": 5.0,
    "width": 20.0,
    "height": 2.0
  },
  "planData": {
    "title": {
      "text": "Plan Title",
      "font": "Arial",
      "size": 20,
      "top": 1.0,
      "left": 2.0,
      "width": 20.0,
      "height": 2.0
    },
    "planTexts": [
      {
        "text": "Plan item 1",
        "font": "Arial",
        "size": 16,
        "top": 4.0,
        "left": 2.0,
        "width": 25.0,
        "height": 1.5
      }
    ]
  },
  "designId": 1,
  "pageCount": 6,
  "withPhoto": true,
  "postTexts": [
    {
      "text": "Post content 1",
      "font": "Arial",
      "size": 14,
      "top": 8.0,
      "left": 2.0,
      "width": 28.0,
      "height": 4.0
    }
  ],
  "isActive": true,
  "filePath": "",
  "photoPositions": [
    {
      "left": 0.0,
      "top": 0.0,
      "width": 15.0,
      "height": 10.0
    },
    {
      "left": 18.0,
      "top": 0.0,
      "width": 15.0,
      "height": 10.0
    }
  ]
}
```

### Photo Files

- Upload photos as `multipart/form-data` with field name `photos`
- Each photo file corresponds to a position in the `photoPositions` array

## Validation Rules

### Photo Count Validation
- When `withPhoto` is `true`: Number of photos must equal `(pageCount - 2) / 2`
- When `withPhoto` is `false`: No photos should be uploaded

### Photo Position Constraints
- `left` + `width` ≤ 33.867 cm (slide width)
- `top` + `height` ≤ 19.05 cm (slide height) 
- All position values must be non-negative
- Width and height must be positive

### Text Fields
- Title and Author are required
- PlanData is required
- At least one PostText is required
- PageCount must be between 1 and 100

## Example Frontend Usage (JavaScript)

```javascript
// Prepare JSON data
const jsonData = {
  title: {
    text: "My Presentation",
    font: "Arial",
    size: 24,
    top: 2.5,
    left: 5.0,
    width: 25.0,
    height: 3.0
  },
  // ... other fields
  withPhoto: true,
  pageCount: 6,
  photoPositions: [
    { left: 0, top: 0, width: 15, height: 10 },
    { left: 18, top: 0, width: 15, height: 10 }
  ]
};

// Prepare form data
const formData = new FormData();
formData.append('jsonData', JSON.stringify(jsonData));

// Add photo files
const photoFiles = document.querySelector('#photos').files;
for (let i = 0; i < photoFiles.length; i++) {
  formData.append('photos', photoFiles[i]);
}

// Submit request
fetch('/api/Presentation', {
  method: 'POST',
  body: formData
})
.then(response => response.json())
.then(data => console.log(data));
```

## Example cURL Request

```bash
curl -X POST "http://localhost:5074/api/Presentation" \
  -H "Content-Type: multipart/form-data" \
  -F "jsonData={\"title\":{\"text\":\"Test\",\"font\":\"Arial\",\"size\":24,\"top\":2.5,\"left\":5.0,\"width\":25.0},\"author\":{\"text\":\"Author\",\"font\":\"Arial\",\"size\":18,\"top\":15.0,\"left\":5.0,\"width\":20.0},\"planData\":{\"title\":{\"text\":\"Plan\",\"font\":\"Arial\",\"size\":20,\"top\":1.0,\"left\":2.0,\"width\":20.0},\"planTexts\":[{\"text\":\"Item 1\",\"font\":\"Arial\",\"size\":16,\"top\":4.0,\"left\":2.0,\"width\":25.0}]},\"designId\":1,\"pageCount\":6,\"withPhoto\":true,\"postTexts\":[{\"text\":\"Post 1\",\"font\":\"Arial\",\"size\":14,\"top\":8.0,\"left\":2.0,\"width\":28.0}],\"isActive\":true,\"filePath\":\"\",\"photoPositions\":[{\"left\":0,\"top\":0,\"width\":15,\"height\":10},{\"left\":18,\"top\":0,\"width\":15,\"height\":10}]}" \
  -F "photos=@image1.jpg" \
  -F "photos=@image2.jpg"
```

## Response Format

```json
{
  "success": true,
  "message": "Presentation created successfully",
  "data": {
    "id": 1,
    "title": {
      "id": 1,
      "text": "Presentation Title",
      "font": "Arial",
      "size": 24
    },
    "withPhoto": true,
    "pageCount": 6,
    "isActive": true,
    "createdAt": "2024-10-14T08:30:00Z",
    "pages": []
  },
  "timestamp": "2024-10-14T08:30:00Z"
}
```

## Error Responses

### Invalid JSON Format
```json
{
  "success": false,
  "message": "Invalid JSON data format",
  "timestamp": "2024-10-14T08:30:00Z"
}
```

### Photo Count Mismatch
```json
{
  "success": false,
  "message": "With WithPhoto=true and PageCount=6, exactly 2 photos are required, but 1 were provided.",
  "timestamp": "2024-10-14T08:30:00Z"
}
```

### Validation Errors
```json
{
  "success": false,
  "message": "Invalid input data",
  "errors": {
    "PhotoPositions[0]": ["The sum of Left position (Left) and Width must not exceed 33.867 cm"]
  },
  "timestamp": "2024-10-14T08:30:00Z"
}
```

## Implementation Details

### DTOs
- **CreatePresentationDataDto**: Contains all text-based fields and photo position data
- **PhotoPositionDto**: Contains position and size data for photos (without files)
- **CreatePresentationRequestDto**: Combines data DTO with file uploads (used internally)

### Validators
- **CreatePresentationDataDtoValidator**: Validates text fields and photo position requirements
- **PhotoPositionDtoValidator**: Validates slide boundary constraints (33.867cm × 19.05cm)

### Controller Logic
1. Receives `jsonData` string and `photos` files from form
2. Deserializes JSON data to `CreatePresentationDataDto`
3. Validates photo count based on `withPhoto` and `pageCount`
4. Maps data and files to original `CreatePresentationDto` for service layer
5. Passes combined DTO to `PresentationService.CreatePresentationAsync()`

This approach provides a clean separation between:
- **Frontend**: Can send JSON data + files naturally using FormData
- **Service Layer**: Continues to work with the existing `CreatePresentationDto` structure
- **Validation**: Enforces business rules for both text data and photo constraints