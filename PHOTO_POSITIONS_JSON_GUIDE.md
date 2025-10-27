# PhotoPositions as JSON - Implementation Guide

This document explains how photo positions are now handled as JSON data instead of individual form fields.

## ✅ **Updated Implementation**

### **What Changed**
Instead of individual form fields like `Photo1Left`, `Photo2Top`, etc., photo positions are now provided as a **single JSON string field** that can be easily populated from request body data.

### **New DTO Structure**
```csharp
public class CreatePresentationMixedDto
{
    // ... (all other fields remain the same)
    
    // Photo positions as JSON string
    public string PhotoPositions { get; set; } = string.Empty; // JSON array of PhotoPositionDto objects
    
    // Post text fields - JSON string of CreateTextSlideDto array
    [Required]
    public string PostTexts { get; set; } = string.Empty;
}
```

## 🎯 **Swagger Display**

**Swagger now shows:**
- `PhotoPositions` (text input field for JSON)
- All other fields remain as individual inputs

**PhotoPositions Field:**
- **Type**: Text input
- **Required**: Only when `WithPhoto = true`
- **Format**: JSON array of PhotoPositionDto objects
- **Validation**: Must be valid JSON with proper position ranges

## 📋 **JSON Structure**

### **PhotoPositions JSON Format**
```json
[
  {
    "left": 5.0,
    "top": 5.0,
    "width": 10.0,
    "height": 8.0
  },
  {
    "left": 15.0,
    "top": 5.0,
    "width": 10.0,
    "height": 8.0
  }
]
```

### **PostTexts JSON Format**
```json
[
  {
    "text": "First post content",
    "font": "Arial",
    "size": 14,
    "left": 2.0,
    "top": 1.0,
    "width": 10.0,
    "height": 2.0
  },
  {
    "text": "Second post content",
    "font": "Georgia", 
    "size": 12,
    "left": 2.0,
    "top": 4.0,
    "width": 8.0,
    "height": 1.5
  }
]
```

## 🔧 **Frontend Integration**

### **How to Send Request**

**Option 1: FormData with JSON strings**
```javascript
const formData = new FormData();

// Add regular fields
formData.append('TitleText', 'My Presentation');
formData.append('TitleFont', 'Arial');
formData.append('TitleSize', '24');
formData.append('WithPhoto', 'true');

// Add photo positions as JSON string
const photoPositions = [
  { left: 5, top: 5, width: 100, height: 50 },
  { left: 10, top: 10, width: 90, height: 45 }
];
formData.append('PhotoPositions', JSON.stringify(photoPositions));

// Add post texts as JSON string  
const postTexts = [
  { text: "Post 1", font: "Arial", size: 14, left: 0, top: 0, width: 10, height: 2 },
  { text: "Post 2", font: "Arial", size: 12, left: 0, top: 3, width: 8, height: 1.5 }
];
formData.append('PostTexts', JSON.stringify(postTexts));

// Add photo files
formData.append('Photos', file1);
formData.append('Photos', file2);

// Send request
fetch('/api/presentation', {
  method: 'POST',
  body: formData
});
```

**Option 2: Construct JSON from separate data**
```javascript
// If you have positions from request body or separate source
const positionsFromBody = [
  { left: 5, top: 5, width: 100, height: 50 },
  { left: 10, top: 10, width: 90, height: 45 }
];

const formData = new FormData();
// ... add other fields
formData.append('PhotoPositions', JSON.stringify(positionsFromBody));
```

## ✅ **Validation Rules**

### **When WithPhoto = true:**
- `PhotoPositions` is **required**
- Must be valid JSON array
- Each position must have valid ranges:
  - `Left`: 0 ≤ left ≤ 33.867 cm
  - `Top`: 0 ≤ top ≤ 19.05 cm  
  - `Width`: 1 ≤ width ≤ 33.867 cm
  - `Height`: 1 ≤ height ≤ 19.05 cm (optional)
  - `Left + Width` ≤ 33.867 cm (within slide bounds)
  - `Top + Height` ≤ 19.05 cm (within slide bounds)

### **When WithPhoto = false:**
- `PhotoPositions` should be empty or null
- No photo validation applied

### **PostTexts (always):**
- **Required** field
- Must be valid JSON array of `CreateTextSlideDto` objects
- Each object must have valid TextSlide properties

## 🔄 **Backend Processing**

### **Controller Logic**
```csharp
// Parse photo positions from JSON string
var photoPositions = new List<PhotoPositionDto>();
if (mixedDto.WithPhoto && !string.IsNullOrWhiteSpace(mixedDto.PhotoPositions))
{
    try
    {
        photoPositions = JsonSerializer.Deserialize<List<PhotoPositionDto>>(
            mixedDto.PhotoPositions, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        ) ?? new List<PhotoPositionDto>();
    }
    catch (JsonException)
    {
        return BadRequest("Invalid PhotoPositions JSON format");
    }
}
```

### **Validation**
- FluentValidation automatically validates JSON format
- Range validation for each position object
- Boundary checking to ensure positions fit within slide dimensions

## 💡 **Advantages**

1. **Flexible**: Easy to add/remove photo positions without changing DTO structure
2. **Frontend-Friendly**: JSON can be easily constructed from request body data
3. **Clean API**: Single field instead of multiple individual fields
4. **Scalable**: Supports any number of photos without DTO changes
5. **Type-Safe**: Strong typing through `PhotoPositionDto` deserialization

## 📝 **Usage Examples**

### **Basic Usage**
```bash
curl -X POST "https://api.example.com/api/presentation" \
  -H "Content-Type: multipart/form-data" \
  -F "TitleText=My Presentation" \
  -F "WithPhoto=true" \
  -F "PhotoPositions=[{\"left\":5,\"top\":5,\"width\":100,\"height\":50}]" \
  -F "PostTexts=[{\"text\":\"Post 1\",\"font\":\"Arial\",\"size\":14,\"left\":0,\"top\":0,\"width\":10,\"height\":2}]" \
  -F "Photos=@photo1.jpg"
```

### **Multiple Photos**
```bash
curl -X POST "https://api.example.com/api/presentation" \
  -F "PhotoPositions=[{\"left\":5,\"top\":5,\"width\":50,\"height\":40},{\"left\":60,\"top\":5,\"width\":50,\"height\":40}]" \
  -F "Photos=@photo1.jpg" \
  -F "Photos=@photo2.jpg"
```

This approach provides the flexibility you requested while maintaining clean API design and proper validation!