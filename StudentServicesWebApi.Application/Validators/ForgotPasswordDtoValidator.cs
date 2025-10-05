using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Auth;

namespace StudentServicesWebApi.Application.Validators;

public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters")
            .MaximumLength(30)
            .WithMessage("Username must not exceed 30 characters");
    }
}

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters")
            .MaximumLength(30)
            .WithMessage("Username must not exceed 30 characters");

        RuleFor(x => x.ResetCode)
            .NotEmpty()
            .WithMessage("Reset code is required")
            .Length(4)
            .WithMessage("Reset code must be exactly 4 digits")
            .Matches(@"^\d{4}$")
            .WithMessage("Reset code must contain only digits");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters")
            .MaximumLength(100)
            .WithMessage("Password must not exceed 100 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
            .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one digit");
    }
}