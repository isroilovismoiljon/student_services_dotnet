using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.PhotoSlide;
using StudentServicesWebApi.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/PhotoSlides")]
[Produces("application/json")]
public class PhotoSlideController : ControllerBase
{
    private readonly IPhotoSlideService _photoSlideService;

    public PhotoSlideController(IPhotoSlideService photoSlideService)
    {
        _photoSlideService = photoSlideService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPhotoSlides([FromQuery] int pageNumber = 1, [FromQuery] [Range(1, 100)] int pageSize = 10, CancellationToken ct = default)
    {
        try
        {
            var (photoSlides, totalCount) = await _photoSlideService.GetPagedPhotoSlidesAsync(pageNumber, pageSize, ct);
            
            return Ok(new
            {
                success = true,
                data = new
                {
                    photoSlides = photoSlides,
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
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving photo slides",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPhotoSlideById(int id, CancellationToken ct = default)
    {
        try
        {
            var photoSlide = await _photoSlideService.GetPhotoSlideByIdAsync(id, ct);
            if (photoSlide == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Photo slide with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                data = photoSlide,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving the photo slide",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePhotoSlide(int id, [FromQuery] bool deleteFile = true, CancellationToken ct = default)
    {
        try
        {
            var result = await _photoSlideService.DeletePhotoSlideAsync(id, deleteFile, ct);
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = $"Photo slide with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = deleteFile ? "Photo slide and file deleted successfully" : "Photo slide deleted successfully (file preserved)",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while deleting the photo slide",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("{id:int}/duplicate")]
    public async Task<IActionResult> DuplicatePhotoSlide(int id, [FromQuery] double leftOffset = 10, [FromQuery] double topOffset = 10, [FromQuery] bool copyFile = true, CancellationToken ct = default)
    {
        try
        {
            var result = await _photoSlideService.DuplicatePhotoSlideAsync(id, leftOffset, topOffset, copyFile, ct);
            if (result == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Photo slide with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return CreatedAtAction(nameof(GetPhotoSlideById), new { id = result.Id }, new
            {
                success = true,
                message = "Photo slide duplicated successfully",
                data = result,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while duplicating the photo slide",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("{id:int}/replace-photo")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ReplacePhoto(int id, IFormFile newPhoto, [FromQuery] bool deleteOldFile = true, CancellationToken ct = default)
    {
        if (newPhoto == null)
            return BadRequest(new
            {
                success = false,
                message = "New photo file is required",
                timestamp = DateTime.UtcNow
            });

        try
        {
            var result = await _photoSlideService.ReplacePhotoAsync(id, newPhoto, deleteOldFile, ct);
            if (result == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Photo slide with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "Photo replaced successfully",
                data = result,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while replacing the photo",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchPhotoSlidesByFilename([FromQuery] string pattern, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(pattern))
            return BadRequest(new
            {
                success = false,
                message = "Search pattern is required",
                timestamp = DateTime.UtcNow
            });

        try
        {
            var photoSlides = await _photoSlideService.SearchPhotoSlidesByFilenameAsync(pattern, ct);
            return Ok(new
            {
                success = true,
                data = photoSlides,
                count = photoSlides.Count,
                searchPattern = pattern,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while searching photo slides",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentPhotoSlides([FromQuery] [Range(1, 50)] int count = 10, CancellationToken ct = default)
    {
        try
        {
            var photoSlides = await _photoSlideService.GetRecentPhotoSlidesAsync(count, ct);
            return Ok(new
            {
                success = true,
                data = photoSlides,
                count = photoSlides.Count,
                requestedCount = count,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving recent photo slides",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("recently-updated")]
    public async Task<IActionResult> GetRecentlyUpdatedPhotoSlides([FromQuery] [Range(1, 50)] int count = 10, CancellationToken ct = default)
    {
        try
        {
            var photoSlides = await _photoSlideService.GetRecentlyUpdatedPhotoSlidesAsync(count, ct);
            return Ok(new
            {
                success = true,
                data = photoSlides,
                count = photoSlides.Count,
                requestedCount = count,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving recently updated photo slides",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetPhotoSlideStats(CancellationToken ct = default)
    {
        try
        {
            var stats = await _photoSlideService.GetPhotoSlideStatsAsync(ct);
            return Ok(new
            {
                success = true,
                data = stats,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving photo slide statistics",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("{id:int}/exists")]
    public async Task<IActionResult> CheckPhotoSlideExists(int id, CancellationToken ct = default)
    {
        try
        {
            var exists = await _photoSlideService.PhotoSlideExistsAsync(id, ct);
            return Ok(new
            {
                success = true,
                exists = exists,
                id = id,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while checking photo slide existence",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("bulk")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> BulkCreatePhotoSlides([FromForm] BulkCreatePhotoSlideDto bulkCreateDto, CancellationToken ct = default)
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
            var result = await _photoSlideService.BulkCreatePhotoSlidesAsync(bulkCreateDto, ct);
            return Ok(new
            {
                success = true,
                message = $"Bulk upload completed: {result.SuccessfulUploads}/{result.TotalAttempted} successful",
                data = result,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred during bulk photo slide creation",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpDelete("bulk")]
    public async Task<IActionResult> BulkDeletePhotoSlides([FromBody] BulkPhotoSlideOperationDto bulkOperationDto, [FromQuery] bool deleteFiles = true, CancellationToken ct = default)
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
            var deletedCount = await _photoSlideService.BulkDeletePhotoSlidesAsync(bulkOperationDto, deleteFiles, ct);
            return Ok(new
            {
                success = true,
                message = $"{deletedCount} photo slides deleted successfully",
                deletedCount = deletedCount,
                filesDeleted = deleteFiles,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred during bulk photo slide deletion",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}