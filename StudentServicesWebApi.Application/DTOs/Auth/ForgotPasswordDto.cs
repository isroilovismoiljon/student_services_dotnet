using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace StudentServicesWebApi.Application.DTOs.Auth;
public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Username is required")]
    [SwaggerSchema(Description = "The username of the account for which to reset the password.")]
    public string Username { get; set; } = default!;
}
public class ResetPasswordDto
{
    [Required(ErrorMessage = "Username is required")]
    [SwaggerSchema(Description = "The username of the account to reset the password for.")]
    public string Username { get; set; } = default!;
    [Required(ErrorMessage = "Verification code is required")]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "Verification code must be 4 digits")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Verification code must be a 4-digit number")]
    [SwaggerSchema(Description = "The 4-digit verification code sent to your Telegram account.")]
    public string VerificationCode { get; set; } = default!;
    [Required(ErrorMessage = "New password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    [SwaggerSchema(Description = "The new password for the account. Must be at least 6 characters long.")]
    public string Password { get; set; } = default!;
}
