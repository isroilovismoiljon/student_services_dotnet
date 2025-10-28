using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Payment;
using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Application.Validators;
public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
{
    public CreatePaymentDtoValidator()
    {
        RuleFor(x => x.RequestedAmount)
            .NotEmpty().WithMessage("Requested amount is required")
            .GreaterThan(0).WithMessage("Requested amount must be greater than 0")
            .LessThanOrEqualTo(1000000).WithMessage("Requested amount cannot exceed 1,000,000");
        RuleFor(x => x.Photo)
            .NotNull().WithMessage("Receipt photo is required")
            .Must(file => file.Length > 0).WithMessage("Photo file cannot be empty")
            .Must(file => file.Length <= 10 * 1024 * 1024).WithMessage("Photo file size cannot exceed 10MB")
            .Must(file => IsValidImageContentType(file.ContentType)).WithMessage("Only JPEG, PNG, GIF, and WebP images are allowed");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
    private static bool IsValidImageContentType(string? contentType)
    {
        if (string.IsNullOrEmpty(contentType))
            return false;
        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        return allowedTypes.Contains(contentType.ToLowerInvariant());
    }
}
public class ProcessPaymentDtoValidator : AbstractValidator<ProcessPaymentDto>
{
    public ProcessPaymentDtoValidator()
    {
        RuleFor(x => x.PaymentStatus)
            .NotEqual(PaymentStatus.Waiting).WithMessage("Payment status cannot be Waiting when processing")
            .IsInEnum().WithMessage("Invalid payment status");
        RuleFor(x => x.ApprovedAmount)
            .GreaterThan(0).WithMessage("Approved amount must be greater than 0")
            .LessThanOrEqualTo(1000000).WithMessage("Approved amount cannot exceed 1,000,000")
            .When(x => x.PaymentStatus == PaymentStatus.Success && x.ApprovedAmount.HasValue);
        RuleFor(x => x.ApprovedAmount)
            .Null().WithMessage("Approved amount should not be set when rejecting payment")
            .When(x => x.PaymentStatus == PaymentStatus.Rejected);
        RuleFor(x => x.RejectReason)
            .NotEmpty().WithMessage("Reject reason is required when rejecting payment")
            .Length(10, 1000).WithMessage("Reject reason must be between 10 and 1000 characters")
            .When(x => x.PaymentStatus == PaymentStatus.Rejected);
        RuleFor(x => x.RejectReason)
            .Null().WithMessage("Reject reason should not be set when approving payment")
            .When(x => x.PaymentStatus == PaymentStatus.Success);
        RuleFor(x => x.AdminNotes)
            .MaximumLength(1000).WithMessage("Admin notes cannot exceed 1000 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.AdminNotes));
    }
}
