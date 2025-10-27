using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace StudentServicesWebApi.Application.DTOs.OpenaiKey;

public class CreateOpenaiKeyDto
{
    [Required(ErrorMessage = "OpenAI API Key is required")]
    [StringLength(256, MinimumLength = 20, ErrorMessage = "Key must be between 20 and 256 characters")]
    [RegularExpression(@"^sk-[a-zA-Z0-9]{20,}$", ErrorMessage = "Key must start with 'sk-' followed by alphanumeric characters")]
    [SwaggerSchema(Description = "The OpenAI API key. Must start with 'sk-' and contain at least 20 characters total.")]
    public string Key { get; set; } = string.Empty;
}

public class UpdateOpenaiKeyDto
{
    [StringLength(256, MinimumLength = 20, ErrorMessage = "Key must be between 20 and 256 characters")]
    [RegularExpression(@"^sk-[a-zA-Z0-9]{20,}$", ErrorMessage = "Key must start with 'sk-' followed by alphanumeric characters")]
    [SwaggerSchema(Description = "The updated OpenAI API key. Must start with 'sk-' and contain at least 20 characters total.")]
    public string? Key { get; set; }

    [SwaggerSchema(Description = "Whether to reset the use count to zero when updating the key.")]
    public bool ResetUseCount { get; set; } = false;
}

public class OpenaiKeyDto
{
    [SwaggerSchema(Description = "Unique identifier for the OpenAI key.")]
    public int Id { get; set; }

    [SwaggerSchema(Description = "The OpenAI API key with middle characters masked for security.")]
    public string Key { get; set; } = string.Empty;

    [SwaggerSchema(Description = "Number of times this OpenAI key has been used for API calls.")]
    public int UseCount { get; set; }

    [SwaggerSchema(Description = "When the OpenAI key was created.")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema(Description = "When the OpenAI key was last updated.")]
    public DateTime UpdatedAt { get; set; }
}

public class OpenaiKeySummaryDto
{
    [SwaggerSchema(Description = "Unique identifier for the OpenAI key.")]
    public int Id { get; set; }

    [SwaggerSchema(Description = "The OpenAI API key with middle characters masked for security.")]
    public string KeyMasked { get; set; } = string.Empty;

    [SwaggerSchema(Description = "Number of times this OpenAI key has been used for API calls.")]
    public int UseCount { get; set; }

    [SwaggerSchema(Description = "Current status of the OpenAI key (Active, High Usage, etc.).")]
    public string Status { get; set; } = string.Empty;

    [SwaggerSchema(Description = "When the OpenAI key was created.")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema(Description = "When the OpenAI key was last updated.")]
    public DateTime UpdatedAt { get; set; }
}

public class BulkOpenaiKeyOperationDto
{
    [Required(ErrorMessage = "OpenaiKeyIds is required")]
    [MinLength(1, ErrorMessage = "At least one OpenAI key ID is required")]
    [SwaggerSchema(Description = "List of OpenAI key IDs to operate on. Must contain at least one ID.")]
    public List<int> OpenaiKeyIds { get; set; } = new();
}

public class BulkCreateOpenaiKeyDto
{
    [Required(ErrorMessage = "OpenaiKeys is required")]
    [MinLength(1, ErrorMessage = "At least one OpenAI key is required")]
    [SwaggerSchema(Description = "List of OpenAI keys to create in bulk. Must contain at least one key definition.")]
    public List<CreateOpenaiKeyDto> OpenaiKeys { get; set; } = new();
}

public class OpenaiKeyStatsDto
{
    [SwaggerSchema(Description = "Total number of OpenAI keys in the system.")]
    public int TotalKeys { get; set; }

    [SwaggerSchema(Description = "Number of OpenAI keys considered active (usage below threshold).")]
    public int ActiveKeys { get; set; }

    [SwaggerSchema(Description = "Number of OpenAI keys with high usage counts.")]
    public int HighUsageKeys { get; set; }

    [SwaggerSchema(Description = "Combined usage count across all OpenAI keys.")]
    public int TotalUsage { get; set; }

    [SwaggerSchema(Description = "Average usage count per OpenAI key.")]
    public double AverageUsage { get; set; }
}

public class IncrementUsageDto
{
    [Range(1, 1000, ErrorMessage = "Increment amount must be between 1 and 1000")]
    [SwaggerSchema(Description = "Amount to increment the usage count by. Must be between 1 and 1000.")]
    public int IncrementBy { get; set; } = 1;
}