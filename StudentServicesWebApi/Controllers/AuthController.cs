using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.Auth;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/Auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _userService;

    public AuthController(IAuthService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var result = await _userService.RegisterAsync(registerDto);
            return Ok(new { success = true, data = result, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var result = await _userService.LoginAsync(loginDto);
            return Ok(new { success = true, data = result, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerificationDto verificationDto)
    {
        try
        {
            var result = await _userService.VerifyCodeAsync(verificationDto);
            return Ok(new { success = true, data = result, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpPost("resend-verification-code")]
    public async Task<IActionResult> ResendVerificationCode([FromBody] ResendVerificationCodeDto resendDto)
    {
        try
        {
            var result = await _userService.ResendVerificationCodeAsync(resendDto);
            return Ok(new { success = true, data = result, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var result = await _userService.ForgotPasswordAsync(forgotPasswordDto);
            return Ok(new { success = true, data = result, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpGet("exists")]
    public IActionResult CheckUserExists([FromQuery] string? username = null)
    {
        // Simple implementation for now
        return Ok(new { exists = false, timestamp = DateTime.UtcNow });
    }
}