using Microsoft.AspNetCore.Http;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface IFileUploadService
{
    /// <summary>
    /// Uploads a payment receipt photo and returns the file path/URL
    /// </summary>
    /// <param name="file">The uploaded file</param>
    /// <param name="paymentId">The payment ID (optional, for organizing files)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The file path or URL where the file is stored</returns>
    Task<string> UploadPaymentReceiptAsync(IFormFile file, int? paymentId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a payment receipt file
    /// </summary>
    /// <param name="filePath">The file path to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deleted successfully, false if file not found</returns>
    Task<bool> DeletePaymentReceiptAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if a file is a valid image for payment receipts
    /// </summary>
    /// <param name="file">The file to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    bool IsValidPaymentReceiptFile(IFormFile file);

    /// <summary>
    /// Gets the full URL for a payment receipt file
    /// </summary>
    /// <param name="filePath">The relative file path</param>
    /// <returns>The full URL to access the file</returns>
    string GetFileUrl(string filePath);

    /// <summary>
    /// Gets allowed file extensions for payment receipts
    /// </summary>
    /// <returns>Array of allowed extensions</returns>
    string[] GetAllowedExtensions();

    /// <summary>
    /// Gets the maximum allowed file size in bytes
    /// </summary>
    /// <returns>Maximum file size in bytes</returns>
    long GetMaxFileSize();
}