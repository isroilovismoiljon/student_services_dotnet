using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Presentation;

namespace StudentServicesWebApi.Application.Validators;

public class CreatePresentationWithPositionsDtoValidator : AbstractValidator<CreatePresentationWithPositionsDto>
{
    public CreatePresentationWithPositionsDtoValidator()
    {
        RuleFor(x => x.TitleText)
            .NotEmpty()
            .WithMessage("Title text is required");

        RuleFor(x => x.TitleFont)
            .NotEmpty()
            .WithMessage("Title font is required");

        RuleFor(x => x.TitleSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Title font size must be between 1 and 100");

        RuleFor(x => x.TitleLeft)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Title Left position must be between 0 and 33.867 cm");

        RuleFor(x => x.TitleTop)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Title Top position must be between 0 and 19.05 cm");

        RuleFor(x => x.TitleWidth)
            .GreaterThan(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Title Width must be between 1 and 33.867 cm");

        RuleFor(x => x.TitleHeight)
            .GreaterThan(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Title Height must be between 1 and 19.05 cm");

        // Author validation rules
        RuleFor(x => x.AuthorText)
            .NotEmpty()
            .WithMessage("Author text is required");

        RuleFor(x => x.AuthorFont)
            .NotEmpty()
            .WithMessage("Author font is required");

        RuleFor(x => x.AuthorSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Author font size must be between 1 and 100");

        RuleFor(x => x.AuthorLeft)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Author Left position must be between 0 and 33.867 cm");

        RuleFor(x => x.AuthorTop)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Author Top position must be between 0 and 19.05 cm");

        RuleFor(x => x.AuthorWidth)
            .GreaterThan(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Author Width must be between 1 and 33.867 cm");

        RuleFor(x => x.AuthorHeight)
            .GreaterThan(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Author Height must be between 1 and 19.05 cm");

        // Plan validation rules
        RuleFor(x => x.PlanText)
            .NotEmpty()
            .WithMessage("Plan text is required");

        RuleFor(x => x.PlanFont)
            .NotEmpty()
            .WithMessage("Plan font is required");

        RuleFor(x => x.PlanSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Plan font size must be between 1 and 100");

        RuleFor(x => x.PlanLeft)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Plan Left position must be between 0 and 33.867 cm");

        RuleFor(x => x.PlanTop)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Plan Top position must be between 0 and 19.05 cm");

        RuleFor(x => x.PlanWidth)
            .GreaterThan(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Plan Width must be between 1 and 33.867 cm");

        RuleFor(x => x.PlanHeight)
            .GreaterThan(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Plan Height must be between 1 and 19.05 cm");

        // General validation rules
        RuleFor(x => x.DesignId)
            .GreaterThan(0)
            .WithMessage("Design ID must be greater than 0");

        RuleFor(x => x.PageCount)
            .InclusiveBetween(1, 100)
            .WithMessage("Page count must be between 1 and 100");

        // Conditional validation for photo positions when WithPhoto is true
        RuleFor(x => x.PhotoPositions)
            .Must((dto, positions) => 
            {
                if (!dto.WithPhoto) return positions.Count == 0;
                var requiredCount = (dto.PageCount - 2) / 2;
                return positions.Count == requiredCount;
            })
            .WithMessage(dto => 
            {
                if (!dto.WithPhoto) return "Photo positions should be empty when WithPhoto is false";
                var requiredCount = (dto.PageCount - 2) / 2;
                return $"When WithPhoto is true and PageCount is {dto.PageCount}, exactly {requiredCount} photo positions are required";
            });

        // PostTexts validation - always required
        RuleFor(x => x.PostTexts)
            .NotEmpty()
            .WithMessage("PostTexts is required")
            .Must(postTexts => postTexts != null && postTexts.Count > 0)
            .WithMessage("At least one post text is required");

        // PageSettings validation
        RuleForEach(x => x.PageSettings).ChildRules(pageSetting =>
        {
            pageSetting.RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be positive");
        });

        // Validate that page settings don't exceed page count
        RuleFor(x => x.PageSettings)
            .Must((dto, pageSettings) => pageSettings.All(ps => ps.PageNumber <= dto.PageCount))
            .WithMessage("Page settings contain page numbers that exceed the total page count");
    }
}