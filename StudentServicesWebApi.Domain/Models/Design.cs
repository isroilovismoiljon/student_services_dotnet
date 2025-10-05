namespace StudentServicesWebApi.Domain.Models;

public class Design : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    
    // Navigation property
    public List<string> Photos { get; set; } = new();
}
