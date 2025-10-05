using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using StudentServicesWebApi.Application.DTOs.Auth;
using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IVerificationCodeService _verificationCodeService;
    private readonly IJwtService _jwtService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDtoMappingService _dtoMappingService;

    public UserService(
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
        // Check if username already exists for a verified user
        if (await _userRepository.VerifiedUsernameExistsAsync(registerDto.Username))
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Username already exists. Please choose a different username."
            };
        }

        // Delete any existing unverified users with the same username
        // This allows users to re-register if they never completed verification
        await _userRepository.DeleteUnverifiedUsersByUsernameAsync(registerDto.Username);

        // Create new user
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
        
        // Generate verification code
        var verificationCode = await _verificationCodeService.GenerateVerificationCodeAsync(createdUser.Id);
        var telegramDeepLink = _verificationCodeService.GenerateTelegramDeepLink(verificationCode);

        var userResponseDto = new Application.DTOs.User.UserResponseDto
        {
            Id = createdUser.Id,
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName,
            Username = createdUser.Username,
            PhoneNumber = createdUser.PhoneNumber,
            TelegramId = createdUser.TelegramId
        };

        return new AuthResponseDto
        {
            Success = true,
            User = userResponseDto,
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
            TokenExpiry = DateTime.UtcNow.AddMinutes(60), // Match JWT config expiry
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

        // If verification is successful, mark the code as used and verify the user
        if (isValid)
        {
            var verificationCode = await _verificationCodeService.GetVerificationByCodeAsync(verificationDto.VerificationCode);
            if (verificationCode != null)
            {
                await _verificationCodeService.MarkCodeAsUsedAsync(verificationCode.Id);
            }

            // Mark user as verified
            user.IsVerified = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            // Delete any other unverified users with the same username
            // This cleans up potential duplicates that may have been created
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

        // Update user's Telegram ID
        var success = await _userRepository.UpdateTelegramIdAsync(verificationCode.UserId, telegramVerificationDto.TelegramId);
        
        if (success)
        {
            // Mark verification code as used
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

        // Generate password reset code
        var resetCode = await _verificationCodeService.GeneratePasswordResetCodeAsync(user.Id);

        // Send the reset code to user's Telegram
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

        // Validate reset code
        var isValidCode = await _verificationCodeService.ValidatePasswordResetCodeAsync(user.Id, resetPasswordDto.ResetCode);
        if (!isValidCode)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid or expired reset code"
            };
        }

        // Update password
        user.PasswordHash = HashPassword(resetPasswordDto.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Mark reset code as used
        var verificationCode = await _verificationCodeService.GetVerificationByCodeAsync(resetPasswordDto.ResetCode);
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
            // Generate new verification code
            var newCode = await _verificationCodeService.ResendVerificationCodeAsync(user.Id, user.TelegramId);
            
            // Send the code via Telegram bot
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
        // TODO: Implement proper password hashing (BCrypt, Argon2, etc.)
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "salt"));
    }
}
