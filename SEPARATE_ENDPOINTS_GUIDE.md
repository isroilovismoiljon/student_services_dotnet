# Separate Endpoints Implementation Guide

This document explains the new approach with separate endpoints for presentation creation and photo uploads.

## ✅ **New Architecture**

### **Two Separate Endpoints:**

1. **`POST /api/presentation`** - Creates presentation (JSON only, no photos)
2. **`PUT /api/presentation/{id}/photos`** - Updates photos for existing presentation

## 🎯 **Endpoint Details**

### **1. CreatePresentation Endpoint**

```http
POST /api/presentation
Content-Type: application/json
```

**Uses:** `CreatePresentationJsonDto` with `[FromBody]`

**Features:**
- ✅ **Pure JSON**: No multipart/form-data, clean JSON request
- ✅ **All properties as individual fields** in Swagger
- ✅ **Photo positions as array**: `PhotoPositions: [{"left": 5, "top": 5, "width": 100, "height": 50}]`
- ✅ **Post texts as array**: `PostTexts: [{"text": "Post 1", "font": "Arial", ...}]`
- ✅ **No photo files**: Creates presentation structure without photos

**Swagger Display:**
```
TitleText: (text input)
TitleFont: (text input)
TitleSize: (number input, 1-100)
TitleLeft: (number input, 0-33.867)
...
PhotoPositions: (array of PhotoPositionDto objects)
PostTexts: (array of CreateTextSlideDto objects)
```

### **2. UpdatePhotos Endpoint**

```http
PUT /api/presentation/{id}/photos
Content-Type: multipart/form-data
```

**Uses:** `UpdatePhotosDto` with `[FromForm]`

**Features:**
- ✅ **Form data only**: For photo uploads with positions
- ✅ **Individual position fields**: Photo1Left, Photo2Top, etc.
- ✅ **File uploads**: Photos array for multiple files
- ✅ **Validation against existing presentation**: Checks WithPhoto flag and PageCount

**Swagger Display:**
```
Photos: (file upload array)
Photo1Left: (number input, 0-33.867)
Photo1Top: (number input, 0-19.05)
Photo1Width: (number input, 1-33.867)
Photo1Height: (number input, 1-19.05, optional)
... (Photo 2-5 positions)
```

## 📋 **Usage Examples**

### **Step 1: Create Presentation (JSON)**

```http
POST /api/presentation
Content-Type: application/json

{
  "titleText": "My Presentation",
  "titleFont": "Arial",
  "titleSize": 24,
  "titleLeft": 2.0,
  "titleTop": 1.0,
  "titleWidth": 10.0,
  "titleHeight": 2.0,
  "authorText": "John Doe",
  "authorFont": "Arial",
  "authorSize": 18,
  "authorLeft": 2.0,
  "authorTop": 4.0,
  "authorWidth": 8.0,
  "authorHeight": 1.5,
  "planText": "Presentation Plan",
  "planFont": "Arial",
  "planSize": 14,
  "designId": 1,
  "pageCount": 10,
  "withPhoto": true,
  "isActive": true,
  "filePath": "",
  "photoPositions": [
    {
      "left": 5.0,
      "top": 5.0,
      "width": 100.0,
      "height": 50.0
    },
    {
      "left": 110.0,
      "top": 5.0,
      "width": 100.0,
      "height": 50.0
    }
  ],
  "postTexts": [
    {
      "text": "First post",
      "font": "Arial",
      "size": 14,
      "left": 0,
      "top": 0,
      "width": 10,
      "height": 2
    }
  ]
}
```

**Response:**
```json
{
  "success": true,
  "message": "Presentation created successfully",
  "data": {
    "id": 123,
    "title": {...},
    "author": {...}
  }
}
```

### **Step 2: Upload Photos (Form Data)**

```http
PUT /api/presentation/123/photos
Content-Type: multipart/form-data

Photo1Left: 5.0
Photo1Top: 5.0
Photo1Width: 100.0
Photo1Height: 50.0
Photo2Left: 110.0
Photo2Top: 5.0
Photo2Width: 100.0
Photo2Height: 50.0
Photos: [file1.jpg, file2.jpg]
```

**Response:**
```json
{
  "success": true,
  "message": "Photos updated successfully",
  "presentationId": 123,
  "photoCount": 2
}
```

## 🔧 **Frontend Integration**

### **JavaScript Example:**

```javascript
// Step 1: Create presentation with JSON
const presentationData = {
  titleText: "My Presentation",
  titleFont: "Arial",
  titleSize: 24,
  withPhoto: true,
  pageCount: 10,
  photoPositions: [
    { left: 5, top: 5, width: 100, height: 50 },
    { left: 110, top: 5, width: 100, height: 50 }
  ],
  postTexts: [
    { text: "Post 1", font: "Arial", size: 14, left: 0, top: 0, width: 10, height: 2 }
  ]
};

// Create presentation
const createResponse = await fetch('/api/presentation', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(presentationData)
});

const result = await createResponse.json();
const presentationId = result.data.id;

// Step 2: Upload photos with positions
const formData = new FormData();
formData.append('Photo1Left', '5');
formData.append('Photo1Top', '5');
formData.append('Photo1Width', '100');
formData.append('Photo1Height', '50');
formData.append('Photo2Left', '110');
formData.append('Photo2Top', '5');
formData.append('Photo2Width', '100');
formData.append('Photo2Height', '50');
formData.append('Photos', file1);
formData.append('Photos', file2);

const photosResponse = await fetch(`/api/presentation/${presentationId}/photos`, {
  method: 'PUT',
  body: formData
});
```

## ✅ **Validation**

### **CreatePresentation Validation:**
- All text fields (Title, Author, Plan) with range validation
- `PostTexts` array with nested validation
- `PhotoPositions` array validation (when WithPhoto=true)
- No photo file validation (handled separately)

### **UpdatePhotos Validation:**
- Photo position fields (Photo1Left, Photo2Top, etc.) with range validation
- Validates against existing presentation's WithPhoto flag
- Validates photo count based on existing presentation's PageCount
- File upload validation for photo types

## 💡 **Advantages**

1. **Clean Separation**: JSON for data, form data for files
2. **Better Frontend Experience**: No mixed content types in single request
3. **Flexible Workflow**: Create presentation first, add photos later
4. **Individual Swagger Fields**: Each property shows separately
5. **Type Safety**: Strong typing for all properties
6. **Reusable**: UpdatePhotos can be called multiple times
7. **Validation**: Separate validation rules for each endpoint

## 🔄 **Workflow**

1. **Frontend creates presentation** with all text data and photo positions (JSON)
2. **Backend creates presentation** without photos, returns presentation ID
3. **Frontend uploads photos** using presentation ID and individual position fields (form data)
4. **Backend validates and updates** presentation with photos
5. **Photos are processed** and linked to the presentation

## 🎉 **Result**

- ✅ **CreatePresentation**: Clean JSON endpoint with individual Swagger fields
- ✅ **UpdatePhotos**: Separate photo upload endpoint with individual position fields  
- ✅ **Best of both worlds**: JSON for data structure, form data for file uploads
- ✅ **Proper separation**: Each endpoint has a single, clear responsibility

This approach gives you the clean separation you requested while maintaining the individual field display in Swagger UI!