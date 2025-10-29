using StudentServicesWebApi.Application.DTOs.User;
namespace StudentServicesWebApi.Application.DTOs.Auth;
public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = default!;
    public object? User { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? TokenExpiry { get; set; }
    public string? VerificationCode { get; set; }
    public string? TelegramDeepLink { get; set; }
    public bool RequiresVerification { get; set; }
}
