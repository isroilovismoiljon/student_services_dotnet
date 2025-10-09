using System.ComponentModel.DataAnnotations;

namespace StudentServicesWebApi.Domain.Models;

public class Design : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public int CreatedById { get; set; }
    
    // Navigation property
    public List<PhotoSlide> Photos { get; set; } = new();
    
    // CreatedBy entity
    public User CreatedBy { get; set; } = default!;
}
