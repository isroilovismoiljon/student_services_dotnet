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
    private readonly string _uploadPath;
    private readonly string _baseUrl;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private readonly string[] _allowedMimeTypes = { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
    private readonly long _maxFileSize = 10 * 1024 * 1024; // 10MB

    public FileUploadService(IConfiguration configuration, ILogger<FileUploadService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        // Get upload path from configuration or use default
        _uploadPath = _configuration["FileUpload:PaymentReceipts:Path"] ?? "wwwroot/uploads/payment-receipts";
        _baseUrl = _configuration["FileUpload:BaseUrl"] ?? "/uploads/payment-receipts";
        
        // Ensure upload directory exists
        EnsureUploadDirectoryExists();
    }

    public async Task<string> UploadPaymentReceiptAsync(IFormFile file, int? paymentId = null, CancellationToken cancellationToken = default)
    {
        if (!IsValidPaymentReceiptFile(file))
        {
            throw new ArgumentException("Invalid file format or size for payment receipt.");
        }

        try
        {
            // Generate unique filename
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"payment_{paymentId?.ToString() ?? "temp"}_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}{fileExtension}";
            
            // Create year/month directory structure
            var yearMonth = DateTime.UtcNow.ToString("yyyy/MM");
            var directoryPath = Path.Combine(_uploadPath, yearMonth);
            Directory.CreateDirectory(directoryPath);
            
            var filePath = Path.Combine(directoryPath, fileName);
            var relativePath = Path.Combine(yearMonth, fileName).Replace('\\', '/');

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            // Optionally resize image if too large
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
            var fullPath = Path.Combine(_uploadPath, filePath.Replace('/', '\\'));
            
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
        if (file == null || file.Length == 0)
            return false;

        // Check file size
        if (file.Length > _maxFileSize)
        {
            _logger.LogWarning("File size {FileSize} exceeds maximum allowed size {MaxSize}", file.Length, _maxFileSize);
            return false;
        }

        // Check file extension
        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
        {
            _logger.LogWarning("Invalid file extension: {Extension}", extension);
            return false;
        }

        // Check MIME type
        if (!_allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            _logger.LogWarning("Invalid MIME type: {MimeType}", file.ContentType);
            return false;
        }

        return true;
    }

    public string GetFileUrl(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return string.Empty;
            
        return $"{_baseUrl.TrimEnd('/')}/{filePath.TrimStart('/')}";
    }

    public string[] GetAllowedExtensions()
    {
        return _allowedExtensions;
    }

    public long GetMaxFileSize()
    {
        return _maxFileSize;
    }

    private void EnsureUploadDirectoryExists()
    {
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
            _logger.LogInformation("Created upload directory: {UploadPath}", _uploadPath);
        }
    }

    private async Task OptimizeImageAsync(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            // Only optimize if the file is too large (over 2MB)
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length <= 2 * 1024 * 1024) // 2MB
                return;

            await Task.Run(() =>
            {
                using var image = Image.FromFile(filePath);
                
                // Calculate new size (max 1920x1080 while maintaining aspect ratio)
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
                
                // Only resize if dimensions changed
                if (newWidth != image.Width || newHeight != image.Height)
                {
                    using var resizedImage = new Bitmap(image, newWidth, newHeight);
                    
                    // Save with high quality JPEG compression
                    var jpegEncoder = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
                    
                    // Create temporary file
                    var tempPath = filePath + ".tmp";
                    resizedImage.Save(tempPath, jpegEncoder, encoderParameters);
                    
                    // Replace original file
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
            // Don't throw - optimization failure shouldn't break upload
        }
    }
}