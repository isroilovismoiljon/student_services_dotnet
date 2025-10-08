using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/TextSlides")]
[Produces("application/json")]
public class TextSlidesController : ControllerBase
{
    private readonly ITextSlideService _textSlideService;

    public TextSlidesController(ITextSlideService textSlideService)
    {
        _textSlideService = textSlideService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTextSlide([FromBody] CreateTextSlideDto createTextSlideDto, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { 
                success = false, 
                message = "Invalid input data", 
                errors = ModelState,
                timestamp = DateTime.UtcNow
            });

        try
        {
            // Validate creation first
            var isValid = await _textSlideService.ValidateTextSlideCreationAsync(createTextSlideDto, ct);
            if (!isValid)
                return BadRequest(new { 
                    success = false, 
                    message = "A text slide with similar properties already exists", 
                    timestamp = DateTime.UtcNow
                });

            var result = await _textSlideService.CreateTextSlideAsync(createTextSlideDto, ct);
            return CreatedAtAction(nameof(GetTextSlideById), new { id = result.Id }, new { 
                success = true, 
                message = "Text slide created successfully",
                data = result, 
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while creating the text slide", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTextSlideById(int id, CancellationToken ct = default)
    {
        try
        {
            var textSlide = await _textSlideService.GetTextSlideByIdAsync(id, ct);
            if (textSlide == null)
                return NotFound(new { 
                    success = false, 
                    message = $"Text slide with ID {id} not found", 
                    timestamp = DateTime.UtcNow
                });

            return Ok(new { 
                success = true, 
                data = textSlide, 
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while retrieving the text slide", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTextSlides([FromQuery] int pageNumber = 1, [FromQuery] [Range(1, 100)] int pageSize = 10, CancellationToken ct = default)
    {
        try
        {
            var (textSlides, totalCount) = await _textSlideService.GetPagedTextSlidesAsync(pageNumber, pageSize, ct);
            
            return Ok(new { 
                success = true, 
                data = new {
                    textSlides = textSlides,
                    pagination = new {
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
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while retrieving text slides", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTextSlide(int id, [FromBody] UpdateTextSlideDto updateTextSlideDto, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { 
                success = false, 
                message = "Invalid input data", 
                errors = ModelState,
                timestamp = DateTime.UtcNow
            });

        try
        {
            // Check if text slide exists
            var exists = await _textSlideService.TextSlideExistsAsync(id, ct);
            if (!exists)
                return NotFound(new { 
                    success = false, 
                    message = $"Text slide with ID {id} not found", 
                    timestamp = DateTime.UtcNow
                });

            // Validate update
            var isValid = await _textSlideService.ValidateTextSlideUpdateAsync(id, updateTextSlideDto, ct);
            if (!isValid)
                return BadRequest(new { 
                    success = false, 
                    message = "Update would create a duplicate text slide", 
                    timestamp = DateTime.UtcNow
                });

            var result = await _textSlideService.UpdateTextSlideAsync(id, updateTextSlideDto, ct);
            if (result == null)
                return NotFound(new { 
                    success = false, 
                    message = $"Text slide with ID {id} not found", 
                    timestamp = DateTime.UtcNow
                });

            return Ok(new { 
                success = true, 
                message = "Text slide updated successfully",
                data = result, 
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while updating the text slide", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTextSlide(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _textSlideService.DeleteTextSlideAsync(id, ct);
            if (!result)
                return NotFound(new { 
                    success = false, 
                    message = $"Text slide with ID {id} not found", 
                    timestamp = DateTime.UtcNow
                });

            return Ok(new { 
                success = true, 
                message = "Text slide deleted successfully", 
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while deleting the text slide", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchTextSlides([FromQuery] string searchTerm, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest(new { 
                success = false, 
                message = "Search term is required", 
                timestamp = DateTime.UtcNow
            });

        try
        {
            var textSlides = await _textSlideService.SearchTextSlidesAsync(searchTerm, ct);
            return Ok(new { 
                success = true, 
                data = textSlides, 
                count = textSlides.Count,
                searchTerm = searchTerm,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while searching text slides", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetTextSlideStats(CancellationToken ct = default)
    {
        try
        {
            var stats = await _textSlideService.GetTextSlideStatsAsync(ct);
            return Ok(new { 
                success = true, 
                data = stats, 
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while retrieving text slide statistics", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }
    [HttpGet("{id:int}/exists")]
    public async Task<IActionResult> CheckTextSlideExists(int id, CancellationToken ct = default)
    {
        try
        {
            var exists = await _textSlideService.TextSlideExistsAsync(id, ct);
            return Ok(new { 
                success = true, 
                exists = exists, 
                id = id,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while checking text slide existence", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("presentation-post/{presentationPostId:int}")]
    public async Task<IActionResult> GetTextSlidesByPresentationPostId(int presentationPostId, CancellationToken ct = default)
    {
        try
        {
            var textSlides = await _textSlideService.GetTextSlidesByPresentationPostIdAsync(presentationPostId, ct);
            return Ok(new { 
                success = true, 
                data = textSlides, 
                count = textSlides.Count,
                presentationPostId = presentationPostId,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while retrieving text slides by presentation post ID", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreateTextSlides([FromBody] BulkCreateTextSlideDto bulkCreateDto, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { 
                success = false, 
                message = "Invalid input data", 
                errors = ModelState,
                timestamp = DateTime.UtcNow
            });

        try
        {
            var results = await _textSlideService.BulkCreateTextSlidesAsync(bulkCreateDto, ct);
            return Ok(new { 
                success = true, 
                message = $"{results.Count} text slides created successfully",
                data = results, 
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred during bulk text slide creation", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }
    [HttpDelete("bulk")]
    public async Task<IActionResult> BulkDeleteTextSlides([FromBody] BulkTextSlideOperationDto bulkOperationDto, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { 
                success = false, 
                message = "Invalid input data", 
                errors = ModelState,
                timestamp = DateTime.UtcNow
            });

        try
        {
            var deletedCount = await _textSlideService.BulkDeleteTextSlidesAsync(bulkOperationDto, ct);
            return Ok(new { 
                success = true, 
                message = $"{deletedCount} text slides deleted successfully",
                deletedCount = deletedCount, 
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred during bulk text slide deletion", 
                details = ex.Message, 
                timestamp = DateTime.UtcNow
            });
        }
    }

    #region Private Helper Methods

    private static string BuildFormattingFilterDescription(bool? isBold, bool? isItalic)
    {
        var filters = new List<string>();
        if (isBold.HasValue)
            filters.Add($"Bold: {isBold.Value}");
        if (isItalic.HasValue)
            filters.Add($"Italic: {isItalic.Value}");
        
        return filters.Count > 0 ? string.Join(", ", filters) : "No formatting filters";
    }

    #endregion
}