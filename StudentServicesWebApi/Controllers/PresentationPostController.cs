using Microsoft.AspNetCore.Mvc;
using StudentServicesWebApi.Application.DTOs.PresentationPost;
using StudentServicesWebApi.Application.Interfaces;

namespace StudentServicesWebApi.Controllers;

[ApiController]
[Route("api/PresentationPosts")]
[Produces("application/json")]
public class PresentationPostController : ControllerBase
{
    private readonly IPresentationPostService _postService;

    public PresentationPostController(IPresentationPostService postService)
    {
        _postService = postService;
    }

    [HttpGet("page/{presentationPageId:int}")]
    public async Task<IActionResult> GetPostsByPresentationPageId(int presentationPageId, CancellationToken ct = default)
    {
        try
        {
            var posts = await _postService.GetPostsByPresentationPageIdAsync(presentationPageId, ct);
            return Ok(new
            {
                success = true,
                data = posts,
                count = posts.Count,
                presentationPageId = presentationPageId,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving presentation posts",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPostById(int id, CancellationToken ct = default)
    {
        try
        {
            var post = await _postService.GetPostByIdAsync(id, ct);
            if (post == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Post with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                data = post,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving the post",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePresentationPostDto createDto, CancellationToken ct = default)
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
            var post = await _postService.CreatePostAsync(createDto, ct);
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, new
            {
                success = true,
                message = "Post created successfully",
                data = post,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while creating the post",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePresentationPostDto updateDto, CancellationToken ct = default)
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
            var post = await _postService.UpdatePostAsync(id, updateDto, ct);
            if (post == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Post with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "Post updated successfully",
                data = post,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while updating the post",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _postService.DeletePostAsync(id, ct);
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = $"Post with ID {id} not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "Post deleted successfully",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while deleting the post",
                details = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}