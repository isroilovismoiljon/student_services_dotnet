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

        RuleFor(x => x.PageCount)
            .GreaterThanOrEqualTo(5)
            .WithMessage("Page count must be at least 5");

        RuleFor(x => x.DesignId)
            .GreaterThan(0)
            .WithMessage("Design ID must be greater than 0");

        RuleFor(x => x.Plan)
            .NotNull()
            .WithMessage("Plan is required");

        RuleFor(x => x.PresentationPages)
            .NotNull()
            .WithMessage("Presentation pages are required")
            .Must(pages => pages != null && pages.Count >= 5)
            .WithMessage("At least 5 presentation pages are required");

        RuleForEach(x => x.PresentationPages)
            .ChildRules(page =>
            {
                page.RuleFor(p => p.PresentationPosts)
                    .NotNull()
                    .WithMessage("Presentation posts are required for each page")
                    .Must(posts => posts != null && posts.Count > 0)
                    .WithMessage("Each page must have at least one post");

                page.RuleForEach(p => p.PresentationPosts)
                    .ChildRules(post =>
                    {
                        post.RuleFor(pp => pp.Text)
                            .NotNull()
                            .WithMessage("Text is required for each post");
                    });
            });
    }
}
