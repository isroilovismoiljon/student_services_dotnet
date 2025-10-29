using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.PresentationIsroilov;
using StudentServicesWebApi.Application.Interfaces;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PresentationIsroilovController : ControllerBase
{
    private readonly IPresentationIsroilovService _presentationService;

    public PresentationIsroilovController(IPresentationIsroilovService presentationService)
    {
        _presentationService = presentationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        try
        {
            var presentations = await _presentationService.GetAllPresentationsAsync(ct);
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
            var presentation = await _presentationService.GetPresentationByIdAsync(id, ct);
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
    public async Task<IActionResult> Create([FromBody] CreatePresentationIsroilovDto createDto, CancellationToken ct)
    {
        try
        {
            var presentation = await _presentationService.CreatePresentationAsync(createDto, ct);
            return CreatedAtAction(nameof(GetById), new { id = presentation.Id }, 
                new { success = true, data = presentation, timestamp = DateTime.UtcNow });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            // Include inner exception details for debugging
            var errorMessage = ex.InnerException != null 
                ? $"{ex.Message} | Inner: {ex.InnerException.Message}" 
                : ex.Message;
            return StatusCode(500, new { success = false, message = errorMessage, timestamp = DateTime.UtcNow });
        }
    }

    [HttpPut("{id}/photos")]
    public async Task<IActionResult> UpdatePhotos(int id, [FromForm] UpdatePresentationIsroilovPhotosDto updateDto, CancellationToken ct)
    {
        try
        {
            var presentation = await _presentationService.UpdatePresentationPhotosAsync(id, updateDto, ct);
            return Ok(new { success = true, data = presentation, timestamp = DateTime.UtcNow });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            var result = await _presentationService.DeletePresentationAsync(id, ct);
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
