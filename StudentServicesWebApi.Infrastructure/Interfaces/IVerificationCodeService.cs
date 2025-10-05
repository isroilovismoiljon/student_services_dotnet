using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface IVerificationCodeService
{
    Task<string> GenerateVerificationCodeAsync(int userId, VerificationCodeType codeType = VerificationCodeType.Registration);
    Task<bool> ValidateCodeAsync(int userId, string code);
    Task<bool> ValidateCodeAsync(string code, string telegramId);
    string GenerateTelegramDeepLink(string code);
    Task<VerificationCode?> GetVerificationByCodeAsync(string code);
    Task<bool> MarkCodeAsUsedAsync(int codeId);
    Task<string> GeneratePasswordResetCodeAsync(int userId);
    Task<bool> ValidatePasswordResetCodeAsync(int userId, string code);
    Task<string> ResendVerificationCodeAsync(int userId, string telegramId);
}
