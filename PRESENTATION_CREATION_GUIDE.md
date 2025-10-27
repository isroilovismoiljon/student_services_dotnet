# 🎨 Advanced Presentation Creation API

## Overview

The presentation creation system has been enhanced with sophisticated logic that automatically handles background images, photo uploads, text slides, and page generation based on user requirements.

## 🔧 New Model Properties

### PresentationIsroilov Model
```csharp
public class PresentationIsroilov : BaseEntity
{
    public int TitleId { get; set; }
    public TextSlide Title { get; set; }
    
    public int AuthorId { get; set; }
    public TextSlide Author { get; set; }
    
    public bool WithPhoto { get; set; } = false;        // NEW
    public int PageCount { get; set; } = 10;           // NEW
    
    public int DesignId { get; set; }
    public Design Design { get; set; }
    
    public int PlanId { get; set; }
    public Plan Plan { get; set; }
}
```

## 📋 API Usage

### Endpoint
`POST /api/presentation`

### Content-Type
`multipart/form-data` (for file uploads)

### Request Structure

```typescript
interface CreatePresentationRequest {
  // Title and Author as TextSlide objects with positioning
  title: {
    text: string;
    size: number;
    font: string;
    colorHex: string;
    left: number;
    top: number;
    width: number;
    height?: number;
    horizontal: HorizontalAlignment;
    vertical: VerticalAlignment;
  };
  
  author: {
    text: string;
    size: number;
    font: string;
    colorHex: string;
    left: number;
    top: number;
    width: number;
    height?: number;
    horizontal: HorizontalAlignment;
    vertical: VerticalAlignment;
  };
  
  // Plan data with TextSlide objects
  planData: {
    planText: TextSlideData;
    plans: TextSlideData;
  };
  
  designId: number;          // Must exist in database
  pageCount: number;         // Between 1-100, default 10
  withPhoto: boolean;        // Default false
  
  // Photos (required if withPhoto = true)
  photos: File[];           // Must be exactly (pageCount - 2) / 2 files
  
  // Post texts for presentation content
  postTexts: TextSlideData[];  // At least one required
  
  isActive: boolean;         // Default true
  filePath?: string;         // Optional
}
```

## ⚙️ Creation Logic

### 1. Photo Validation
```
If WithPhoto = true:
  Required photos = (PageCount - 2) / 2
  
Examples:
- PageCount = 10 → Required photos = 4
- PageCount = 8  → Required photos = 3
- PageCount = 6  → Required photos = 2
```

### 2. Background Assignment
```
Page 1 → Design.Photos[0]
Page 2 → Design.Photos[1]
Page 3 → Design.Photos[0] (cycles back)
...
```

### 3. User Photo Distribution
```
If WithPhoto = true:
  User photos are distributed evenly across pages
  Each photo gets positioning data from user input
```

### 4. Automatic Entity Creation

The system automatically creates:

1. **2 TextSlide entities** (Title + Author)
2. **1 Plan entity** with 2 TextSlide entities (PlanText + Plans)
3. **N PhotoSlide entities** (if WithPhoto = true)
4. **M TextSlide entities** (from postTexts array)
5. **PageCount PresentationPage entities**
6. **Up to 3 PresentationPost entities per page**

## 📊 Example Requests

### Simple Presentation (No Photos)
```json
{
  "title": {
    "text": "AI Revolution",
    "size": 32,
    "font": "Arial",
    "colorHex": "#000000",
    "left": 50,
    "top": 20,
    "width": 200,
    "height": 40,
    "horizontal": "Center",
    "vertical": "Top"
  },
  "author": {
    "text": "John Doe",
    "size": 18,
    "font": "Arial",
    "colorHex": "#666666",
    "left": 50,
    "top": 70,
    "width": 150,
    "height": 25,
    "horizontal": "Center",
    "vertical": "Top"
  },
  "planData": {
    "planText": {
      "text": "Presentation Plan",
      "size": 24,
      "font": "Arial",
      "colorHex": "#333333",
      "left": 10,
      "top": 10,
      "width": 300,
      "height": 30
    },
    "plans": {
      "text": "1. Introduction\n2. Main Content\n3. Conclusion",
      "size": 16,
      "font": "Arial",
      "colorHex": "#333333",
      "left": 10,
      "top": 50,
      "width": 300,
      "height": 100
    }
  },
  "designId": 1,
  "pageCount": 6,
  "withPhoto": false,
  "postTexts": [
    {
      "text": "Welcome to our presentation",
      "size": 20,
      "font": "Arial",
      "colorHex": "#000000",
      "left": 20,
      "top": 100,
      "width": 250,
      "height": 30
    },
    {
      "text": "Key benefits include...",
      "size": 18,
      "font": "Arial", 
      "colorHex": "#000000",
      "left": 20,
      "top": 140,
      "width": 250,
      "height": 50
    }
  ]
}
```

### Advanced Presentation (With Photos)
```
POST /api/presentation
Content-Type: multipart/form-data

Fields:
- title: (TextSlide JSON)
- author: (TextSlide JSON)  
- planData: (Plan JSON)
- designId: 1
- pageCount: 8
- withPhoto: true
- postTexts: (TextSlide array JSON)

Files:
- photos[0]: image1.jpg
- photos[1]: image2.jpg  
- photos[2]: image3.jpg
(Exactly 3 photos for pageCount=8)
```

## 🛡️ Validation Rules

1. **Photo Count Validation**
   ```
   If withPhoto = true: photos.length MUST equal (pageCount - 2) / 2
   If withPhoto = false: photos.length MUST equal 0
   ```

2. **Design Validation**
   ```
   designId must reference an existing Design entity
   ```

3. **Content Validation**
   ```
   - At least 1 postText is required
   - All TextSlide objects must have valid positioning data
   - PageCount must be between 1-100
   ```

## 📝 Response Format

### Success Response
```json
{
  "success": true,
  "message": "Presentation created successfully",
  "data": {
    "id": 123,
    "title": { /* TextSlide object */ },
    "author": { /* TextSlide object */ },
    "withPhoto": true,
    "pageCount": 8,
    "designId": 1,
    "planId": 45,
    "pages": [
      { "id": 1, "postCount": 3 },
      { "id": 2, "postCount": 2 }
    ],
    "createdAt": "2024-10-12T08:45:00Z"
  },
  "timestamp": "2024-10-12T08:45:00Z"
}
```

### Error Response
```json
{
  "success": false,
  "message": "With WithPhoto=true and PageCount=8, exactly 3 photos are required, but 2 were provided.",
  "timestamp": "2024-10-12T08:45:00Z"
}
```

## 🔄 Migration Details

The database migration `AddWithPhotoAndPageCountToPresentationIsroilov` adds:
- `WithPhoto` boolean field (default: false)
- `PageCount` integer field (default: 10)
- Updates to `PresentationPage` nullable background photo relationships

## 🏗️ Architecture Benefits

1. **Single API Call** - Complete presentation creation in one request
2. **Automatic Background Management** - No manual background assignment needed
3. **Flexible Photo Integration** - User controls photo positioning and distribution
4. **Rich Text Formatting** - Full TextSlide capabilities for all text elements
5. **Scalable Design** - Supports 1-100 pages with automatic content distribution
6. **Type Safety** - Strong validation and error handling throughout the pipeline

This implementation provides a powerful, user-friendly API for creating complex presentations with minimal client-side logic while maintaining full control over content positioning and styling.