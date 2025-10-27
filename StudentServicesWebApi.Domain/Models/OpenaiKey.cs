using System.ComponentModel.DataAnnotations;

namespace StudentServicesWebApi.Domain.Models;

public class OpenaiKey : BaseEntity
{
    public required string Key { get; set; } = string.Empty;
    public int UseCount { get; set; } = 0;
}
