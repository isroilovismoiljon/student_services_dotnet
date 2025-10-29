using FluentValidation;
using GemBox.Presentation;
using StudentServicesWebApi.Application.DTOs.TextSlide;
namespace StudentServicesWebApi.Application.Validators;
public class CreateTextSlideDtoValidator : AbstractValidator<CreateTextSlideDto>
{
    public CreateTextSlideDtoValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text content is required")
            .MaximumLength(5000).WithMessage("Text cannot exceed 5000 characters");
        RuleFor(x => x.Size)
            .GreaterThan(0).WithMessage("Font size must be greater than 0")
            .LessThanOrEqualTo(200).WithMessage("Font size cannot exceed 200");
        RuleFor(x => x.Font)
            .NotEmpty().WithMessage("Font is required")
            .MaximumLength(100).WithMessage("Font name cannot exceed 100 characters")
            .Must(BeValidFont).WithMessage("Font name contains invalid characters");
        RuleFor(x => x.ColorHex)
            .NotEmpty().WithMessage("Color is required")
            .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$").WithMessage("Color must be a valid hex color code (e.g., #000000 or #000)");
        RuleFor(x => x.Left)
            .GreaterThanOrEqualTo(0).WithMessage("Left position must be non-negative")
            .LessThanOrEqualTo(33.867).WithMessage("Left position cannot exceed 33.867 cm (slide width)");
        RuleFor(x => x.Top)
            .GreaterThanOrEqualTo(0).WithMessage("Top position must be non-negative")
            .LessThanOrEqualTo(19.05).WithMessage("Top position cannot exceed 19.05 cm (slide height)");
        RuleFor(x => x.Width)
            .GreaterThan(0).WithMessage("Width must be greater than 0")
            .LessThanOrEqualTo(33.867).WithMessage("Width cannot exceed 33.867 cm (slide width)");
        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0 when specified")
            .LessThanOrEqualTo(19.05).WithMessage("Height cannot exceed 19.05 cm (slide height)")
            .When(x => x.Height.HasValue);
        RuleFor(x => x.Horizontal)
            .IsInEnum().WithMessage("Invalid horizontal alignment value");
        RuleFor(x => x.Vertical)
            .IsInEnum().WithMessage("Invalid vertical alignment value");
        RuleFor(x => x)
            .Must(HaveValidPosition).WithMessage("Text element exceeds slide boundaries. Left + Width must not exceed 33.867 cm, Top + Height must not exceed 19.05 cm");
    }
    private static bool BeValidFont(string font)
    {
        if (string.IsNullOrWhiteSpace(font))
            return false;
        var forbiddenChars = new[] { '<', '>', '"', '\'', '&', '\n', '\r', '\t' };
        return !forbiddenChars.Any(font.Contains);
    }
    private static bool HaveValidPosition(CreateTextSlideDto dto)
    {
        const double MaxSlideWidth = 33.867;  // cm
        const double MaxSlideHeight = 19.05;   // cm
        
        bool widthValid = dto.Left + dto.Width <= MaxSlideWidth;
        bool heightValid = dto.Top + (dto.Height ?? 0) <= MaxSlideHeight;
        
        return widthValid && heightValid;
    }
}
public class UpdateTextSlideDtoValidator : AbstractValidator<UpdateTextSlideDto>
{
    public UpdateTextSlideDtoValidator()
    {
        RuleFor(x => x.Text)
            .MaximumLength(5000).WithMessage("Text cannot exceed 5000 characters")
            .When(x => x.Text != null);
        RuleFor(x => x.Size)
            .GreaterThan(0).WithMessage("Font size must be greater than 0")
            .LessThanOrEqualTo(200).WithMessage("Font size cannot exceed 200")
            .When(x => x.Size.HasValue);
        RuleFor(x => x.Font)
            .MaximumLength(100).WithMessage("Font name cannot exceed 100 characters")
            .Must(BeValidFont).WithMessage("Font name contains invalid characters")
            .When(x => x.Font != null);
        RuleFor(x => x.ColorHex)
            .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$").WithMessage("Color must be a valid hex color code (e.g., #000000 or #000)")
            .When(x => x.ColorHex != null);
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
        RuleFor(x => x.Horizontal)
            .IsInEnum().WithMessage("Invalid horizontal alignment value")
            .When(x => x.Horizontal.HasValue);
        RuleFor(x => x.Vertical)
            .IsInEnum().WithMessage("Invalid vertical alignment value")
            .When(x => x.Vertical.HasValue);
    }
    private static bool BeValidFont(string font)
    {
        if (string.IsNullOrWhiteSpace(font))
            return false;
        var forbiddenChars = new[] { '<', '>', '"', '\'', '&', '\n', '\r', '\t' };
        return !forbiddenChars.Any(font.Contains);
    }
}
public class BulkCreateTextSlideDtoValidator : AbstractValidator<BulkCreateTextSlideDto>
{
    public BulkCreateTextSlideDtoValidator()
    {
        RuleFor(x => x.TextSlides)
            .NotNull().WithMessage("TextSlides collection is required")
            .NotEmpty().WithMessage("At least one text slide is required")
            .Must(x => x.Count <= 100).WithMessage("Cannot create more than 100 text slides at once");
        RuleForEach(x => x.TextSlides)
            .SetValidator(new CreateTextSlideDtoValidator());
        RuleFor(x => x.TextSlides)
            .Must(HaveUniquePositions).WithMessage("Multiple text slides cannot have the same position in a single request")
            .When(x => x.TextSlides != null && x.TextSlides.Any());
    }
    private static bool HaveUniquePositions(List<CreateTextSlideDto> textSlides)
    {
        var positions = textSlides.Select(ts => new { ts.Text, ts.Left, ts.Top }).ToList();
        return positions.Count == positions.Distinct().Count();
    }
}
public class BulkTextSlideOperationDtoValidator : AbstractValidator<BulkTextSlideOperationDto>
{
    public BulkTextSlideOperationDtoValidator()
    {
        RuleFor(x => x.TextSlideIds)
            .NotNull().WithMessage("TextSlideIds collection is required")
            .NotEmpty().WithMessage("At least one text slide ID is required")
            .Must(x => x.Count <= 1000).WithMessage("Cannot operate on more than 1000 text slides at once");
        RuleForEach(x => x.TextSlideIds)
            .GreaterThan(0).WithMessage("Text slide ID must be greater than 0");
        RuleFor(x => x.TextSlideIds)
            .Must(HaveUniqueIds).WithMessage("Duplicate text slide IDs are not allowed")
            .When(x => x.TextSlideIds != null && x.TextSlideIds.Any());
    }
    private static bool HaveUniqueIds(List<int> ids)
    {
        return ids.Count == ids.Distinct().Count();
    }
}
