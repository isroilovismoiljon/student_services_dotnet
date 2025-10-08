using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.Presentation;
using StudentServicesWebApi.Application.Interfaces;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/Presentations")]
[Produces("application/json")]
public class PresentationController : ControllerBase
{
    private readonly IPresentationService _presentationService;

    public PresentationController(IPresentationService presentationService)
    {
        _presentationService = presentationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPresentations(CancellationToken ct = default)
    {
        try
        {
            var presentations = await _presentationService.GetAllPresentationsAsync(ct);
            return Ok(new
            {
                success = true,
                data = presentations,
                count = presentations.Count,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving presentations",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPresentationById(int id, CancellationToken ct = default)
    {
        try
        {
            var presentation = await _presentationService.GetPresentationByIdAsync(id, ct);
            if (presentation == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Presentation with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                data = presentation,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving the presentation",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePresentation([FromBody] CreatePresentationDto createDto, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(new
            {
                success = false,
                message = "Invalid input data",
                errors = ModelState,
                timestamp = DateTime.UtcNow
            });

        try
        {
            var presentation = await _presentationService.CreatePresentationAsync(createDto, ct);
            return CreatedAtAction(nameof(GetPresentationById), new { id = presentation.Id }, new
            {
                success = true,
                message = "Presentation created successfully",
                data = presentation,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while creating the presentation",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePresentation(int id, [FromBody] UpdatePresentationDto updateDto, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(new
            {
                success = false,
                message = "Invalid input data",
                errors = ModelState,
                timestamp = DateTime.UtcNow
            });

        try
        {
            var presentation = await _presentationService.UpdatePresentationAsync(id, updateDto, ct);
            if (presentation == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Presentation with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "Presentation updated successfully",
                data = presentation,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while updating the presentation",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePresentation(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _presentationService.DeletePresentationAsync(id, ct);
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = $"Presentation with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "Presentation deleted successfully",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while deleting the presentation",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}