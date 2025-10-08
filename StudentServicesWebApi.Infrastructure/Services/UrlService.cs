using Microsoft.AspNetCore.Http;
using StudentServicesWebApi.Domain.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class UrlService : IUrlService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UrlService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetImageUrl(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return null;

        var baseUrl = GetBaseUrl();
        
        // Ensure the relative path starts with a forward slash for proper URL construction
        var cleanPath = relativePath.Replace('\\', '/');
        
        // If the path doesn't start with uploads/, assume it's a payment receipt
        // Payment receipts are stored as just "2025/10/filename.png" but need to be served as "/uploads/payment-receipts/2025/10/filename.png"
        if (!cleanPath.StartsWith("/uploads/") && !cleanPath.StartsWith("uploads/"))
        {
            cleanPath = "/uploads/payment-receipts/" + cleanPath.TrimStart('/');
        }
        else if (!cleanPath.StartsWith('/'))
        {
            cleanPath = '/' + cleanPath;
        }

        return $"{baseUrl}{cleanPath}";
    }

    public string GetBaseUrl()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            // Fallback for scenarios where HttpContext is not available (background services, etc.)
            return "http://localhost:5074";
        }

        var request = context.Request;
        return $"{request.Scheme}://{request.Host}";
    }

    public string? GetFileUrl(string? relativePath)
    {
        // For now, files and images use the same transformation logic
        return GetImageUrl(relativePath);
    }
}