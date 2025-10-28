using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Presentation;
namespace StudentServicesWebApi.Application.Validators;
public class CreatePresentationDataDtoValidator : AbstractValidator<CreatePresentationDataDto>
{
    public CreatePresentationDataDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage("Title is required");
        RuleFor(x => x.Author)
            .NotNull()
            .WithMessage("Author is required");
        RuleFor(x => x.PlanData)
            .NotNull()
            .WithMessage("Plan data is required");
        RuleFor(x => x.DesignId)
            .GreaterThan(0)
            .WithMessage("Design ID must be greater than 0");
        RuleFor(x => x.PageCount)
            .InclusiveBetween(1, 100)
            .WithMessage("Page count must be between 1 and 100");
        RuleFor(x => x.PostTexts)
            .NotNull()
            .NotEmpty()
            .WithMessage("At least one post text is required");
        RuleFor(x => x.FilePath)
            .NotNull()
            .WithMessage("File path cannot be null");
        RuleFor(x => x.PhotoPositions)
            .NotNull()
            .WithMessage("Photo positions cannot be null");
        When(x => x.WithPhoto, () =>
        {
            RuleFor(x => x.PhotoPositions)
                .Must((dto, positions) =>
                {
                    var requiredCount = (dto.PageCount - 2) / 2;
                    return positions.Count == requiredCount;
                })
                .WithMessage(dto => 
                {
                    var requiredCount = (dto.PageCount - 2) / 2;
                    return $"With WithPhoto=true and PageCount={dto.PageCount}, exactly {requiredCount} photo positions are required, but {dto.PhotoPositions.Count} were provided.";
                });
        });
        When(x => !x.WithPhoto, () =>
        {
            RuleFor(x => x.PhotoPositions)
                .Must(positions => positions.Count == 0)
                .WithMessage("Photo positions should be empty when WithPhoto is false");
        });
        RuleForEach(x => x.PhotoPositions).SetValidator(new PhotoPositionDtoValidator());
    }
}
