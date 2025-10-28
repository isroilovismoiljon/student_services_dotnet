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
    private readonly ILogger<PhotoSlideController> _logger;
    public PhotoSlideController(IPhotoSlideService photoSlideService, ILogger<PhotoSlideController> logger)
    {
        _photoSlideService = photoSlideService;
        _logger = logger;
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
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreatePhotoSlide([FromForm] CreatePhotoSlideDto createPhotoSlideDto, CancellationToken ct = default)
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
            var photoSlide = await _photoSlideService.CreatePhotoSlideAsync(createPhotoSlideDto, ct);
            return CreatedAtAction(nameof(GetPhotoSlideById), new { id = photoSlide.Id }, new
            {
                success = true,
                message = "Photo slide created successfully",
                data = photoSlide,
                timestamp = DateTime.UtcNow
            });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("position"))
        {
            return Conflict(new
            {
                success = false,
                message = ex.Message,
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
                message = "An error occurred while creating the photo slide",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdatePhotoSlide(int id, [FromForm] UpdatePhotoSlideDto updatePhotoSlideDto, CancellationToken ct = default)
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
            var photoSlide = await _photoSlideService.UpdatePhotoSlideAsync(id, updatePhotoSlideDto, ct);
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
                message = "Photo slide updated successfully",
                data = photoSlide,
                timestamp = DateTime.UtcNow
            });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("position"))
        {
            return Conflict(new
            {
                success = false,
                message = ex.Message,
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
                message = "An error occurred while updating the photo slide",
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
    [HttpPost("add-to-design/{designId:int}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddPhotoToDesign(
        int designId,
        [FromForm] AddPhotoToDesignDto addPhotoToDesignDto,
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
        try
        {
            var photoSlide = await _photoSlideService.AddPhotoToDesignAsync(designId, addPhotoToDesignDto, ct);
            return CreatedAtAction(nameof(GetPhotoSlideById), new { id = photoSlide.Id }, new
            {
                success = true,
                message = "Photo successfully added to design with default positioning",
                data = photoSlide,
                designId = designId,
                defaultPosition = new
                {
                    left = 0,
                    top = 0,
                    width = 33.867,
                    height = 19.05
                },
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
            _logger.LogError(ex, "Error adding photo to design {DesignId}", designId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while adding photo to design",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
    [HttpGet("design/{designId:int}")]
    public async Task<IActionResult> GetPhotosByDesignId(int designId, CancellationToken ct = default)
    {
        try
        {
            var allPhotos = await _photoSlideService.GetPagedPhotoSlidesAsync(1, 1000, ct); 
            var designPhotos = allPhotos.PhotoSlides.Where(p => p.Id > 0).ToList(); 
            var hasMinimumPhotos = designPhotos.Count >= 4;
            return Ok(new
            {
                success = true,
                data = new
                {
                    designId = designId,
                    photos = designPhotos,
                    photoCount = designPhotos.Count,
                    hasMinimumPhotos = hasMinimumPhotos,
                    minimumRequired = 4
                },
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving photos for design {DesignId}", designId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving design photos",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}
