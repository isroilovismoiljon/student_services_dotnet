using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Notification;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Application.Validators;

public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequestDto>
{
    public CreateNotificationRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message is required")
            .MaximumLength(1000)
            .WithMessage("Message must not exceed 1000 characters");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid notification type");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0")
            .When(x => !x.IsGlobal);

        RuleFor(x => x.ActionUrl)
            .Must(BeValidUrl)
            .WithMessage("ActionUrl must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.ActionUrl));

        RuleFor(x => x.IconUrl)
            .Must(BeValidUrl)
            .WithMessage("IconUrl must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.IconUrl));

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("ExpiresAt must be in the future")
            .When(x => x.ExpiresAt.HasValue);

        RuleFor(x => x.Metadata)
            .Must(BeValidJson)
            .WithMessage("Metadata must be valid JSON")
            .When(x => !string.IsNullOrEmpty(x.Metadata));
    }

    private bool BeValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    private bool BeValidJson(string? json)
    {
        try
        {
            System.Text.Json.JsonDocument.Parse(json!);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
