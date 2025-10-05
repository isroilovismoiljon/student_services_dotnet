namespace StudentServicesWebApi.Infrastructure.Interfaces;

/// <summary>
/// Service for handling URL transformations, especially for converting relative paths to full URLs
/// </summary>
public interface IUrlService
{
    /// <summary>
    /// Converts a relative image path to a full URL
    /// </summary>
    /// <param name="relativePath">The relative path stored in database</param>
    /// <returns>Full URL for the image, or null if path is null/empty</returns>
    string? GetImageUrl(string? relativePath);
    
    /// <summary>
    /// Gets the base URL for the application
    /// </summary>
    /// <returns>Base URL (e.g., https://localhost:5074)</returns>
    string GetBaseUrl();
    
    /// <summary>
    /// Converts a relative file path to a full URL
    /// </summary>
    /// <param name="relativePath">The relative path stored in database</param>
    /// <returns>Full URL for the file, or null if path is null/empty</returns>
    string? GetFileUrl(string? relativePath);
}