namespace StudentServicesWebApi.Application.DTOs.Auth;

public class ForgotPasswordDto
{
    public string Username { get; set; } = default!;
}

public class ResetPasswordDto
{
    public string Username { get; set; } = default!;
    public string ResetCode { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}