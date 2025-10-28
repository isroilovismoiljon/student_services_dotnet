using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Presentation;
namespace StudentServicesWebApi.Application.Validators;
public class CreatePresentationJsonDtoValidator : AbstractValidator<CreatePresentationJsonDto>
{
    public CreatePresentationJsonDtoValidator()
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
        RuleFor(x => x.DesignId)
            .GreaterThan(0)
            .WithMessage("Design ID must be greater than 0");
        RuleFor(x => x.PageCount)
            .InclusiveBetween(1, 100)
            .WithMessage("Page count must be between 1 and 100");
        RuleFor(x => x.PostTexts)
            .NotNull()
            .WithMessage("PostTexts is required")
            .NotEmpty()
            .WithMessage("At least one post text is required");
        When(x => x.WithPhoto, () => {
            RuleFor(x => x.PhotoPositions)
                .NotNull()
                .WithMessage("PhotoPositions is required when WithPhoto is true")
                .NotEmpty()
                .WithMessage("At least one photo position is required when WithPhoto is true");
            RuleForEach(x => x.PhotoPositions).ChildRules(position => {
                position.RuleFor(p => p.Left)
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(33.867)
                    .WithMessage("Photo Left position must be between 0 and 33.867 cm");
                position.RuleFor(p => p.Top)
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(19.05)
                    .WithMessage("Photo Top position must be between 0 and 19.05 cm");
                position.RuleFor(p => p.Width)
                    .GreaterThan(0)
                    .LessThanOrEqualTo(33.867)
                    .WithMessage("Photo Width must be between 1 and 33.867 cm");
                position.RuleFor(p => p.Height)
                    .GreaterThan(0)
                    .LessThanOrEqualTo(19.05)
                    .When(p => p.Height.HasValue)
                    .WithMessage("Photo Height must be between 1 and 19.05 cm");
            });
        });
    }
}
