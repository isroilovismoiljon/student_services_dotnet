using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Domain.Models;

public class User : BaseEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? TelegramId { get; set; }
    public string PasswordHash { get; set; } = default!;
    public string? Photo { get; set; } = default!;
    public int Balance { get; set; } = 0;
    public int? ReferralId { get; set; }
    public UserRole UserRole { get; set; } = UserRole.User;
    public bool IsVerified { get; set; } = false;
}
