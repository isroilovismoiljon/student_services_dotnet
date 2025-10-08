using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentServicesWebApi.Domain.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class TelegramBotHostedService : IHostedService
{
    private readonly ITelegramBotService _telegramBotService;
    private readonly ILogger<TelegramBotHostedService> _logger;

    public TelegramBotHostedService(ITelegramBotService telegramBotService, ILogger<TelegramBotHostedService> logger)
    {
        _telegramBotService = telegramBotService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting Telegram bot hosted service...");
            
            await _telegramBotService.StartBotAsync();
            
            _logger.LogInformation("Telegram bot hosted service started successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start Telegram bot hosted service");
            // Don't throw - let the application continue even if bot fails to start
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Stopping Telegram bot hosted service...");
            
            await _telegramBotService.StopBotAsync();
            
            _logger.LogInformation("Telegram bot hosted service stopped successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping Telegram bot hosted service");
        }
    }
}