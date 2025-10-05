using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Application.DTOs.User;

public class UserResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? TelegramId { get; set; }
    public string? Photo { get; set; }
    public int Balance { get; set; }
    public UserRole UserRole { get; set; }
    public int? ReferralId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
