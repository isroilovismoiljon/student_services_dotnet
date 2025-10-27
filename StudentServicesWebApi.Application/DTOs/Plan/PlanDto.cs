using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using StudentServicesWebApi.Application.DTOs.TextSlide;

namespace StudentServicesWebApi.Application.DTOs.Plan;

public class CreatePlanDto
{
    [Required(ErrorMessage = "PlanText is required")]
    [SwaggerSchema(Description = "The plan text slide")]
    public CreateTextSlideDto PlanText { get; set; } = default!;

    [Required(ErrorMessage = "Plans is required")]
    [SwaggerSchema(Description = "The plans content slide")]
    public CreateTextSlideDto Plans { get; set; } = default!;
}

public class UpdatePlanDto
{
    [SwaggerSchema(Description = "The plan text slide")]
    public UpdateTextSlideDto? PlanText { get; set; }

    [SwaggerSchema(Description = "The plans content slide")]
    public UpdateTextSlideDto? Plans { get; set; }
}

public class PlanDto
{
    [SwaggerSchema(Description = "Unique identifier for the plan")]
    public int Id { get; set; }

    [SwaggerSchema(Description = "The plan text slide")]
    public TextSlideDto PlanText { get; set; } = default!;

    [SwaggerSchema(Description = "The plans content slide")]
    public TextSlideDto Plans { get; set; } = default!;

    [SwaggerSchema(Description = "When the plan was created")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema(Description = "When the plan was last updated")]
    public DateTime UpdatedAt { get; set; }
}

public class PlanSummaryDto
{
    [SwaggerSchema(Description = "Unique identifier for the plan")]
    public int Id { get; set; }

    [SwaggerSchema(Description = "Preview of the plan text")]
    public string PlanTextPreview { get; set; } = default!;

    [SwaggerSchema(Description = "Preview of the plans content")]
    public string PlansPreview { get; set; } = default!;

    [SwaggerSchema(Description = "When the plan was created")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema(Description = "When the plan was last updated")]
    public DateTime UpdatedAt { get; set; }
}
