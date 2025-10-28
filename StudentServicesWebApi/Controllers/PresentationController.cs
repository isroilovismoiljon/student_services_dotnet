using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.Presentation;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.DTOs.Plan;
using StudentServicesWebApi.Application.Interfaces;
namespace StudentServicesWebApi.Controllers;
[ApiController]
[Route("api/Presentation")]
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
    [Consumes("application/json")]
    public async Task<IActionResult> CreatePresentation(
        [FromBody] CreatePresentationWithPositionsDto createDto,
        CancellationToken ct = default)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid input data",
                    errors = ModelState,
                    timestamp = DateTime.UtcNow
                });
            if (createDto.WithPhoto)
            {
                var expectedPhotoCount = (createDto.PageCount - 2) / 2;
                if (createDto.PhotoPositions.Count != expectedPhotoCount)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = $"When WithPhoto is true and PageCount is {createDto.PageCount}, exactly {expectedPhotoCount} photo positions are required. Got {createDto.PhotoPositions.Count}.",
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            else
            {
                if (createDto.PhotoPositions.Count > 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "PhotoPositions should be empty when WithPhoto is false",
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            var presentation = await _presentationService.CreatePresentationWithPositionsAsync(createDto, ct);
            return CreatedAtAction(nameof(GetPresentationById), new { id = presentation.Id }, new
            {
                success = true,
                message = "Presentation created successfully. Use UpdatePresentationPhotos endpoint to upload photo files.",
                data = presentation,
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
    [HttpPut("{id:int}/photos")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdatePresentationPhotos(
        int id,
        [FromForm] UpdatePresentationPhotosDto photosDto,
        CancellationToken ct = default)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid input data",
                    errors = ModelState,
                    timestamp = DateTime.UtcNow
                });
            var existingPresentation = await _presentationService.GetPresentationByIdAsync(id, ct);
            if (existingPresentation == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Presentation with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });
            if (!existingPresentation.WithPhoto)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "This presentation was created with WithPhoto=false. Photos cannot be added.",
                    timestamp = DateTime.UtcNow
                });
            }
            var expectedPhotoCount = (existingPresentation.PageCount - 2) / 2;
            if (photosDto.Photos.Count != expectedPhotoCount)
            {
                return BadRequest(new
                {
                    success = false,
                    message = $"Expected {expectedPhotoCount} photos for this presentation (PageCount: {existingPresentation.PageCount}), but got {photosDto.Photos.Count}",
                    timestamp = DateTime.UtcNow
                });
            }
            var updatedPresentation = await _presentationService.UpdatePresentationPhotosAsync(id, photosDto.Photos, ct);
            return Ok(new
            {
                success = true,
                message = "Presentation photos updated successfully",
                data = updatedPresentation,
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
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while updating presentation photos",
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
