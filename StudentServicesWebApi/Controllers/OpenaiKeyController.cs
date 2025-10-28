using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using StudentServicesWebApi.Application.DTOs.OpenaiKey;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OpenaiKeyController(IOpenaiKeyService openaiKeyService) : ControllerBase
{
    private readonly IOpenaiKeyService _openaiKeyService = openaiKeyService;
    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRole.SuperAdmin)}")]
    [SwaggerOperation(Summary = "Create a new OpenAI key", Description = "Creates a new OpenAI API key in the system. Only admins can perform this action.")]
    [SwaggerResponse(201, "OpenAI key created successfully", typeof(OpenaiKeyDto))]
    [SwaggerResponse(400, "Invalid request data or key already exists")]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    [SwaggerResponse(403, "Forbidden - admin role required")]
    public async Task<ActionResult<OpenaiKeyDto>> CreateAsync([FromBody] CreateOpenaiKeyDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _openaiKeyService.CreateAsync(createDto, cancellationToken);
            return Created($"/api/OpenaiKey/{result.Id}", result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the OpenAI key.", details = ex.Message });
        }
    }
    [HttpGet("{id}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.SuperAdmin)}")]
    [SwaggerOperation(Summary = "Get OpenAI key by ID", Description = "Retrieves a specific OpenAI key by its unique identifier.")]
    [SwaggerResponse(200, "OpenAI key found", typeof(OpenaiKeyDto))]
    [SwaggerResponse(404, "OpenAI key not found")]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    [SwaggerResponse(403, "Forbidden - admin role required")]
    public async Task<ActionResult<OpenaiKeyDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _openaiKeyService.GetByIdAsync(id, cancellationToken);
        return result == null ? NotFound(new { message = $"OpenAI key with ID {id} not found." }) : Ok(result);
    }
    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.SuperAdmin)}")]
    [SwaggerOperation(Summary = "Get all OpenAI keys", Description = "Retrieves all OpenAI keys in the system.")]
    [SwaggerResponse(200, "List of all OpenAI keys", typeof(List<OpenaiKeyDto>))]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    [SwaggerResponse(403, "Forbidden - admin role required")]
    public async Task<ActionResult<List<OpenaiKeyDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _openaiKeyService.GetAllAsync(cancellationToken);
        return Ok(result);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(UserRole.SuperAdmin)}")]
    [SwaggerOperation(Summary = "Update OpenAI key", Description = "Updates an existing OpenAI key.")]
    [SwaggerResponse(200, "OpenAI key updated successfully", typeof(OpenaiKeyDto))]
    [SwaggerResponse(400, "Invalid request data or key already exists")]
    [SwaggerResponse(404, "OpenAI key not found")]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    [SwaggerResponse(403, "Forbidden - admin role required")]
    public async Task<ActionResult<OpenaiKeyDto>> UpdateAsync(int id, [FromBody] UpdateOpenaiKeyDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _openaiKeyService.UpdateAsync(id, updateDto, cancellationToken);
            return result == null 
                ? NotFound(new { message = $"OpenAI key with ID {id} not found." })
                : Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the OpenAI key.", details = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(UserRole.SuperAdmin)}")]
    [SwaggerOperation(Summary = "Delete OpenAI key", Description = "Deletes an OpenAI key from the system.")]
    [SwaggerResponse(204, "OpenAI key deleted successfully")]
    [SwaggerResponse(404, "OpenAI key not found")]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    [SwaggerResponse(403, "Forbidden - admin role required")]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var success = await _openaiKeyService.DeleteAsync(id, cancellationToken);
        return success 
            ? NoContent() 
            : NotFound(new { message = $"OpenAI key with ID {id} not found." });
    }
}
