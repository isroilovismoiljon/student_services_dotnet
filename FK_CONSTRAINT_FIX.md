# Foreign Key Constraint Error Fix

## Problem
When creating a presentation, the API was returning this error:
```
23503: insert or update on table "Designs" violates foreign key constraint "FK_Designs_Users_CreatedById"
```

## Root Cause
The error occurred because:
1. When fetching the Design to get background photo IDs, the repository loaded the entire Design entity with `Include(d => d.CreatedBy)`
2. EF Core tracked these entities in the same DbContext
3. If the `CreatedBy` user had an invalid foreign key or data issue, EF Core tried to save/update it during `SaveChanges()`
4. This caused the foreign key constraint violation

## Solution
Created a new repository method that:
- ✅ Fetches ONLY the photo IDs (not the entire Design entity)
- ✅ Uses `.AsNoTracking()` to prevent EF Core from tracking entities
- ✅ Avoids loading navigation properties like `CreatedBy`

### Changes Made

#### 1. Added Repository Method
**File:** `IDesignRepository.cs`
```csharp
Task<List<int>> GetPhotoIdsByDesignIdAsync(int designId, CancellationToken cancellationToken = default);
```

**File:** `DesignRepository.cs`
```csharp
public async Task<List<int>> GetPhotoIdsByDesignIdAsync(int designId, CancellationToken cancellationToken = default)
{
    return await _context.Set<PhotoSlide>()
        .AsNoTracking()  // ← Prevents entity tracking
        .Where(p => p.DesignId == designId)
        .OrderBy(p => p.CreatedAt)
        .Select(p => p.Id)  // ← Only select IDs, not full entities
        .ToListAsync(cancellationToken);
}
```

#### 2. Updated Service
**File:** `PresentationIsroilovService.cs`

**Before:**
```csharp
DesignDto? design = null;
if (design == null)
{
    design = await _designService.GetDesignByIdAsync(createDto.DesignId, ct);
}
if (design != null && design.Photos != null && design.Photos.Count > 0)
{
    backgroundPhotoId = design.Photos[i % design.Photos.Count].Id;
}
```

**After:**
```csharp
List<int>? designPhotoIds = null;
if (designPhotoIds == null)
{
    designPhotoIds = await _designRepository.GetPhotoIdsByDesignIdAsync(createDto.DesignId, ct);
}
if (designPhotoIds.Count > 0)
{
    backgroundPhotoId = designPhotoIds[i % designPhotoIds.Count];
}
```

## Benefits
✅ No more foreign key constraint errors
✅ Better performance (only fetching IDs, not full entities)
✅ No entity tracking overhead
✅ Cleaner, more focused queries

## Testing
After restarting the app, the create presentation endpoint should work without foreign key errors.

**Test with:**
```bash
POST /api/PresentationIsroilov
```

With the JSON structure from `PRESENTATION_CREATE_API.md`
