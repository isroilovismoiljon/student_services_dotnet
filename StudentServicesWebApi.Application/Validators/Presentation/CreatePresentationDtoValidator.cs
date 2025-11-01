using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Presentation;

namespace StudentServicesWebApi.Application.Validators.Presentation;

public class CreatePresentationDtoValidator : AbstractValidator<CreatePresentationDto>
{
    public CreatePresentationDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull().WithMessage("Title is required");

        RuleFor(x => x.Author)
            .NotNull().WithMessage("Author is required");

        RuleFor(x => x.PageCount)
            .GreaterThan(0).WithMessage("Page count must be greater than 0");
    }
}
