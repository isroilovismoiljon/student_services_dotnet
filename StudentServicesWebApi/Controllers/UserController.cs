using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IUserService userService,
        ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken ct = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var user = await _userService.GetUserByIdAsync(userId, ct);
            
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "User not found",
                    timestamp = DateTime.UtcNow
                });
            }

            return Ok(new
            {
                success = true,
                data = user,
                timestamp = DateTime.UtcNow
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                success = false,
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current user information");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving user information",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            if (pageNumber < 1)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Page number must be greater than 0",
                    timestamp = DateTime.UtcNow
                });
            }

            var (users, totalCount) = await _userService.GetAllUsersAsync(pageNumber, pageSize, ct);
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return Ok(new
            {
                success = true,
                data = new
                {
                    users = users,
                    pagination = new
                    {
                        currentPage = pageNumber,
                        pageSize = pageSize,
                        totalCount = totalCount,
                        totalPages = totalPages,
                        hasNextPage = pageNumber < totalPages,
                        hasPreviousPage = pageNumber > 1
                    }
                },
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving users",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("{userId:int}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GetUser(int userId, CancellationToken ct = default)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId, ct);
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"User with ID {userId} not found",
                    timestamp = DateTime.UtcNow
                });
            }

            return Ok(new
            {
                success = true,
                data = user,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user {UserId}", userId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving user",
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
}
