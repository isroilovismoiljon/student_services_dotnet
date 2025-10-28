namespace StudentServicesWebApi.Application.DTOs.Auth;
public class RegisterDto
{
    public string Username { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int? ReferralId { get; set; }
}
