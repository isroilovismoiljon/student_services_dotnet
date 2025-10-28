using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using StudentServicesWebApi.Application.DTOs.Plan;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PlanController(IPlanService planService) : ControllerBase
{
    private readonly IPlanService _planService = planService;
    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.SuperAdmin)}")]
    [SwaggerOperation(Summary = "Create a new plan", Description = "Creates a new plan with PlanText and Plans slides.")]
    [SwaggerResponse(201, "Plan created successfully", typeof(PlanDto))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    [SwaggerResponse(403, "Forbidden - admin role required")]
    public async Task<ActionResult<PlanDto>> CreateAsync([FromBody] CreatePlanDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _planService.CreateAsync(createDto, cancellationToken);
            return Created($"/api/Plan/{result.Id}", result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the plan.", details = ex.Message });
        }
    }
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get plan by ID", Description = "Retrieves a specific plan by its unique identifier.")]
    [SwaggerResponse(200, "Plan found", typeof(PlanDto))]
    [SwaggerResponse(404, "Plan not found")]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    public async Task<ActionResult<PlanDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _planService.GetByIdAsync(id, cancellationToken);
        return result == null ? NotFound(new { message = $"Plan with ID {id} not found." }) : Ok(result);
    }
    [HttpGet]
    [SwaggerOperation(Summary = "Get all plans", Description = "Retrieves all plans in the system.")]
    [SwaggerResponse(200, "List of all plans", typeof(List<PlanDto>))]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    public async Task<ActionResult<List<PlanDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _planService.GetAllAsync(cancellationToken);
        return Ok(result);
    }
    [HttpGet("paged")]
    [SwaggerOperation(Summary = "Get paginated plans", Description = "Retrieves plans with pagination support.")]
    [SwaggerResponse(200, "Paginated list of plans")]
    [SwaggerResponse(400, "Invalid pagination parameters")]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    public async Task<ActionResult> GetPagedAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var (plans, totalCount) = await _planService.GetPagedAsync(pageNumber, pageSize, cancellationToken);
            var response = new
            {
                Data = plans,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving paginated plans.", details = ex.Message });
        }
    }
    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.SuperAdmin)}")]
    [SwaggerOperation(Summary = "Update plan", Description = "Updates an existing plan.")]
    [SwaggerResponse(200, "Plan updated successfully", typeof(PlanDto))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(404, "Plan not found")]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    [SwaggerResponse(403, "Forbidden - admin role required")]
    public async Task<ActionResult<PlanDto>> UpdateAsync(int id, [FromBody] UpdatePlanDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _planService.UpdateAsync(id, updateDto, cancellationToken);
            return result == null 
                ? NotFound(new { message = $"Plan with ID {id} not found." })
                : Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the plan.", details = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.SuperAdmin)}")]
    [SwaggerOperation(Summary = "Delete plan", Description = "Deletes a plan from the system.")]
    [SwaggerResponse(204, "Plan deleted successfully")]
    [SwaggerResponse(404, "Plan not found")]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    [SwaggerResponse(403, "Forbidden - admin role required")]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var success = await _planService.DeleteAsync(id, cancellationToken);
        return success 
            ? NoContent() 
            : NotFound(new { message = $"Plan with ID {id} not found." });
    }
    [HttpGet("exists/{id}")]
    [SwaggerOperation(Summary = "Check if plan exists", Description = "Checks if a plan exists by its ID.")]
    [SwaggerResponse(200, "Plan existence status")]
    [SwaggerResponse(401, "Unauthorized - authentication required")]
    public async Task<ActionResult> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _planService.ExistsAsync(id, cancellationToken);
        return Ok(new { exists, id });
    }
}
