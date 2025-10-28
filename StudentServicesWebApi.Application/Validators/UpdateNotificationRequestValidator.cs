using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Notification;
namespace StudentServicesWebApi.Application.Validators;
public class UpdateNotificationRequestValidator : AbstractValidator<UpdateNotificationRequestDto>
{
    public UpdateNotificationRequestValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Title));
        RuleFor(x => x.Message)
            .MaximumLength(1000)
            .WithMessage("Message must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Message));
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid notification type")
            .When(x => x.Type.HasValue);
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid notification status")
            .When(x => x.Status.HasValue);
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
