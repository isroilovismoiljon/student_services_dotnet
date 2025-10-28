using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudentServicesWebApi.Domain.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
namespace StudentServicesWebApi.Infrastructure.Services;
public class FileUploadService : IFileUploadService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<FileUploadService> _logger;
    private readonly string _paymentUploadPath;
    private readonly string _paymentBaseUrl;
    private readonly string _presentationUploadPath;
    private readonly string _presentationBaseUrl;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private readonly string[] _allowedMimeTypes = { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
    private readonly long _maxFileSize = 10 * 1024 * 1024; 
    public FileUploadService(IConfiguration configuration, ILogger<FileUploadService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _paymentUploadPath = _configuration["FileUpload:PaymentReceipts:Path"] ?? "wwwroot/uploads/payment-receipts";
        _paymentBaseUrl = _configuration["FileUpload:PaymentReceipts:BaseUrl"] ?? "/uploads/payment-receipts";
        _presentationUploadPath = _configuration["FileUpload:PresentationFiles:Path"] ?? "wwwroot/uploads/presentation-files";
        _presentationBaseUrl = _configuration["FileUpload:PresentationFiles:BaseUrl"] ?? "/uploads/presentation-files";
        EnsureUploadDirectoryExists(_paymentUploadPath);
        EnsureUploadDirectoryExists(_presentationUploadPath);
    }
    public async Task<string> UploadPaymentReceiptAsync(IFormFile file, int? paymentId = null, CancellationToken cancellationToken = default)
    {
        if (!IsValidPaymentReceiptFile(file))
        {
            throw new ArgumentException("Invalid file format or size for payment receipt.");
        }
        try
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"payment_{paymentId?.ToString() ?? "temp"}_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}{fileExtension}";
            var yearMonth = DateTime.UtcNow.ToString("yyyy/MM");
            var directoryPath = Path.Combine(_paymentUploadPath, yearMonth);
            Directory.CreateDirectory(directoryPath);
            var filePath = Path.Combine(directoryPath, fileName);
            var relativePath = Path.Combine(yearMonth, fileName).Replace('\\', '/');
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }
            await OptimizeImageAsync(filePath, cancellationToken);
            _logger.LogInformation("Payment receipt uploaded: {FileName} for payment {PaymentId}", fileName, paymentId);
            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload payment receipt for payment {PaymentId}", paymentId);
            throw new InvalidOperationException("Failed to upload payment receipt.", ex);
        }
    }
    public async Task<bool> DeletePaymentReceiptAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            var fullPath = Path.Combine(_paymentUploadPath, filePath.Replace('/', '\\'));
            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath), cancellationToken);
                _logger.LogInformation("Payment receipt deleted: {FilePath}", filePath);
                return true;
            }
            _logger.LogWarning("Attempted to delete non-existent payment receipt: {FilePath}", filePath);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete payment receipt: {FilePath}", filePath);
            return false;
        }
    }
    public bool IsValidPaymentReceiptFile(IFormFile file)
    {
        return IsValidImageFile(file);
    }
    public async Task<string> UploadPresentationFileAsync(IFormFile file, int? slideId = null, CancellationToken cancellationToken = default)
    {
        if (!IsValidPresentationFile(file))
        {
            throw new ArgumentException("Invalid file format or size for presentation file.");
        }
        try
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"slide_{slideId?.ToString() ?? "temp"}_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}{fileExtension}";
            var yearMonth = DateTime.UtcNow.ToString("yyyy/MM");
            var directoryPath = Path.Combine(_presentationUploadPath, yearMonth);
            Directory.CreateDirectory(directoryPath);
            var filePath = Path.Combine(directoryPath, fileName);
            var relativePath = Path.Combine(yearMonth, fileName).Replace('\\', '/');
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }
            await OptimizeImageAsync(filePath, cancellationToken);
            _logger.LogInformation("Presentation file uploaded: {FileName} for slide {SlideId}", fileName, slideId);
            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload presentation file for slide {SlideId}", slideId);
            throw new InvalidOperationException("Failed to upload presentation file.", ex);
        }
    }
    public async Task<bool> DeletePresentationFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            var fullPath = Path.Combine(_presentationUploadPath, filePath.Replace('/', '\\'));
            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath), cancellationToken);
                _logger.LogInformation("Presentation file deleted: {FilePath}", filePath);
                return true;
            }
            _logger.LogWarning("Attempted to delete non-existent presentation file: {FilePath}", filePath);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete presentation file: {FilePath}", filePath);
            return false;
        }
    }
    public bool IsValidPresentationFile(IFormFile file)
    {
        return IsValidImageFile(file);
    }
    public string GetFileUrl(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return string.Empty;
        if (filePath.Contains("payment") || filePath.StartsWith(_paymentBaseUrl.TrimStart('/')))
        {
            return $"{_paymentBaseUrl.TrimEnd('/')}/{filePath.TrimStart('/')}";
        }
        else
        {
            return $"{_presentationBaseUrl.TrimEnd('/')}/{filePath.TrimStart('/')}";
        }
    }
    public string[] GetAllowedExtensions()
    {
        return _allowedExtensions;
    }
    public long GetMaxFileSize()
    {
        return _maxFileSize;
    }
    private void EnsureUploadDirectoryExists(string uploadPath)
    {
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
            _logger.LogInformation("Created upload directory: {UploadPath}", uploadPath);
        }
    }
    private bool IsValidImageFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;
        if (file.Length > _maxFileSize)
        {
            _logger.LogWarning("File size {FileSize} exceeds maximum allowed size {MaxSize}", file.Length, _maxFileSize);
            return false;
        }
        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
        {
            _logger.LogWarning("Invalid file extension: {Extension}", extension);
            return false;
        }
        if (!_allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            _logger.LogWarning("Invalid MIME type: {MimeType}", file.ContentType);
            return false;
        }
        return true;
    }
    private async Task OptimizeImageAsync(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length <= 2 * 1024 * 1024) 
                return;
            await Task.Run(() =>
            {
                using var image = Image.FromFile(filePath);
                const int maxWidth = 1920;
                const int maxHeight = 1080;
                int newWidth = image.Width;
                int newHeight = image.Height;
                if (newWidth > maxWidth || newHeight > maxHeight)
                {
                    double ratioX = (double)maxWidth / newWidth;
                    double ratioY = (double)maxHeight / newHeight;
                    double ratio = Math.Min(ratioX, ratioY);
                    newWidth = (int)(newWidth * ratio);
                    newHeight = (int)(newHeight * ratio);
                }
                if (newWidth != image.Width || newHeight != image.Height)
                {
                    using var resizedImage = new Bitmap(image, newWidth, newHeight);
                    var jpegEncoder = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
                    var tempPath = filePath + ".tmp";
                    resizedImage.Save(tempPath, jpegEncoder, encoderParameters);
                    File.Delete(filePath);
                    File.Move(tempPath, filePath);
                    _logger.LogInformation("Optimized image: {FilePath}, Original size: {OriginalSize}, New size: {NewSize}", 
                        filePath, fileInfo.Length, new FileInfo(filePath).Length);
                }
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to optimize image: {FilePath}", filePath);
        }
    }
}
