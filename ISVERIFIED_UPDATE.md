# IsVerified Field Added to Register Response

## Changes Made

### ✅ Created `RegisterResponseDto`
**File:** `StudentServicesWebApi.Application/DTOs/Auth/RegisterResponseDto.cs`

Created a new DTO specifically for registration response with `IsVerified` field:
```csharp
public class RegisterResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string? PhoneNumber { get; set; }
    public string? TelegramId { get; set; }
    public string? Photo { get; set; }
    public int Balance { get; set; }
    public UserRole UserRole { get; set; }
    public bool IsVerified { get; set; }  // ✅ NEW FIELD
    public int? ReferralId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### ✅ Updated AutoMapper
**File:** `StudentServicesWebApi.Application/Mappings/UserProfile.cs`
- Added mapping: `CreateMap<User, RegisterResponseDto>()`

### ✅ Updated DtoMappingService
**File:** `StudentServicesWebApi.Infrastructure/Services/DtoMappingService.cs`
- Added `MapToRegisterResponseDto(User user)` method

### ✅ Updated AuthService
**File:** `StudentServicesWebApi.Infrastructure/Services/AuthService.cs`
- Register method now uses `MapToRegisterResponseDto` instead of manually creating DTO

### ✅ Updated AuthResponseDto
**File:** `StudentServicesWebApi.Application/DTOs/Auth/AuthResponseDto.cs`
- Changed `User` property from `UserResponseDto?` to `object?` to support both DTOs

## Impact

### Affected Endpoints

Only the register endpoint returns the `IsVerified` field:

1. **POST** `/api/Auth/register` - Register new user
   ```json
   {
     "success": true,
     "message": "To complete the registration, please log in to the Telegram bot...",
     "user": {
       "id": 1,
       "firstName": "John",
       "lastName": "Doe",
       "username": "johndoe",
       "phoneNumber": null,
       "telegramId": null,
       "photo": null,
       "balance": 0,
       "userRole": 0,
       "isVerified": false,  // ✅ NEW - Only in register response
       "referralId": null,
       "createdAt": "2025-10-29T05:30:00Z",
       "updatedAt": "2025-10-29T05:30:00Z"
     },
     "verificationCode": "ABC123",
     "telegramDeepLink": "https://t.me/bot?start=ABC123",
     "requiresVerification": true
   }
   ```

**Other endpoints (login, get user) return `UserResponseDto` WITHOUT `isVerified`**

### Mapping

AutoMapper automatically handles the mapping since:
- `User` model already has `IsVerified` property
- `UserResponseDto` now has `IsVerified` property
- `UserProfile` mapping is already configured

## Benefits

✅ Clients can now see user verification status in all responses
✅ No breaking changes - only adding a new field
✅ Consistent with the existing `User` model structure

## Testing

After restarting the app, test with:

```bash
# Register a new user
POST /api/Auth/register

# Get user info
GET /api/User/{id}
```

Both should now return `isVerified` in the response.
