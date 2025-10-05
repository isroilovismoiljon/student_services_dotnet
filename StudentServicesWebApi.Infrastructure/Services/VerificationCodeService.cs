using Microsoft.Extensions.Options;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Infrastructure.Configuration;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class VerificationCodeService : IVerificationCodeService
{
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly TelegramBotConfiguration _telegramConfig;

    public VerificationCodeService(
        IVerificationCodeRepository verificationCodeRepository,
        IOptions<TelegramBotConfiguration> telegramConfig)
    {
        _verificationCodeRepository = verificationCodeRepository;
        _telegramConfig = telegramConfig.Value;
    }

    public async Task<string> GenerateVerificationCodeAsync(int userId, VerificationCodeType codeType = VerificationCodeType.Registration)
    {
        // Invalidate existing codes for user of the same type
        await _verificationCodeRepository.InvalidateUserCodesAsync(userId);

        // Generate 4-digit code
        var random = new Random();
        var code = random.Next(1000, 9999).ToString();

        // Create verification code
        var verificationCode = new VerificationCode
        {
            UserId = userId,
            Code = code,
            CodeType = codeType,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5), // 5 minutes expiry
            TelegramDeepLink = codeType == VerificationCodeType.Registration ? GenerateTelegramDeepLink(code) : null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _verificationCodeRepository.AddAsync(verificationCode);
        return code;
    }

    public async Task<bool> ValidateCodeAsync(int userId, string code)
    {
        var verificationCode = await _verificationCodeRepository.GetValidCodeAsync(userId, code);
        return verificationCode != null;
    }

    public async Task<bool> ValidateCodeAsync(string code, string telegramId)
    {
        var verificationCode = await _verificationCodeRepository.GetByCodeAsync(code);
        
        if (verificationCode == null) 
            return false;

        // Update user's Telegram ID if code is valid
        var userRepository = verificationCode.User;
        // Note: This would need the user repository to update the TelegramId
        
        return true;
    }

    public string GenerateTelegramDeepLink(string code)
    {
        return $"{_telegramConfig.BaseUrl}/{_telegramConfig.BotUsername}?start={code}";
    }

    public async Task<VerificationCode?> GetVerificationByCodeAsync(string code)
    {
        return await _verificationCodeRepository.GetByCodeAsync(code);
    }

    public async Task<bool> MarkCodeAsUsedAsync(int codeId)
    {
        return await _verificationCodeRepository.MarkAsUsedAsync(codeId);
    }

    public async Task<string> GeneratePasswordResetCodeAsync(int userId)
    {
        return await GenerateVerificationCodeAsync(userId, VerificationCodeType.PasswordReset);
    }

    public async Task<bool> ValidatePasswordResetCodeAsync(int userId, string code)
    {
        var verificationCode = await _verificationCodeRepository.GetValidCodeAsync(userId, code);
        return verificationCode != null && verificationCode.CodeType == VerificationCodeType.PasswordReset;
    }

    public async Task<string> ResendVerificationCodeAsync(int userId, string telegramId)
    {
        // Generate new verification code (this will invalidate existing ones)
        var newCode = await GenerateVerificationCodeAsync(userId, VerificationCodeType.Registration);
        
        // Note: TelegramBotService will be injected via service locator pattern
        // This is acceptable here since this is an infrastructure service
        
        return newCode;
    }
}
