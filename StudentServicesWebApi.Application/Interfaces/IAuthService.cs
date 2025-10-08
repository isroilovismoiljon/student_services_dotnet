using StudentServicesWebApi.Application.DTOs.Auth;
using StudentServicesWebApi.Application.DTOs.User;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto?> GetUserByUsernameAsync(string username);
    Task<VerificationResultDto> VerifyCodeAsync(VerificationDto verificationDto);
    Task<bool> LinkTelegramAccountAsync(TelegramVerificationDto telegramVerificationDto);
    Task<IEnumerable<UserResponseDto>> GetReferralsByUserIdAsync(int userId);
    Task<bool> UserExistsAsync(string? username = null, string? phoneNumber = null, string? telegramId = null);
    Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<AuthResponseDto> ResendVerificationCodeAsync(ResendVerificationCodeDto resendDto);
}
