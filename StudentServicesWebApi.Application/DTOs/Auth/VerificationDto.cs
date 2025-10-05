namespace StudentServicesWebApi.Application.DTOs.Auth;

public class VerificationDto
{
    public int UserId { get; set; }
    public string VerificationCode { get; set; } = default!;
}

public class TelegramVerificationDto
{
    public string TelegramId { get; set; } = default!;
    public string VerificationCode { get; set; } = default!;
}

public class VerificationResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
