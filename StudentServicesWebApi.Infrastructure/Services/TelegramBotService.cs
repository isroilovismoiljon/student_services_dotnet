using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using StudentServicesWebApi.Infrastructure.Configuration;
using StudentServicesWebApi.Infrastructure.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Services;
public class TelegramBotService : ITelegramBotService
{
    private readonly ILogger<TelegramBotService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TelegramBotConfiguration _config;
    private TelegramBotClient? _botClient;
    private CancellationTokenSource? _cancellationTokenSource;
    public TelegramBotService(
        ILogger<TelegramBotService> logger,
        IServiceProvider serviceProvider,
        IOptions<TelegramBotConfiguration> config)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _config = config.Value;
    }
    public async Task StartBotAsync()
    {
        try
        {
            _logger.LogInformation("Starting Telegram bot...");
            if (string.IsNullOrEmpty(_config.BotToken))
            {
                _logger.LogWarning("Bot token is not configured. Telegram bot will not start.");
                return;
            }
            _botClient = new TelegramBotClient(_config.BotToken);
            _cancellationTokenSource = new CancellationTokenSource();
            var me = await _botClient.GetMe(_cancellationTokenSource.Token);
            _logger.LogInformation($"Bot @{me.Username} (ID: {me.Id}) started successfully!");
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() 
            };
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandlePollingErrorAsync,
                receiverOptions,
                _cancellationTokenSource.Token
            );
            _logger.LogInformation("Telegram bot is now listening for messages...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting Telegram bot. The bot will be disabled but the application will continue.");
            // Don't throw - let the app continue without the bot
        }
    }
    public async Task StopBotAsync()
    {
        try
        {
            _logger.LogInformation("Stopping Telegram bot...");
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _botClient = null;
            _logger.LogInformation("Telegram bot stopped.");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping Telegram bot");
            throw;
        }
    }
    public async Task SendVerificationCodeAsync(string telegramId, string code)
    {
        try
        {
            var message = $"Your verification code is: {code}\n\nThis code will expire in 5 minutes.";
            await SendMessageAsync(telegramId, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending verification code to {telegramId}");
            throw;
        }
    }
    public async Task SendPasswordResetCodeAsync(string telegramId, string code)
    {
        try
        {
            var message = $"🔐 Password Reset Request\n\n" +
                         $"Your password reset code is: *{code}*\n\n" +
                         $"⏰ This code will expire in 5 minutes.\n" +
                         $"🔒 Use this code to reset your password in the app.\n\n" +
                         $"⚠️ If you didn't request this, please ignore this message.";
            await SendMessageAsync(telegramId, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending password reset code to {telegramId}");
            throw;
        }
    }
    public async Task HandleStartCommandAsync(string telegramId, string? startParameter)
    {
        try
        {
            if (string.IsNullOrEmpty(startParameter))
            {
                await SendMessageAsync(telegramId, "Welcome! Please register through our web application first.");
                return;
            }
            using var scope = _serviceProvider.CreateScope();
            var verificationCodeService = scope.ServiceProvider.GetRequiredService<IVerificationCodeService>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var verificationCode = await verificationCodeService.GetVerificationByCodeAsync(startParameter);
            if (verificationCode == null)
            {
                await SendMessageAsync(telegramId, "❌ Invalid or expired verification code. Please try registering again.");
                return;
            }
            var existingUserWithTelegram = await userRepository.GetByTelegramIdAsync(telegramId);
            if (existingUserWithTelegram != null && existingUserWithTelegram.Id != verificationCode.UserId)
            {
                await SendMessageAsync(telegramId, $"❌ This Telegram account is already linked to this user\nId: {existingUserWithTelegram.Id}\nUsername: {existingUserWithTelegram.Username}");
                return;
            }
            try
            {
                var success = await userRepository.UpdateTelegramIdAsync(verificationCode.UserId, telegramId);
                if (success)
                {
                    await SendMessageAsync(telegramId,
                        $"✅ Great! Your Telegram account has been successfully linked!\n\n" +
                        $"🔑 Your verification code: *{startParameter}*\n\n" +
                        $"📱 You can now use this code in the app to complete your registration verification.");
                    _logger.LogInformation($"Successfully linked Telegram ID {telegramId} to User ID {verificationCode.UserId} with code {startParameter}");
                }
                else
                {
                    await SendMessageAsync(telegramId, "❌ Failed to link your Telegram account. Please try again.");
                    _logger.LogError($"Failed to link Telegram ID {telegramId} to User ID {verificationCode.UserId}");
                }
            }
            catch (InvalidOperationException ex)
            {
                await SendMessageAsync(telegramId, $"❌ {ex.Message}");
                _logger.LogWarning($"Could not link Telegram ID {telegramId}: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error handling start command for {telegramId}");
            await SendMessageAsync(telegramId, "An error occurred. Please try again later.");
        }
    }
    public async Task SendMessageAsync(string telegramId, string message)
    {
        try
        {
            if (_botClient == null)
            {
                _logger.LogWarning($"Bot client is not initialized. Cannot send message to {telegramId}");
                return;
            }
            if (long.TryParse(telegramId, out var chatId))
            {
                await _botClient.SendMessage(chatId, message);
                _logger.LogInformation($"Message sent to {telegramId}: {message}");
            }
            else
            {
                _logger.LogError($"Invalid Telegram ID format: {telegramId}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending message to {telegramId}");
            throw;
        }
    }
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;
            var chatId = message.Chat.Id;
            var telegramId = chatId.ToString();
            _logger.LogInformation($"Received message from {telegramId}: {messageText}");
            if (messageText.StartsWith("/start"))
            {
                var parts = messageText.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                var startParameter = parts.Length > 1 ? parts[1] : null;
                await HandleStartCommandAsync(telegramId, startParameter);
            }
            else if (messageText.StartsWith("/reset"))
            {
                await HandleResetCommandAsync(telegramId, messageText);
            }
            else if (messageText.StartsWith("/help"))
            {
                await HandleHelpCommandAsync(telegramId);
            }
            else
            {
                await botClient.SendMessage(chatId, 
                    "Welcome! Available commands:\n" +
                    "/start - Link your account or verify registration\n" +
                    "/reset - Check for password reset codes\n" +
                    "/help - Show this help message", 
                    cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling Telegram update");
        }
    }
    private async Task HandleResetCommandAsync(string telegramId, string messageText)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var verificationCodeService = scope.ServiceProvider.GetRequiredService<IVerificationCodeService>();
            var user = await userRepository.GetByTelegramIdAsync(telegramId);
            if (user == null)
            {
                await SendMessageAsync(telegramId, "❌ No account found linked to this Telegram. Please register first.");
                return;
            }
            var activeCode = await verificationCodeService.GetVerificationByCodeAsync(""); 
            await SendMessageAsync(telegramId, 
                "🔐 Password Reset Help\n\n" +
                "If you've requested a password reset, you should have received a code.\n\n" +
                "Use the code in the app to reset your password.\n\n" +
                "If you haven't received a code, please use the 'Forgot Password' option in the app.");
            _logger.LogInformation($"User {user.Username} (Telegram ID: {telegramId}) requested password reset help");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error handling reset command for {telegramId}");
            await SendMessageAsync(telegramId, "An error occurred. Please try again later.");
        }
    }
    private async Task HandleHelpCommandAsync(string telegramId)
    {
        try
        {
            var helpMessage = "🤖 Student Services Bot Help\n\n" +
                             "Available commands:\n" +
                             "/start - Link your account or verify registration\n" +
                             "/reset - Get help with password reset\n" +
                             "/help - Show this help message\n\n" +
                             "📱 How to use:\n" +
                             "1. Register in the app\n" +
                             "2. Click the Telegram verification link\n" +
                             "3. Complete verification with the received code\n\n" +
                             "🔐 Password Reset:\n" +
                             "Use 'Forgot Password' in the app to receive reset codes here.";
            await SendMessageAsync(telegramId, helpMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error handling help command for {telegramId}");
            await SendMessageAsync(telegramId, "An error occurred. Please try again later.");
        }
    }
    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        _logger.LogError(exception, "Telegram bot polling error: {ErrorMessage}", errorMessage);
        return Task.CompletedTask;
    }
}
