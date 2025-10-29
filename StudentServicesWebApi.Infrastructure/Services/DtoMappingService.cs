using AutoMapper;
using StudentServicesWebApi.Application.DTOs.Auth;
using StudentServicesWebApi.Application.DTOs.Design;
using StudentServicesWebApi.Application.DTOs.OpenaiKey;
using StudentServicesWebApi.Application.DTOs.Payment;
using StudentServicesWebApi.Application.DTOs.PhotoSlide;
using StudentServicesWebApi.Application.DTOs.Plan;
using StudentServicesWebApi.Application.DTOs.PresentationPage;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Interfaces;
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
        dto.Photo = _urlService.GetPaymentImageUrl(user.Photo);
        return dto;
    }
    public RegisterResponseDto MapToRegisterResponseDto(User user)
    {
        var dto = _mapper.Map<RegisterResponseDto>(user);
        dto.Photo = _urlService.GetPaymentImageUrl(user.Photo);
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
            Photo = _urlService.GetPaymentImageUrl(payment.Photo) ?? string.Empty, 
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
            Photo = _urlService.GetPaymentImageUrl(payment.Photo) ?? string.Empty, 
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
        var textPreview = textSlide.Text.Length <= 100 
            ? textSlide.Text 
            : textSlide.Text.Substring(0, 100) + "...";
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
        var fileSizeFormatted = FormatFileSize(photoSlide.FileSize);
        return new PhotoSlideDto
        {
            Id = photoSlide.Id,
            PhotoPath = photoSlide.PhotoPath,
            PhotoUrl = _urlService.GetPresentationPhotoUrl(photoSlide.PhotoPath) ?? string.Empty,
            OriginalFileName = photoSlide.OriginalFileName,
            FileSize = photoSlide.FileSize,
            FileSizeFormatted = fileSizeFormatted,
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
        var positionSummary = $"Left: {photoSlide.Left:F1}, Top: {photoSlide.Top:F1}, "
                            + $"Width: {photoSlide.Width:F1}"
                            + (photoSlide.Height.HasValue ? $", Height: {photoSlide.Height.Value:F1}" : "");
        var fileSizeFormatted = FormatFileSize(photoSlide.FileSize);
        var photoUrl = _urlService.GetPresentationPhotoUrl(photoSlide.PhotoPath) ?? string.Empty;
        return new PhotoSlideSummaryDto
        {
            Id = photoSlide.Id,
            PhotoPath = photoSlide.PhotoPath,
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
    public DesignDto MapToDesignDto(Design design)
    {
        var firstPhoto = design.Photos.FirstOrDefault();
        var firstPhotoUrl = firstPhoto != null 
            ? _urlService.GetPresentationPhotoUrl(firstPhoto.PhotoPath) 
            : null;
        return new DesignDto
        {
            Id = design.Id,
            Title = design.Title,
            CreatedBy = MapToUserResponseDto(design.CreatedBy),
            CreatedAt = design.CreatedAt,
            UpdatedAt = design.UpdatedAt,
            Photos = design.Photos.Select(MapToPhotoSlideDto).ToList(),
            FirstPhotoUrl = firstPhotoUrl
        };
    }
    public DesignSummaryDto MapToDesignSummaryDto(Design design)
    {
        var firstPhoto = design.Photos.FirstOrDefault();
        var firstPhotoUrl = firstPhoto != null 
            ? _urlService.GetPresentationPhotoUrl(firstPhoto.PhotoPath) 
            : null;
        return new DesignSummaryDto
        {
            Id = design.Id,
            Title = design.Title,
            FirstPhotoUrl = firstPhotoUrl,
            CreatedByName = $"{design.CreatedBy.FirstName} {design.CreatedBy.LastName}".Trim(),
            CreatedByUsername = design.CreatedBy.Username,
            CreatedAt = design.CreatedAt,
            UpdatedAt = design.UpdatedAt,
            PhotoCount = design.Photos.Count
        };
    }
    public async Task<OpenaiKeyDto> MapToOpenaiKeyDtoAsync(OpenaiKey openaiKey)
    {
        return new OpenaiKeyDto
        {
            Id = openaiKey.Id,
            Key = openaiKey.Key,
            UseCount = openaiKey.UseCount,
            CreatedAt = openaiKey.CreatedAt,
            UpdatedAt = openaiKey.UpdatedAt
        };
    }
    public async Task<OpenaiKeySummaryDto> MapToOpenaiKeySummaryDtoAsync(OpenaiKey openaiKey)
    {
        var maskedKey = await MaskKeyForDisplay(openaiKey.Key);
        var status = GetKeyStatus(openaiKey.UseCount);
        return new OpenaiKeySummaryDto
        {
            Id = openaiKey.Id,
            KeyMasked = maskedKey,
            UseCount = openaiKey.UseCount,
            Status = status,
            CreatedAt = openaiKey.CreatedAt,
            UpdatedAt = openaiKey.UpdatedAt
        };
    }
    private static async Task<string> MaskKeyForDisplay(string key)
    {
        await Task.CompletedTask; 
        if (string.IsNullOrEmpty(key) || key.Length < 10)
            return "****";
        return $"{key[..4]}****{key[^4..]}";
    }
    private static string GetKeyStatus(int useCount)
    {
        return useCount switch
        {
            >= 1000 => "Very High Usage",
            >= 500 => "High Usage",
            >= 100 => "Moderate Usage",
            >= 50 => "Low Usage",
            _ => "Active"
        };
    }
    public async Task<PlanDto> MapToPlanDtoAsync(Plan plan)
    {
        return new PlanDto
        {
            Id = plan.Id,
            PlanText = MapToTextSlideDto(plan.PlanText),
            Plans = MapToTextSlideDto(plan.Plans),
            CreatedAt = plan.CreatedAt,
            UpdatedAt = plan.UpdatedAt
        };
    }
    public async Task<PlanSummaryDto> MapToPlanSummaryDtoAsync(Plan plan)
    {
        var planTextPreview = plan.PlanText.Text.Length <= 50 
            ? plan.PlanText.Text 
            : plan.PlanText.Text.Substring(0, 50) + "...";
        var plansPreview = plan.Plans.Text.Length <= 50 
            ? plan.Plans.Text 
            : plan.Plans.Text.Substring(0, 50) + "...";
        return new PlanSummaryDto
        {
            Id = plan.Id,
            PlanTextPreview = planTextPreview,
            PlansPreview = plansPreview,
            CreatedAt = plan.CreatedAt,
            UpdatedAt = plan.UpdatedAt
        };
    }

    public PlanDto MapToPlanDto(Plan plan)
    {
        return new PlanDto
        {
            Id = plan.Id,
            PlanText = MapToTextSlideDto(plan.PlanText),
            Plans = MapToTextSlideDto(plan.Plans),
            CreatedAt = plan.CreatedAt,
            UpdatedAt = plan.UpdatedAt
        };
    }

    public PresentationPageDto MapToPresentationPageDto(PresentationPage presentationPage)
    {
        return new PresentationPageDto
        {
            Id = presentationPage.Id,
            PresentationIsroilovId = presentationPage.PresentationIsroilovId,
            PhotoId = presentationPage.PhotoId,
            BackgroundPhotoId = presentationPage.BackgroundPhotoId,
            WithPhoto = presentationPage.WithPhoto,
            CreatedAt = presentationPage.CreatedAt,
            UpdatedAt = presentationPage.UpdatedAt,
            Posts = presentationPage.PresentationPosts?.Select(pp => new PresentationPostSummaryDto
            {
                Id = pp.Id,
                TitleId = pp.TitleId,
                TextId = pp.TextId,
                CreatedAt = pp.CreatedAt
            }).ToList() ?? new List<PresentationPostSummaryDto>()
        };
    }
}
