# API Versioning Guide

## Overview
Your Student Services API now supports versioning to maintain backward compatibility while allowing for future enhancements.

## Supported Versions

### Version 1.0 (Current)
- **Route Pattern**: `/api/v1/notifications`
- **Features**: Basic CRUD operations for notifications
- **Status**: Stable

### Version 2.0 (Enhanced)
- **Route Pattern**: `/api/v2/notifications`
- **Features**: Enhanced responses, pagination, better metadata
- **Status**: Preview/Beta

## How to Specify API Version

### 1. URL Path (Recommended)
```
GET /api/v1/notifications/123
GET /api/v2/notifications/123
```

### 2. Query String
```
GET /api/notifications/123?apiVersion=1.0
GET /api/notifications/123?apiVersion=2.0
```

### 3. Header
```
GET /api/notifications/123
X-Version: 1.0

GET /api/notifications/123
X-Version: 2.0
```

### 4. Media Type
```
GET /api/notifications/123
Accept: application/json;ver=1.0

GET /api/notifications/123
Accept: application/json;ver=2.0
```

## API Differences

### V1 (Basic Response)
```json
{
  "id": 1,
  "title": "Welcome",
  "message": "Welcome to our platform!",
  "type": 1,
  "status": 1,
  "userId": 123,
  "createdAt": "2024-01-01T10:00:00Z"
}
```

### V2 (Enhanced Response)
```json
{
  "data": {
    "id": 1,
    "title": "Welcome",
    "message": "Welcome to our platform!",
    "type": 1,
    "status": 1,
    "userId": 123,
    "createdAt": "2024-01-01T10:00:00Z"
  },
  "version": "2.0",
  "retrievedAt": "2024-01-01T10:05:00Z",
  "links": {
    "self": "/api/v2/notifications/1",
    "update": "/api/v2/notifications/1",
    "delete": "/api/v2/notifications/1"
  }
}
```

## V2 Enhancements

### Pagination Support
```
GET /api/v2/notifications/user/123?page=1&pageSize=10
```

Response includes pagination metadata:
```json
{
  "data": [...],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "totalCount": 50,
    "totalPages": 5,
    "hasNextPage": true,
    "hasPreviousPage": false
  },
  "version": "2.0"
}
```

### Enhanced Metadata
All V2 responses include:
- API version information
- Timestamp information
- HATEOS links where applicable
- Enhanced error details

## Default Behavior
- If no version is specified, the API defaults to **v1.0**
- This ensures backward compatibility for existing clients

## Swagger Documentation
- V1 Documentation: Available at `/swagger` (select V1 from dropdown)
- V2 Documentation: Available at `/swagger` (select V2 from dropdown)

## Migration Guide

### For Existing Clients
- No changes required - continue using existing endpoints
- V1 endpoints remain fully functional

### For New Clients
- Use V2 endpoints for enhanced features
- Take advantage of pagination and enhanced metadata
- Use HATEOS links for better API discoverability

## Best Practices

1. **Always specify version explicitly** in production applications
2. **Use URL path versioning** for clarity and cacheability
3. **Monitor usage** of different API versions
4. **Plan deprecation** of older versions with adequate notice
5. **Document breaking changes** clearly

## Future Versioning

When creating new versions:
1. Create new controller class (e.g., `NotificationsV3Controller`)
2. Add `[ApiVersion("3.0")]` attribute
3. Update route to use version parameter
4. Add new Swagger document in Program.cs
5. Update this documentation
