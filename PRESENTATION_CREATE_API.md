# PresentationIsroilov Create API - Complete Specification

## Endpoint
**POST** `/api/PresentationIsroilov`

## Request Structure

The API accepts the exact JSON structure you provided. Here's the complete specification:

### Root Object: `CreatePresentationIsroilovDto`

```json
{
  "title": { ... },           // CreateTextSlideDto - REQUIRED
  "author": { ... },          // CreateTextSlideDto - REQUIRED
  "withPhoto": false,         // boolean - REQUIRED
  "pageCount": 5,            // int - REQUIRED (minimum 5)
  "designId": 1,             // int - REQUIRED
  "plan": { ... },           // CreatePlanDto - REQUIRED
  "presentationPages": [ ... ] // Array - REQUIRED (must have exactly pageCount items)
}
```

### TextSlide Object (for title, author, and all post title/text)

**Used in:** `title`, `author`, `plan.planText`, `plan.plans`, `presentationPages[].presentationPosts[].title`, `presentationPages[].presentationPosts[].text`

```json
{
  "text": "Content here",      // string - REQUIRED
  "size": 16,                 // int (1-200) - REQUIRED
  "font": "Arial",            // string - REQUIRED
  "isBold": false,            // boolean - REQUIRED
  "isItalic": false,          // boolean - REQUIRED
  "colorHex": "#000000",      // string (hex format) - REQUIRED
  "left": 2.0,                // double (0-33.867) - REQUIRED
  "top": 3.0,                 // double (0-19.05) - REQUIRED
  "width": 20.0,              // double (>0, ≤33.867) - REQUIRED
  "height": 2.0,              // double (>0, ≤19.05) - OPTIONAL
  "horizontal": 1,            // enum - OPTIONAL
  "vertical": 1               // enum - OPTIONAL
}
```

### Plan Object

```json
{
  "planText": { ... },   // CreateTextSlideDto - REQUIRED
  "plans": { ... }       // CreateTextSlideDto - REQUIRED
}
```

### Presentation Pages Array

```json
"presentationPages": [
  {
    "presentationPosts": [         // Array - REQUIRED (at least 1 post per page)
      {
        "title": { ... },         // CreateTextSlideDto - OPTIONAL
        "text": { ... }           // CreateTextSlideDto - REQUIRED
      }
    ]
  }
]
```

## Validation Rules

### ✅ **Required Fields**
- `title` (CreateTextSlideDto)
- `author` (CreateTextSlideDto)
- `pageCount` (int, ≥5)
- `designId` (int)
- `plan.planText` (CreateTextSlideDto)
- `plan.plans` (CreateTextSlideDto)
- `presentationPages` (Array, length must equal `pageCount`)
- Each page must have `presentationPosts` array
- Each post must have `text` (CreateTextSlideDto)

### 🚫 **Removed Fields (Auto-Assigned)**
- `photoId` - Automatically `null` on creation (set later via UpdatePhotos endpoint)
- `backgroundPhotoId` - Automatically assigned from Design photos

