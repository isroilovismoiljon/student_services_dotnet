using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.PresentationPage;
using StudentServicesWebApi.Application.Interfaces;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/PresentationPages")]
[Produces("application/json")]
public class PresentationPageController : ControllerBase
{
    private readonly IPresentationPageService _pageService;

    public PresentationPageController(IPresentationPageService pageService)
    {
        _pageService = pageService;
    }

    [HttpGet("presentation/{presentationId:int}")]
    public async Task<IActionResult> GetPagesByPresentationId(int presentationId, CancellationToken ct = default)
    {
        try
        {
            var pages = await _pageService.GetPagesByPresentationIdAsync(presentationId, ct);
            return Ok(new
            {
                success = true,
                data = pages,
                count = pages.Count,
                presentationId = presentationId,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving presentation pages",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPageById(int id, CancellationToken ct = default)
    {
        try
        {
            var page = await _pageService.GetPageByIdAsync(id, ct);
            if (page == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Page with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                data = page,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving the page",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePage([FromBody] CreatePresentationPageDto createDto, CancellationToken ct = default)
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
            var page = await _pageService.CreatePageAsync(createDto, ct);
            return CreatedAtAction(nameof(GetPageById), new { id = page.Id }, new
            {
                success = true,
                message = "Page created successfully",
                data = page,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while creating the page",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePage(int id, [FromBody] UpdatePresentationPageDto updateDto, CancellationToken ct = default)
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
            var page = await _pageService.UpdatePageAsync(id, updateDto, ct);
            if (page == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Page with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "Page updated successfully",
                data = page,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while updating the page",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePage(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _pageService.DeletePageAsync(id, ct);
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = $"Page with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "Page deleted successfully",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while deleting the page",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}