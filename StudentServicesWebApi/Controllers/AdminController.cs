using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.Admin;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IAdminActionService _adminActionService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        IAdminActionService adminActionService,
        ILogger<AdminController> logger)
    {
        _adminActionService = adminActionService;
        _logger = logger;
    }

    [HttpPut("users/{userId:int}/role")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> UpdateUserRole(
        int userId,
        [FromBody] UpdateUserRoleDto updateUserRoleDto,
        CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(new
            {
                success = false,
                message = "Invalid input data",
                errors = ModelState,
                timestamp = DateTime.UtcNow
            });

        updateUserRoleDto.UserId = userId;

        try
        {
            var adminId = GetCurrentUserId();
            var ipAddress = GetClientIpAddress();

            var result = await _adminActionService.UpdateUserRoleAsync(updateUserRoleDto, adminId, ipAddress, ct);

            return Ok(new
            {
                success = true,
                message = "User role updated successfully",
                data = result,
                timestamp = DateTime.UtcNow
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                success = false,
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user role for user {UserId}", userId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while updating user role",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("users/{userId:int}/balance/add")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> AddUserBalance(
        int userId,
        [FromBody] ModifyBalanceDto modifyBalanceDto,
        CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(new
            {
                success = false,
                message = "Invalid input data",
                errors = ModelState,
                timestamp = DateTime.UtcNow
            });

        modifyBalanceDto.UserId = userId;

        try
        {
            var adminId = GetCurrentUserId();
            var ipAddress = GetClientIpAddress();

            var result = await _adminActionService.AddBalanceAsync(modifyBalanceDto, adminId, ipAddress, ct);

            return Ok(new
            {
                success = true,
                message = $"Balance of {modifyBalanceDto.Amount:C} added successfully",
                data = result,
                timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                success = false,
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding balance for user {UserId}", userId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while adding balance",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("users/{userId:int}/balance/subtract")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> SubtractUserBalance(
        int userId,
        [FromBody] ModifyBalanceDto modifyBalanceDto,
        CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(new
            {
                success = false,
                message = "Invalid input data",
                errors = ModelState,
                timestamp = DateTime.UtcNow
            });

        modifyBalanceDto.UserId = userId;

        try
        {
            var adminId = GetCurrentUserId();
            var ipAddress = GetClientIpAddress();

            var result = await _adminActionService.SubtractBalanceAsync(modifyBalanceDto, adminId, ipAddress, ct);

            return Ok(new
            {
                success = true,
                message = $"Balance of {modifyBalanceDto.Amount:C} subtracted successfully",
                data = result,
                timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                success = false,
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error subtracting balance for user {UserId}", userId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while subtracting balance",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("actions/{actionId:int}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GetAdminAction(int actionId, CancellationToken ct = default)
    {
        try
        {
            var action = await _adminActionService.GetAdminActionByIdAsync(actionId, ct);
            if (action == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Admin action with ID {actionId} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                data = action,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin action {ActionId}", actionId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving admin action",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("actions")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GetAdminActions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 10,
        [FromQuery] AdminActionType? actionType = null,
        CancellationToken ct = default)
    {
        try
        {
            var (actions, totalCount) = await _adminActionService.GetPagedAdminActionsAsync(
                pageNumber, pageSize, actionType, ct);

            return Ok(new
            {
                success = true,
                data = new
                {
                    actions = actions,
                    pagination = new
                    {
                        currentPage = pageNumber,
                        pageSize = pageSize,
                        totalCount = totalCount,
                        totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                    },
                    filter = new
                    {
                        actionType = actionType?.ToString()
                    }
                },
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin actions");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving admin actions",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("admins/{adminId:int}/actions")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GetAdminActionsByAdmin(
        int adminId,
        [FromQuery] AdminActionType? actionType = null,
        CancellationToken ct = default)
    {
        try
        {
            var actions = await _adminActionService.GetAdminActionsByAdminIdAsync(adminId, actionType, ct);

            return Ok(new
            {
                success = true,
                data = actions,
                adminId = adminId,
                actionType = actionType?.ToString(),
                count = actions.Count,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin actions for admin {AdminId}", adminId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving admin actions",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("users/{userId:int}/actions")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GetAdminActionsByUser(
        int userId,
        [FromQuery] AdminActionType? actionType = null,
        CancellationToken ct = default)
    {
        try
        {
            var actions = await _adminActionService.GetAdminActionsByUserIdAsync(userId, actionType, ct);

            return Ok(new
            {
                success = true,
                data = actions,
                userId = userId,
                actionType = actionType?.ToString(),
                count = actions.Count,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin actions for user {UserId}", userId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving admin actions",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("actions/recent")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GetRecentAdminActions(
        [FromQuery] [Range(1, 100)] int count = 20,
        CancellationToken ct = default)
    {
        try
        {
            var actions = await _adminActionService.GetRecentActionsAsync(count, ct);

            return Ok(new
            {
                success = true,
                data = actions,
                requestedCount = count,
                actualCount = actions.Count,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recent admin actions");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving recent admin actions",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }

    private string? GetClientIpAddress()
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        
        if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            var forwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forwardedFor))
            {
                ipAddress = forwardedFor.Split(',').FirstOrDefault()?.Trim();
            }
        }
        else if (HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
        {
            var realIp = HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(realIp))
            {
                ipAddress = realIp;
            }
        }

        return ipAddress;
    }
}
