using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Domain.Models;
public class VerificationCode : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public string Code { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } = false;
    public string? TelegramDeepLink { get; set; }
    public VerificationCodeType CodeType { get; set; } = VerificationCodeType.Registration;
}
