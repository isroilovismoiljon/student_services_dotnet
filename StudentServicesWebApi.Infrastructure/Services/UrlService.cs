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
    public string? GetPaymentImageUrl(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return null;
        var baseUrl = GetBaseUrl();
        var cleanPath = relativePath.Replace('\\', '/');
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
            return "http://localhost:5074";
        }
        var request = context.Request;
        return $"{request.Scheme}://{request.Host}";
    }
    public string? GetPaymentFileUrl(string? relativePath)
    {
        return GetPaymentImageUrl(relativePath);
    }
    public string? GetPresentationPhotoUrl(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return null;
        var baseUrl = GetBaseUrl();
        var cleanPath = relativePath.Replace('\\', '/');
        if (!cleanPath.StartsWith("/uploads/") && !cleanPath.StartsWith("uploads/"))
        {
            cleanPath = "/uploads/presentation-files/" + cleanPath.TrimStart('/');
        }
        else if (!cleanPath.StartsWith('/'))
        {
            cleanPath = '/' + cleanPath;
        }
        return $"{baseUrl}{cleanPath}";
    }
    public string? GetPresentationFileUrl(string? relativePath)
    {
        return GetPresentationFileUrl(relativePath);
    }
}
