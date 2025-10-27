using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Presentation;

namespace StudentServicesWebApi.Application.Validators;

public class PhotoPositionDtoValidator : AbstractValidator<PhotoPositionDto>
{
    private const double MaxSlideWidth = 33.867;  // Maximum slide width in cm
    private const double MaxSlideHeight = 19.05;  // Maximum slide height in cm

    public PhotoPositionDtoValidator()
    {
        RuleFor(x => x.Left)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Left position must be non-negative")
            .LessThan(MaxSlideWidth)
            .WithMessage($"Left position must be less than {MaxSlideWidth} cm");

        RuleFor(x => x.Top)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Top position must be non-negative")
            .LessThan(MaxSlideHeight)
            .WithMessage($"Top position must be less than {MaxSlideHeight} cm");

        RuleFor(x => x.Width)
            .GreaterThan(0)
            .WithMessage("Width must be positive")
            .LessThanOrEqualTo(MaxSlideWidth)
            .WithMessage($"Width must not exceed {MaxSlideWidth} cm");

        RuleFor(x => x.Height)
            .GreaterThan(0)
            .When(x => x.Height.HasValue)
            .WithMessage("Height must be positive when provided")
            .LessThanOrEqualTo(MaxSlideHeight)
            .When(x => x.Height.HasValue)
            .WithMessage($"Height must not exceed {MaxSlideHeight} cm when provided");

        // Validate that Left + Width doesn't exceed slide width
        RuleFor(x => x)
            .Must(x => x.Left + x.Width <= MaxSlideWidth)
            .WithMessage($"The sum of Left position ({nameof(PhotoPositionDto.Left)}) and Width must not exceed {MaxSlideWidth} cm");

        // Validate that Top + Height doesn't exceed slide height (when height is provided)
        RuleFor(x => x)
            .Must(x => !x.Height.HasValue || (x.Top + x.Height.Value <= MaxSlideHeight))
            .WithMessage($"The sum of Top position ({nameof(PhotoPositionDto.Top)}) and Height must not exceed {MaxSlideHeight} cm when Height is provided");
    }
}