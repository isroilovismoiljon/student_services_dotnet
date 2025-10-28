using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Auth;
namespace StudentServicesWebApi.Application.Validators;
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters")
            .MaximumLength(30)
            .WithMessage("Username must not exceed 30 characters")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("Username can only contain letters, numbers, and underscores");
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 characters")
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters")
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters")
            .MaximumLength(100)
            .WithMessage("Password must not exceed 100 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
            .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one digit");
        RuleFor(x => x.ReferralId)
            .GreaterThan(0)
            .WithMessage("Referral ID must be greater than 0")
            .When(x => x.ReferralId.HasValue);
    }
}
