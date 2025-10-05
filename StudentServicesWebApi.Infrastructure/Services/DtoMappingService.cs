using AutoMapper;
using StudentServicesWebApi.Application.DTOs.Payment;
using StudentServicesWebApi.Application.DTOs.PhotoSlide;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class DtoMappingService : IDtoMappingService
{
    private readonly IMapper _mapper;
    private readonly IUrlService _urlService;

    public DtoMappingService(IMapper mapper, IUrlService urlService)
    {
        _mapper = mapper;
        _urlService = urlService;
    }

    public UserResponseDto MapToUserResponseDto(User user)
    {
        var dto = _mapper.Map<UserResponseDto>(user);
        
        // Transform the photo path to full URL
        dto.Photo = _urlService.GetImageUrl(user.Photo);
        
        return dto;
    }

    public PaymentDto MapToPaymentDto(Payment payment)
    {
        var dto = new PaymentDto
        {
            Id = payment.Id,
            Sender = MapToUserResponseDto(payment.Sender),
            ProcessedByAdmin = payment.ProcessedByAdmin != null ? MapToUserResponseDto(payment.ProcessedByAdmin) : null,
            RequestedAmount = payment.RequestedAmount,
            ApprovedAmount = payment.ApprovedAmount,
            FinalAmount = payment.FinalAmount,
            Photo = _urlService.GetImageUrl(payment.Photo) ?? string.Empty, // Transform photo path to full URL
            Description = payment.Description,
            PaymentStatus = payment.PaymentStatus,
            RejectReason = payment.RejectReason,
            ProcessedAt = payment.ProcessedAt,
            AdminNotes = payment.AdminNotes,
            CreatedAt = payment.CreatedAt,
            UpdatedAt = payment.UpdatedAt,
            AmountWasAdjusted = payment.AmountWasAdjusted
        };

        return dto;
    }

    public PaymentSummaryDto MapToPaymentSummaryDto(Payment payment)
    {
        return new PaymentSummaryDto
        {
            Id = payment.Id,
            SenderName = $"{payment.Sender.FirstName} {payment.Sender.LastName}".Trim(),
            SenderUsername = payment.Sender.Username,
            ProcessedByAdminName = payment.ProcessedByAdmin != null 
                ? $"{payment.ProcessedByAdmin.FirstName} {payment.ProcessedByAdmin.LastName}".Trim() 
                : "Unassigned",
            RequestedAmount = payment.RequestedAmount,
            ApprovedAmount = payment.ApprovedAmount,
            FinalAmount = payment.FinalAmount,
            Photo = _urlService.GetImageUrl(payment.Photo) ?? string.Empty, // Transform photo path to full URL
            PaymentStatus = payment.PaymentStatus,
            CreatedAt = payment.CreatedAt,
            ProcessedAt = payment.ProcessedAt,
            AmountWasAdjusted = payment.AmountWasAdjusted
        };
    }

    public TextSlideDto MapToTextSlideDto(TextSlide textSlide)
    {
        return new TextSlideDto
        {
            Id = textSlide.Id,
            Text = textSlide.Text,
            Size = textSlide.Size,
            Font = textSlide.Font,
            IsBold = textSlide.IsBold,
            IsItalic = textSlide.IsItalic,
            ColorHex = textSlide.ColorHex,
            Left = textSlide.Left,
            Top = textSlide.Top,
            Width = textSlide.Width,
            Height = textSlide.Height,
            Horizontal = textSlide.Horizontal,
            Vertical = textSlide.Vertical,
            CreatedAt = textSlide.CreatedAt,
            UpdatedAt = textSlide.UpdatedAt
        };
    }

    public TextSlideSummaryDto MapToTextSlideSummaryDto(TextSlide textSlide)
    {
        // Create text preview (first 100 characters)
        var textPreview = textSlide.Text.Length <= 100 
            ? textSlide.Text 
            : textSlide.Text.Substring(0, 100) + "...";

        // Create position summary
        var positionSummary = $"Left: {textSlide.Left:F1}, Top: {textSlide.Top:F1}, "
                            + $"Width: {textSlide.Width:F1}"
                            + (textSlide.Height.HasValue ? $", Height: {textSlide.Height.Value:F1}" : "");

        return new TextSlideSummaryDto
        {
            Id = textSlide.Id,
            TextPreview = textPreview,
            Size = textSlide.Size,
            Font = textSlide.Font,
            IsBold = textSlide.IsBold,
            IsItalic = textSlide.IsItalic,
            ColorHex = textSlide.ColorHex,
            PositionSummary = positionSummary,
            CreatedAt = textSlide.CreatedAt,
            UpdatedAt = textSlide.UpdatedAt
        };
    }

    public PhotoSlideDto MapToPhotoSlideDto(PhotoSlide photoSlide)
    {
        return new PhotoSlideDto
        {
            Id = photoSlide.Id,
            PhotoUrl = _urlService.GetImageUrl(photoSlide.PhotoPath) ?? string.Empty,
            OriginalFileName = photoSlide.OriginalFileName,
            FileSize = photoSlide.FileSize,
            ContentType = photoSlide.ContentType,
            Left = photoSlide.Left,
            Top = photoSlide.Top,
            Width = photoSlide.Width,
            Height = photoSlide.Height,
            CreatedAt = photoSlide.CreatedAt,
            UpdatedAt = photoSlide.UpdatedAt
        };
    }

    public PhotoSlideSummaryDto MapToPhotoSlideSummaryDto(PhotoSlide photoSlide)
    {
        // Create position summary
        var positionSummary = $"Left: {photoSlide.Left:F1}, Top: {photoSlide.Top:F1}, "
                            + $"Width: {photoSlide.Width:F1}"
                            + (photoSlide.Height.HasValue ? $", Height: {photoSlide.Height.Value:F1}" : "");

        // Format file size to human readable
        var fileSizeFormatted = FormatFileSize(photoSlide.FileSize);

        var photoUrl = _urlService.GetImageUrl(photoSlide.PhotoPath) ?? string.Empty;
        
        return new PhotoSlideSummaryDto
        {
            Id = photoSlide.Id,
            ThumbnailUrl = photoUrl, // For now, same as photo URL - could be enhanced with thumbnail generation
            PhotoUrl = photoUrl,
            OriginalFileName = photoSlide.OriginalFileName,
            FileSize = photoSlide.FileSize,
            FileSizeFormatted = fileSizeFormatted,
            ContentType = photoSlide.ContentType,
            PositionSummary = positionSummary,
            CreatedAt = photoSlide.CreatedAt,
            UpdatedAt = photoSlide.UpdatedAt
        };
    }

    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        
        return $"{len:0.##} {sizes[order]}";
    }
}
