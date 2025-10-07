using System.Data;
using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.Notification;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/notifications")]
[Produces("application/json")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequestDto dto)
    {
        try
        {
            var result = await _notificationService.CreateNotificationAsync(dto);
            return Ok(new { success = true, data = result, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNotification(int id)
    {
        try
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            return Ok(new { success = true, data = notification, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserNotifications(int userId)
    {
        try
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(new { success = true, data = notifications, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpGet("user/unread")]
    public async Task<IActionResult> GetUserUnreadNotifications(int userId)
    {
        try
        {
            var notifications = await _notificationService.GetUnreadNotificationsByUserIdAsync(userId);
            return Ok(new { success = true, data = notifications, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

}