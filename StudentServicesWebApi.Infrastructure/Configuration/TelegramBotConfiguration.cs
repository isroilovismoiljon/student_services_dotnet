namespace StudentServicesWebApi.Infrastructure.Configuration;
public class TelegramBotConfiguration
{
    public const string SectionName = "TelegramBot";
    public string BotToken { get; set; } = string.Empty;
    public string BotUsername { get; set; } = "YourBotUsername";
    public string BaseUrl { get; set; } = "https://t.me";
}
