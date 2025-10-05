using FluentValidation;
using Microsoft.AspNetCore.Http;
using StudentServicesWebApi.Application.DTOs.PhotoSlide;

namespace StudentServicesWebApi.Application.Validators;

public class CreatePhotoSlideDtoValidator : AbstractValidator<CreatePhotoSlideDto>
{
    public CreatePhotoSlideDtoValidator()
    {
        RuleFor(x => x.Photo)
            .NotNull().WithMessage("Photo file is required")
            .Must(HaveContent).WithMessage("Photo file cannot be empty")
            .Must(BeValidImageFile).WithMessage("Only JPEG, PNG, GIF, WebP, and BMP images are allowed")
            .Must(BeWithinSizeLimit).WithMessage("Photo file size cannot exceed 10MB");

        RuleFor(x => x.Left)
            .GreaterThanOrEqualTo(0).WithMessage("Left position must be non-negative");

        RuleFor(x => x.Top)
            .GreaterThanOrEqualTo(0).WithMessage("Top position must be non-negative");

        RuleFor(x => x.Width)
            .GreaterThan(0).WithMessage("Width must be greater than 0")
            .LessThanOrEqualTo(10000).WithMessage("Width cannot exceed 10000");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0 when specified")
            .LessThanOrEqualTo(10000).WithMessage("Height cannot exceed 10000")
            .When(x => x.Height.HasValue);

        // Custom rule to validate position doesn't create negative coordinates
        RuleFor(x => x)
            .Must(HaveValidPosition).WithMessage("Photo slide position and dimensions result in negative coordinates");
    }

    private static bool HaveContent(IFormFile? file)
    {
        return file != null && file.Length > 0;
    }

    private static bool BeValidImageFile(IFormFile? file)
    {
        if (file == null) return false;

        var allowedContentTypes = new[]
        {
            "image/jpeg",
            "image/jpg", 
            "image/png",
            "image/gif",
            "image/webp",
            "image/bmp"
        };

        return allowedContentTypes.Contains(file.ContentType?.ToLowerInvariant());
    }

    private static bool BeWithinSizeLimit(IFormFile? file)
    {
        if (file == null) return false;
        
        const long maxSizeInBytes = 10 * 1024 * 1024; // 10MB
        return file.Length <= maxSizeInBytes;
    }

    private static bool HaveValidPosition(CreatePhotoSlideDto dto)
    {
        // Ensure the photo slide fits within reasonable bounds
        return dto.Left >= 0 && dto.Top >= 0 && 
               dto.Left + dto.Width <= 50000 && 
               dto.Top + (dto.Height ?? 100) <= 50000;
    }
}

public class UpdatePhotoSlideDtoValidator : AbstractValidator<UpdatePhotoSlideDto>
{
    public UpdatePhotoSlideDtoValidator()
    {
        RuleFor(x => x.Photo)
            .Must(BeValidImageFile).WithMessage("Only JPEG, PNG, GIF, WebP, and BMP images are allowed")
            .Must(BeWithinSizeLimit).WithMessage("Photo file size cannot exceed 10MB")
            .When(x => x.Photo != null);

        RuleFor(x => x.Left)
            .GreaterThanOrEqualTo(0).WithMessage("Left position must be non-negative")
            .When(x => x.Left.HasValue);

        RuleFor(x => x.Top)
            .GreaterThanOrEqualTo(0).WithMessage("Top position must be non-negative")
            .When(x => x.Top.HasValue);

        RuleFor(x => x.Width)
            .GreaterThan(0).WithMessage("Width must be greater than 0")
            .LessThanOrEqualTo(10000).WithMessage("Width cannot exceed 10000")
            .When(x => x.Width.HasValue);

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0 when specified")
            .LessThanOrEqualTo(10000).WithMessage("Height cannot exceed 10000")
            .When(x => x.Height.HasValue);
    }

    private static bool BeValidImageFile(IFormFile? file)
    {
        if (file == null) return true; // null is valid for updates

        var allowedContentTypes = new[]
        {
            "image/jpeg",
            "image/jpg", 
            "image/png",
            "image/gif",
            "image/webp",
            "image/bmp"
        };

        return allowedContentTypes.Contains(file.ContentType?.ToLowerInvariant());
    }

