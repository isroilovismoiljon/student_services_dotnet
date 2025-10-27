using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Presentation;

namespace StudentServicesWebApi.Application.Validators;

public class UpdatePresentationPhotosDtoValidator : AbstractValidator<UpdatePresentationPhotosDto>
{
    public UpdatePresentationPhotosDtoValidator()
    {
        RuleFor(x => x.Photos)
            .NotEmpty()
            .WithMessage("At least one photo is required");

        RuleForEach(x => x.Photos)
            .Must(file => file != null && file.Length > 0)
            .WithMessage("Each photo file must not be empty")
            .Must(file => IsValidImageFormat(file?.ContentType))
            .WithMessage("Only image files are allowed (JPEG, PNG, GIF, BMP, WebP)")
            .Must(file => file == null || file.Length <= 10 * 1024 * 1024) // 10MB limit
            .WithMessage("Each photo file size must not exceed 10MB");
    }

    private static bool IsValidImageFormat(string? contentType)
    {
        if (string.IsNullOrEmpty(contentType)) return false;

        var allowedTypes = new[]
        {
            "image/jpeg",
            "image/jpg", 
            "image/png",
            "image/gif",
            "image/bmp",
            "image/webp"
        };

        return allowedTypes.Contains(contentType.ToLowerInvariant());
    }
}