### ✅ **TextSlide Validation**
- `text`: Required, max 5000 characters
- `size`: 1-200
- `font`: Required, max 100 characters
- `colorHex`: Must be valid hex (#000000 or #000)
- `left`: 0 to 33.867 cm (slide width)
- `top`: 0 to 19.05 cm (slide height)
- `width`: > 0 and ≤ 33.867 cm
- `height`: > 0 and ≤ 19.05 cm (if provided)
- **`left + width` must not exceed 33.867 cm**
- **`top + height` must not exceed 19.05 cm**

### ✅ **Array Validation**
- `presentationPages.length` must equal `pageCount`
- Each page must have at least 1 post in `presentationPosts`

## Complete Example Request

```json
{
  "title": {
    "text": "My Presentation",
    "size": 32,
    "font": "Arial",
    "isBold": true,
    "isItalic": false,
    "colorHex": "#000000",
    "left": 1.0,
    "top": 1.0,
    "width": 20.0
  },
  "author": {
    "text": "Author Name",
    "size": 18,
    "font": "Arial",
    "isBold": false,
    "isItalic": false,
    "colorHex": "#666666",
    "left": 1.0,
    "top": 5.0,
    "width": 20.0
  },
  "withPhoto": false,
  "pageCount": 5,
  "designId": 1,
  "plan": {
    "planText": {
      "text": "Plan",
      "font": "Arial",
      "size": 20,
      "colorHex": "#000000",
      "isBold": true,
      "isItalic": false,
      "left": 2.0,
      "top": 3.0,
      "width": 20.0,
      "height": 2.0
    },
    "plans": {
      "text": "1. Introduction\n2. Content\n3. Conclusion",
      "font": "Arial",
      "size": 16,
      "colorHex": "#333333",
      "isBold": false,
      "isItalic": false,
      "left": 3.0,
      "top": 8.0,
      "width": 20.0,
      "height": 9.0
    }
  },
  "presentationPages": [
    {
      "presentationPosts": [
        {
          "title": {
            "text": "Page 1 Title",
            "size": 16,
            "font": "Arial",
            "isBold": false,
            "isItalic": false,
            "colorHex": "#000000",
            "left": 2.0,
            "top": 3.0,
            "width": 28.0
          },
          "text": {
            "text": "Page 1 content",
            "size": 16,
            "font": "Arial",
            "isBold": false,
            "isItalic": false,
            "colorHex": "#000000",
            "left": 2.0,
            "top": 6.0,
            "width": 20.0
          }
        }
      ]
    },
    {
      "presentationPosts": [
        {
          "title": {
            "text": "Page 2 Title",
            "size": 16,
            "font": "Arial",
            "isBold": false,
            "isItalic": false,
            "colorHex": "#000000",
            "left": 2.0,
            "top": 3.0,
            "width": 28.0
          },
          "text": {
            "text": "Page 2 content",
            "size": 16,
            "font": "Arial",
            "isBold": false,
            "isItalic": false,
            "colorHex": "#000000",
            "left": 2.0,
            "top": 6.0,
            "width": 20.0
          }
        }
      ]
    },
    {
      "presentationPosts": [
        {
          "title": {
            "text": "Page 3 Title",
            "size": 16,
            "font": "Arial",
            "isBold": false,
            "isItalic": false,
            "colorHex": "#000000",
            "left": 2.0,
            "top": 3.0,
            "width": 28.0
          },
          "text": {
            "text": "Page 3 content",
            "size": 16,
            "font": "Arial",
            "isBold": false,
            "isItalic": false,
            "colorHex": "#000000",
            "left": 2.0,
            "top": 6.0,
            "width": 20.0
          }
        }
      ]
    },
    {
      "presentationPosts": [
        {
          "title": {
            "text": "Page 4 Title",
            "size": 16,
            "font": "Arial",
            "isBold": false,
            "isItalic": false,
            "colorHex": "#000000",
            "left": 2.0,
            "top": 3.0,
            "width": 28.0
          },
          "text": {
            "text": "Page 4 content",
            "size": 16,
            "font": "Arial",
            "isBold": false,
            "isItalic": false,
            "colorHex": "#000000",
            "left": 2.0,
            "top": 6.0,
            "width": 20.0
          }
        }
      ]
    },
    {
      "presentationPosts": [
        {
          "title": {
            "text": "Page 5 Title",
            "size": 16,
            "font": "Arial",
            "isBold": false,
            "isItalic": false,
            "colorHex": "#000000",
            "left": 2.0,
            "top": 3.0,
            "width": 28.0
          },
          "text": {
            "text": "Page 5 content",
            "size": 16,
            "font": "Arial",
            "isBold": false,
            "isItalic": false,
            "colorHex": "#000000",
            "left": 2.0,
            "top": 6.0,
            "width": 20.0
          }
        }
      ]
    }
  ]
}
```

## Success Response (201 Created)

```json
{
  "success": true,
  "data": {
    "id": 1,
    "titleId": 123,
    "authorId": 124,
    "withPhoto": false,
    "pageCount": 5,
    "designId": 1,
    "planId": 456,
    "isActive": true,
    "filePath": "",
    "createdAt": "2025-10-29T06:00:00Z",
    "updatedAt": "2025-10-29T06:00:00Z",
    "presentationPages": [ ... ],
    "title": { ... },
    "author": { ... },
    "plan": { ... },
    "design": { ... }
  },
  "timestamp": "2025-10-29T06:00:00Z"
}
```

## Error Responses

### 400 Bad Request - Validation Error
```json
{
  "success": false,
  "message": "Text element exceeds slide boundaries. Left + Width must not exceed 33.867 cm",
  "timestamp": "2025-10-29T06:00:00Z"
}
```

### 400 Bad Request - Page Count Mismatch
```json
{
  "success": false,
  "message": "Number of presentation pages (3) must match PageCount (5)",
  "timestamp": "2025-10-29T06:00:00Z"
}
```

## Key Features

✅ Full control over text styling (font, size, color, position, bold, italic)
✅ **Automatic background photo assignment** from design (no need to specify)
✅ **Simplified request** - no photoId or backgroundPhotoId needed
✅ Proper validation prevents elements from exceeding slide boundaries
✅ Supports multiple posts per page
✅ Title is optional for posts, text is required
✅ User photos can be added later via `/api/PresentationIsroilov/{id}/photos` endpoint

## Implementation Status

✅ DTOs configured correctly
✅ Service layer validates and creates all entities
✅ Controller handles requests properly
✅ Validation rules enforced
✅ Database relationships maintained

**The API is ready to accept your exact JSON structure!**