    private static bool BeWithinSizeLimit(IFormFile? file)
    {
        if (file == null) return true; // null is valid for updates
        
        const long maxSizeInBytes = 10 * 1024 * 1024; // 10MB
        return file.Length <= maxSizeInBytes;
    }
}

public class BulkCreatePhotoSlideDtoValidator : AbstractValidator<BulkCreatePhotoSlideDto>
{
    public BulkCreatePhotoSlideDtoValidator()
    {
        RuleFor(x => x.Photos)
            .NotNull().WithMessage("Photos collection is required")
            .NotEmpty().WithMessage("At least one photo is required")
            .Must(x => x.Count <= 50).WithMessage("Cannot upload more than 50 photos at once");

        RuleForEach(x => x.Photos)
            .Must(HaveContent).WithMessage("Photo file cannot be empty")
            .Must(BeValidImageFile).WithMessage("Only JPEG, PNG, GIF, WebP, and BMP images are allowed")
            .Must(BeWithinSizeLimit).WithMessage("Photo file size cannot exceed 10MB");

        RuleFor(x => x.DefaultLeft)
            .GreaterThanOrEqualTo(0).WithMessage("Default left position must be non-negative");

        RuleFor(x => x.DefaultTop)
            .GreaterThanOrEqualTo(0).WithMessage("Default top position must be non-negative");

        RuleFor(x => x.DefaultWidth)
            .GreaterThan(0).WithMessage("Default width must be greater than 0")
            .LessThanOrEqualTo(10000).WithMessage("Default width cannot exceed 10000");

        RuleFor(x => x.DefaultHeight)
            .GreaterThan(0).WithMessage("Default height must be greater than 0 when specified")
            .LessThanOrEqualTo(10000).WithMessage("Default height cannot exceed 10000")
            .When(x => x.DefaultHeight.HasValue);

        RuleFor(x => x.Spacing)
            .GreaterThanOrEqualTo(0).WithMessage("Spacing must be non-negative");

        RuleFor(x => x.LayoutDirection)
            .IsInEnum().WithMessage("Invalid layout direction value");

        // Validate total file size
        RuleFor(x => x.Photos)
            .Must(HaveValidTotalSize).WithMessage("Total file size of all photos cannot exceed 100MB")
            .When(x => x.Photos != null && x.Photos.Any());
    }

    private static bool HaveContent(IFormFile file)
    {
        return file.Length > 0;
    }

    private static bool BeValidImageFile(IFormFile file)
    {
        var allowedContentTypes = new[]
        {
            "image/jpeg",
            "image/jpg", 
            "image/png",
            "image/gif",
            "image/webp",
            "image/bmp"
        };

        return allowedContentTypes.Contains(file.ContentType?.ToLowerInvariant());
    }

    private static bool BeWithinSizeLimit(IFormFile file)
    {
        const long maxSizeInBytes = 10 * 1024 * 1024; // 10MB per file
        return file.Length <= maxSizeInBytes;
    }

    private static bool HaveValidTotalSize(List<IFormFile> photos)
    {
        const long maxTotalSizeInBytes = 100 * 1024 * 1024; // 100MB total
        var totalSize = photos.Sum(p => p.Length);
        return totalSize <= maxTotalSizeInBytes;
    }
}

public class BulkPhotoSlideOperationDtoValidator : AbstractValidator<BulkPhotoSlideOperationDto>
{
    public BulkPhotoSlideOperationDtoValidator()
    {
        RuleFor(x => x.PhotoSlideIds)
            .NotNull().WithMessage("PhotoSlideIds collection is required")
            .NotEmpty().WithMessage("At least one photo slide ID is required")
            .Must(x => x.Count <= 1000).WithMessage("Cannot operate on more than 1000 photo slides at once");

        RuleForEach(x => x.PhotoSlideIds)
            .GreaterThan(0).WithMessage("Photo slide ID must be greater than 0");

        // Ensure no duplicate IDs
        RuleFor(x => x.PhotoSlideIds)
            .Must(HaveUniqueIds).WithMessage("Duplicate photo slide IDs are not allowed")
            .When(x => x.PhotoSlideIds != null && x.PhotoSlideIds.Any());
    }

    private static bool HaveUniqueIds(List<int> ids)
    {
        return ids.Count == ids.Distinct().Count();
    }
}