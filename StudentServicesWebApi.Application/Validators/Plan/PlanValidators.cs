using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Plan;
using StudentServicesWebApi.Application.Validators;

namespace StudentServicesWebApi.Application.Validators.Plan;

public class CreatePlanDtoValidator : AbstractValidator<CreatePlanDto>
{
    public CreatePlanDtoValidator()
    {
        RuleFor(x => x.PlanText)
            .NotNull()
            .WithMessage("PlanText is required")
            .SetValidator(new CreateTextSlideDtoValidator());

        RuleFor(x => x.Plans)
            .NotNull()
            .WithMessage("Plans is required")
            .SetValidator(new CreateTextSlideDtoValidator());
    }
}

public class UpdatePlanDtoValidator : AbstractValidator<UpdatePlanDto>
{
    public UpdatePlanDtoValidator()
    {
        RuleFor(x => x.PlanText)
            .SetValidator(new UpdateTextSlideDtoValidator()!)
            .When(x => x.PlanText != null);

        RuleFor(x => x.Plans)
            .SetValidator(new UpdateTextSlideDtoValidator()!)
            .When(x => x.Plans != null);

        // Ensure at least one property is provided for update
        RuleFor(x => x)
            .Must(x => x.PlanText != null || x.Plans != null)
            .WithMessage("At least one field must be provided for update");
    }
}