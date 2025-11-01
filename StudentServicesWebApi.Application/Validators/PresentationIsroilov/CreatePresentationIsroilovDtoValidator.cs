using FluentValidation;
using StudentServicesWebApi.Application.DTOs.PresentationIsroilov;

namespace StudentServicesWebApi.Application.Validators.PresentationIsroilov;

public class CreatePresentationIsroilovDtoValidator : AbstractValidator<CreatePresentationIsroilovDto>
{
    public CreatePresentationIsroilovDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage("Title is required");

        RuleFor(x => x.Author)
            .NotNull()
            .WithMessage("Author is required");

        RuleFor(x => x.Plan)
            .NotNull()
            .WithMessage("Plan is required");

        RuleFor(x => x.DesignId)
            .GreaterThan(0)
            .WithMessage("DesignId must be greater than 0");

        RuleFor(x => x.PageCount)
            .GreaterThan(0)
            .WithMessage("Page count must be greater than 0");
    }
}
