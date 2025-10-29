using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using StudentServicesWebApi.Application.DTOs.Auth;
using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Infrastructure.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Services;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IVerificationCodeService _verificationCodeService;
    private readonly IJwtService _jwtService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDtoMappingService _dtoMappingService;
    public AuthService(
        IUserRepository userRepository, 
        IVerificationCodeService verificationCodeService,
        IJwtService jwtService,
        IServiceProvider serviceProvider,
        IDtoMappingService dtoMappingService)
    {
        _userRepository = userRepository;
        _verificationCodeService = verificationCodeService;
        _jwtService = jwtService;
        _serviceProvider = serviceProvider;
        _dtoMappingService = dtoMappingService;
    }
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userRepository.VerifiedUsernameExistsAsync(registerDto.Username))
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Username already exists. Please choose a different username."
            };
        }
        
        // Validate referral ID if provided
        User? referralUser = null;
        if (registerDto.ReferralId.HasValue)
        {
            referralUser = await _userRepository.GetByIdAsync(registerDto.ReferralId.Value);
            if (referralUser == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid referral ID. The referral user does not exist."
                };
            }
        }
        
        await _userRepository.DeleteUnverifiedUsersByUsernameAsync(registerDto.Username);
        var user = new User
        {
            Username = registerDto.Username,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PasswordHash = HashPassword(registerDto.Password),
            ReferralId = registerDto.ReferralId,
            UserRole = UserRole.User,
            Balance = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var createdUser = await _userRepository.AddAsync(user);
        
        // Add bonus to referral user's balance
        if (referralUser != null)
        {
            referralUser.Balance += 1000;
            referralUser.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(referralUser);
        }
        var verificationCode = await _verificationCodeService.GenerateVerificationCodeAsync(createdUser.Id);
        var telegramDeepLink = _verificationCodeService.GenerateTelegramDeepLink(verificationCode);
        var registerResponseDto = _dtoMappingService.MapToRegisterResponseDto(createdUser);
        return new AuthResponseDto
        {
            Success = true,
            User = registerResponseDto,
            TelegramDeepLink = telegramDeepLink,
            VerificationCode = verificationCode,
            Message = "To complete the registration, please log in to the Telegram bot via the link below, get the code, enter it in the here and go through verification.",
            RequiresVerification = true
        };
    }
    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.AuthenticateAsync(loginDto.Username, loginDto.Password);
        if (user == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid username or password"
            };
        }
        var userResponseDto = _dtoMappingService.MapToUserResponseDto(user);
        var token = _jwtService.GenerateToken(user);
        return new AuthResponseDto
        {
            Success = true,
            User = userResponseDto,
            Token = token,
            TokenExpiry = DateTime.UtcNow.AddMinutes(60),
            Message = "Login successful"
        };
    }
    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null ? _dtoMappingService.MapToUserResponseDto(user) : null;
    }
    public async Task<UserResponseDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        return user != null ? _dtoMappingService.MapToUserResponseDto(user) : null;
    }
    public async Task<VerificationResultDto> VerifyCodeAsync(VerificationDto verificationDto)
    {
        var user = await _userRepository.GetByIdAsync(verificationDto.UserId);
        if (user == null)
            return new VerificationResultDto { Success = false, Message = "User not found" };
        if (user.TelegramId == null)
            return new VerificationResultDto { Success = false, Message = "You need enter telegram bot." };
        var isValid = await _verificationCodeService.ValidateCodeAsync(
            verificationDto.UserId,
            verificationDto.VerificationCode
        );
        if (isValid)
        {
            var verificationCode = await _verificationCodeService.GetVerificationByCodeAsync(verificationDto.VerificationCode);
            if (verificationCode != null)
            {
                await _verificationCodeService.MarkCodeAsUsedAsync(verificationCode.Id);
            }
            user.IsVerified = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.DeleteUnverifiedUsersByUsernameAsync(user.Username);
        }
        return new VerificationResultDto
        {
            Success = isValid,
            Message = isValid ? "Verification successful" : "Invalid or expired verification code"
        };
    }
    public async Task<bool> LinkTelegramAccountAsync(TelegramVerificationDto telegramVerificationDto)
    {
        var verificationCode = await _verificationCodeService.GetVerificationByCodeAsync(telegramVerificationDto.VerificationCode);
        if (verificationCode == null)
            return false;
        var success = await _userRepository.UpdateTelegramIdAsync(verificationCode.UserId, telegramVerificationDto.TelegramId);
        if (success)
        {
            await _verificationCodeService.MarkCodeAsUsedAsync(verificationCode.Id);
        }
        return success;
    }
    public async Task<IEnumerable<UserResponseDto>> GetReferralsByUserIdAsync(int userId)
    {
        var referrals = await _userRepository.GetReferralsByUserIdAsync(userId);
        return referrals.Select(_dtoMappingService.MapToUserResponseDto);
    }
    public async Task<bool> UserExistsAsync(string? username = null, string? phoneNumber = null, string? telegramId = null)
    {
        return await _userRepository.ExistsAsync(username, phoneNumber, telegramId);
    }
    public async Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userRepository.GetByUsernameAsync(forgotPasswordDto.Username);
        if (user == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "User not found with the provided username"
            };
        }
        if (string.IsNullOrEmpty(user.TelegramId))
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "No Telegram account is linked to this user. Please contact support."
            };
        }
        var resetCode = await _verificationCodeService.GeneratePasswordResetCodeAsync(user.Id);
        var telegramBotService = _serviceProvider.GetService<ITelegramBotService>();
        if (telegramBotService != null)
        {
            await telegramBotService.SendPasswordResetCodeAsync(user.TelegramId, resetCode);
        }
        return new AuthResponseDto
        {
            Success = true,
            Message = "Password reset code has been sent to your Telegram account"
        };
    }
    public async Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userRepository.GetByUsernameAsync(resetPasswordDto.Username);
        if (user == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "User not found with the provided username"
            };
        }
        var isValidCode = await _verificationCodeService.ValidatePasswordResetCodeAsync(user.Id, resetPasswordDto.VerificationCode);
        if (!isValidCode)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid or expired verification code"
            };
        }
        user.PasswordHash = HashPassword(resetPasswordDto.Password);
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        var verificationCode = await _verificationCodeService.GetVerificationByCodeAsync(resetPasswordDto.VerificationCode);
        if (verificationCode != null)
        {
            await _verificationCodeService.MarkCodeAsUsedAsync(verificationCode.Id);
        }
        return new AuthResponseDto
        {
            Success = true,
            Message = "Password has been reset successfully. You can now login with your new password."
        };
    }
    public async Task<AuthResponseDto> ResendVerificationCodeAsync(ResendVerificationCodeDto resendDto)
    {
        var user = await _userRepository.GetByIdAsync(resendDto.UserId);
        if (user == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "User not found"
            };
        }
        if (user.IsVerified)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "User is already verified"
            };
        }
        if (string.IsNullOrEmpty(user.TelegramId))
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Telegram account is not linked. Please use the original verification link to link your Telegram account first."
            };
        }
        try
        {
            var newCode = await _verificationCodeService.ResendVerificationCodeAsync(user.Id, user.TelegramId);
            var telegramBotService = _serviceProvider.GetService<ITelegramBotService>();
            if (telegramBotService != null)
            {
                await telegramBotService.SendVerificationCodeAsync(user.TelegramId, newCode);
            }
            return new AuthResponseDto
            {
                Success = true,
                Message = "Verification code has been resent to your Telegram account"
            };
        }
        catch (Exception ex)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Failed to resend verification code. Please try again."
            };
        }
    }
    private string HashPassword(string password)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "salt"));
    }
}
