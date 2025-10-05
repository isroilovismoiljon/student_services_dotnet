namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface ITelegramBotService
{
    Task StartBotAsync();
    Task StopBotAsync();
    Task SendVerificationCodeAsync(string telegramId, string code);
    Task SendPasswordResetCodeAsync(string telegramId, string code);
    Task HandleStartCommandAsync(string telegramId, string? startParameter);
    Task SendMessageAsync(string telegramId, string message);
}
