using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.Presentation;
using StudentServicesWebApi.Application.Interfaces;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PresentationController : ControllerBase
{
    private readonly IPresentationService _presentationService;

    public PresentationController(IPresentationService presentationService)
    {
        _presentationService = presentationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        try
        {
            var presentations = await _presentationService.GetAllAsync(ct);
            return Ok(new { success = true, data = presentations, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        try
        {
            var presentation = await _presentationService.GetByIdAsync(id, ct);
            if (presentation == null)
            {
                return NotFound(new { success = false, message = "Presentation not found", timestamp = DateTime.UtcNow });
            }
            return Ok(new { success = true, data = presentation, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePresentationDto createDto, CancellationToken ct)
    {
        try
        {
            var presentation = await _presentationService.CreateAsync(createDto, ct);
            return CreatedAtAction(nameof(GetById), new { id = presentation.Id }, 
                new { success = true, data = presentation, timestamp = DateTime.UtcNow });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            var errorMessage = ex.InnerException != null 
                ? $"{ex.Message} | Inner: {ex.InnerException.Message}" 
                : ex.Message;
            return StatusCode(500, new { success = false, message = errorMessage, timestamp = DateTime.UtcNow });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            var result = await _presentationService.DeleteAsync(id, ct);
            if (!result)
            {
                return NotFound(new { success = false, message = "Presentation not found", timestamp = DateTime.UtcNow });
            }
            return Ok(new { success = true, message = "Presentation deleted successfully", timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }
}
