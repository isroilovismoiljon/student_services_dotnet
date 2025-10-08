using Microsoft.AspNetCore.Http;

namespace StudentServicesWebApi.Domain.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadPaymentReceiptAsync(IFormFile file, int? paymentId = null, CancellationToken cancellationToken = default);
    Task<bool> DeletePaymentReceiptAsync(string filePath, CancellationToken cancellationToken = default);
    bool IsValidPaymentReceiptFile(IFormFile file);
    string GetFileUrl(string filePath);
    string[] GetAllowedExtensions();
    long GetMaxFileSize();
}
