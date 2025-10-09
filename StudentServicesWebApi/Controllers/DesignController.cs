using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.Design;
using StudentServicesWebApi.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class DesignController : ControllerBase
{
    private readonly IDesignService _designService;
    private readonly ILogger<DesignController> _logger;

    public DesignController(
        IDesignService designService,
        ILogger<DesignController> logger)
    {
        _designService = designService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetDesigns(
        [FromQuery] int pageNumber = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var (designs, totalCount) = await _designService.GetPagedDesignsAsync(pageNumber, pageSize, ct);
            
            return Ok(new
            {
                success = true,
                data = new
                {
                    designs = designs,
                    pagination = new
                    {
                        currentPage = pageNumber,
                        pageSize = pageSize,
                        totalCount = totalCount,
                        totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                    }
                },
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving designs");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving designs",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDesignById(int id, CancellationToken ct = default)
    {
        try
        {
            var design = await _designService.GetDesignByIdAsync(id, ct);
            if (design == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Design with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                data = design,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving design {DesignId}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving the design",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateDesign(
        [FromForm] DesignCreateWithPhotosDto createDesignWithPhotosDto,
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

        // Additional validation for photos
        if (createDesignWithPhotosDto.Photos == null || createDesignWithPhotosDto.Photos.Length < 4)
        {
            return BadRequest(new
            {
                success = false,
                message = "At least 4 photos are required to create a design",
                timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var userId = GetCurrentUserId();
            var design = await _designService.CreateDesignAsync(createDesignWithPhotosDto, userId, ct);

            return CreatedAtAction(nameof(GetDesignById), new { id = design.Id }, new
            {
                success = true,
                message = $"Design created successfully with {createDesignWithPhotosDto.Photos.Length} photos",
                data = design,
                timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument while creating design with photos");
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating design with photos");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while creating the design with photos",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDesign(int id, [FromBody] UpdateDesignDto updateDesignDto, CancellationToken ct = default)
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
            var design = await _designService.UpdateDesignAsync(id, updateDesignDto, ct);
            if (design == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Design with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "Design updated successfully",
                data = design,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating design {DesignId}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while updating the design",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDesign(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _designService.DeleteDesignAsync(id, ct);
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = $"Design with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "Design deleted successfully",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting design {DesignId}", id);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while deleting the design",
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