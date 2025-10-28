using Microsoft.AspNetCore.Http;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface IFileUploadService
{
    Task<string> UploadPaymentReceiptAsync(IFormFile file, int? paymentId = null, CancellationToken cancellationToken = default);
    Task<bool> DeletePaymentReceiptAsync(string filePath, CancellationToken cancellationToken = default);
    bool IsValidPaymentReceiptFile(IFormFile file);
    Task<string> UploadPresentationFileAsync(IFormFile file, int? slideId = null, CancellationToken cancellationToken = default);
    Task<bool> DeletePresentationFileAsync(string filePath, CancellationToken cancellationToken = default);
    bool IsValidPresentationFile(IFormFile file);
    string GetFileUrl(string filePath);
    string[] GetAllowedExtensions();
    long GetMaxFileSize();
}
