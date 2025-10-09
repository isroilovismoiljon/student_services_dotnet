using AutoMapper;
using StudentServicesWebApi.Application.DTOs.Admin;
using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class AdminActionService : IAdminActionService
{
    private readonly IAdminActionRepository _adminActionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotService _telegramBotService;
    private readonly IDtoMappingService _dtoMappingService;

    // Telegram IDs for admin notifications
    private readonly List<string> _adminTelegramIds = new() { "6224181119", "1364757999" };

    public AdminActionService(
        IAdminActionRepository adminActionRepository,
        IUserRepository userRepository,
        ITelegramBotService telegramBotService,
        IDtoMappingService dtoMappingService)
    {
        _adminActionRepository = adminActionRepository;
        _userRepository = userRepository;
        _telegramBotService = telegramBotService;
        _dtoMappingService = dtoMappingService;
    }

    public async Task<AdminActionDto> UpdateUserRoleAsync(UpdateUserRoleDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default)
    {
        if (dto.NewRole != UserRole.User && dto.NewRole != UserRole.Admin)
        {
            throw new ArgumentException("Can only change role to User or Admin");
        }

        var targetUser = await _userRepository.GetByIdAsync(dto.UserId, ct);
        if (targetUser == null)
        {
            throw new ArgumentException("User not found");
        }

        if (targetUser.UserRole == UserRole.SuperAdmin)
        {
            throw new ArgumentException("Cannot change SuperAdmin role");
        }

        var previousRole = targetUser.UserRole;
        
        if (previousRole == dto.NewRole)
        {
            throw new ArgumentException("User already has the specified role");
        }

        targetUser.UserRole = dto.NewRole;
        targetUser.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(targetUser, ct);

        var adminAction = new AdminAction
        {
            AdminId = adminId,
            TargetUserId = dto.UserId,
            ActionType = AdminActionType.RoleChange,
            Description = $"Changed user role from {previousRole} to {dto.NewRole}",
            PreviousValue = previousRole.ToString(),
            NewValue = dto.NewRole.ToString(),
            Reason = dto.Reason,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdAction = await _adminActionRepository.AddAsync(adminAction, ct);
        var actionWithDetails = await _adminActionRepository.GetWithDetailsAsync(createdAction.Id, ct);

        await SendTelegramNotificationAsync(actionWithDetails!, ct);

        return MapToAdminActionDto(actionWithDetails!);
    }

    public async Task<AdminActionDto> AddBalanceAsync(ModifyBalanceDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default)
    {
        var targetUser = await _userRepository.GetByIdAsync(dto.UserId, ct);
        if (targetUser == null)
        {
            throw new ArgumentException("User not found");
        }

        var previousBalance = targetUser.Balance;
        var newBalance = previousBalance + (int)dto.Amount;

        targetUser.Balance = newBalance;
        targetUser.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(targetUser, ct);

        var adminAction = new AdminAction
        {
            AdminId = adminId,
            TargetUserId = dto.UserId,
            ActionType = AdminActionType.BalanceAdd,
            Description = $"Added {dto.Amount:C} to user balance",
            PreviousValue = previousBalance.ToString(),
            NewValue = newBalance.ToString(),
            Amount = dto.Amount,
            Reason = dto.Reason,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdAction = await _adminActionRepository.AddAsync(adminAction, ct);
        var actionWithDetails = await _adminActionRepository.GetWithDetailsAsync(createdAction.Id, ct);

        // Send Telegram notification
        await SendTelegramNotificationAsync(actionWithDetails!, ct);

        return MapToAdminActionDto(actionWithDetails!);
    }

    public async Task<AdminActionDto> SubtractBalanceAsync(ModifyBalanceDto dto, int adminId, string? ipAddress = null, CancellationToken ct = default)
    {
        // Get the target user
        var targetUser = await _userRepository.GetByIdAsync(dto.UserId, ct);
        if (targetUser == null)
        {
            throw new ArgumentException("User not found");
        }

        var previousBalance = targetUser.Balance;
        var newBalance = previousBalance - (int)dto.Amount;

        if (newBalance < 0)
        {
            throw new ArgumentException($"Insufficient balance. User has {previousBalance:C}, cannot subtract {dto.Amount:C}");
        }

        // Update user balance
        targetUser.Balance = newBalance;
        targetUser.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(targetUser, ct);

        // Create admin action record
        var adminAction = new AdminAction
        {
            AdminId = adminId,
            TargetUserId = dto.UserId,
            ActionType = AdminActionType.BalanceSubtract,
            Description = $"Subtracted {dto.Amount:C} from user balance",
            PreviousValue = previousBalance.ToString(),
            NewValue = newBalance.ToString(),
            Amount = dto.Amount,
            Reason = dto.Reason,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdAction = await _adminActionRepository.AddAsync(adminAction, ct);
        var actionWithDetails = await _adminActionRepository.GetWithDetailsAsync(createdAction.Id, ct);

        // Send Telegram notification
        await SendTelegramNotificationAsync(actionWithDetails!, ct);

        return MapToAdminActionDto(actionWithDetails!);
    }

    public async Task<AdminActionDto?> GetAdminActionByIdAsync(int actionId, CancellationToken ct = default)
    {
        var action = await _adminActionRepository.GetWithDetailsAsync(actionId, ct);
        return action == null ? null : MapToAdminActionDto(action);
    }

    public async Task<(List<AdminActionSummaryDto> Actions, int TotalCount)> GetPagedAdminActionsAsync(
        int pageNumber, 
        int pageSize, 
        AdminActionType? actionType = null, 
        CancellationToken ct = default)
    {
        var actions = await _adminActionRepository.GetPagedAsync(pageNumber, pageSize, actionType, ct);
        var totalCount = await _adminActionRepository.GetCountAsync(actionType, ct);

        return (actions.Select(MapToAdminActionSummaryDto).ToList(), totalCount);
    }

    public async Task<List<AdminActionSummaryDto>> GetAdminActionsByAdminIdAsync(int adminId, AdminActionType? actionType = null, CancellationToken ct = default)
    {
        var actions = await _adminActionRepository.GetByAdminIdAsync(adminId, actionType, ct);
        return actions.Select(MapToAdminActionSummaryDto).ToList();
    }

    public async Task<List<AdminActionSummaryDto>> GetAdminActionsByUserIdAsync(int userId, AdminActionType? actionType = null, CancellationToken ct = default)
    {
        var actions = await _adminActionRepository.GetByTargetUserIdAsync(userId, actionType, ct);
        return actions.Select(MapToAdminActionSummaryDto).ToList();
    }

    public async Task<List<AdminActionSummaryDto>> GetRecentActionsAsync(int count = 50, CancellationToken ct = default)
    {
        var actions = await _adminActionRepository.GetRecentActionsAsync(count, ct);
        return actions.Select(MapToAdminActionSummaryDto).ToList();
    }

    private async Task SendTelegramNotificationAsync(AdminAction action, CancellationToken ct = default)
    {
        try
        {
            var message = FormatTelegramMessage(action);
            
            foreach (var telegramId in _adminTelegramIds)
            {
                try
                {
                    await _telegramBotService.SendMessageAsync(telegramId, message);
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the main operation
                    Console.WriteLine($"Failed to send Telegram notification to {telegramId}: {ex.Message}");
                }
            }

            // Update the action to mark notification as sent
            action.NotificationSent = true;
            await _adminActionRepository.UpdateAsync(action, ct);
        }
        catch (Exception ex)
        {
            // Log error but don't fail the main operation
            Console.WriteLine($"Error sending Telegram notifications: {ex.Message}");
        }
    }

    private static string FormatTelegramMessage(AdminAction action)
    {
        var emoji = action.ActionType switch
        {
            AdminActionType.RoleChange => "👤",
            AdminActionType.BalanceAdd => "💰➕",
            AdminActionType.BalanceSubtract => "💰➖",
            _ => "⚙️"
        };

        var adminName = $"{action.Admin.FirstName} {action.Admin.LastName}".Trim();
        var targetName = $"{action.TargetUser.FirstName} {action.TargetUser.LastName}".Trim();

        return $"{emoji} **Admin Action Alert**\n\n" +
               $"**Action:** {action.ActionType}\n" +
               $"**Admin:** {adminName} (@{action.Admin.Username})\n" +
               $"**Target User:** {targetName} (@{action.TargetUser.Username})\n" +
               $"**Description:** {action.Description}\n" +
               (action.Amount.HasValue ? $"**Amount:** {action.Amount:C}\n" : "") +
               (string.IsNullOrEmpty(action.Reason) ? "" : $"**Reason:** {action.Reason}\n") +
               $"**Time:** {action.CreatedAt:yyyy-MM-dd HH:mm:ss} UTC";
    }

    private AdminActionDto MapToAdminActionDto(AdminAction action)
    {
        return new AdminActionDto
        {
            Id = action.Id,
            Admin = _dtoMappingService.MapToUserResponseDto(action.Admin),
            TargetUser = _dtoMappingService.MapToUserResponseDto(action.TargetUser),
            ActionType = action.ActionType,
            Description = action.Description,
            PreviousValue = action.PreviousValue,
            NewValue = action.NewValue,
            Amount = action.Amount,
            Reason = action.Reason,
            CreatedAt = action.CreatedAt,
            NotificationSent = action.NotificationSent
        };
    }

    private static AdminActionSummaryDto MapToAdminActionSummaryDto(AdminAction action)
    {
        return new AdminActionSummaryDto
        {
            Id = action.Id,
            AdminName = $"{action.Admin.FirstName} {action.Admin.LastName}".Trim(),
            AdminUsername = action.Admin.Username,
            TargetUserFullName = $"{action.TargetUser.FirstName} {action.TargetUser.LastName}".Trim(),
            TargetUserUsername = action.TargetUser.Username,
            ActionType = action.ActionType,
            Description = action.Description,
            Amount = action.Amount,
            CreatedAt = action.CreatedAt,
            NotificationSent = action.NotificationSent
        };
    }
}