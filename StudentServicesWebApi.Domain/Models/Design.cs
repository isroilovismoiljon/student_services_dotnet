using System.ComponentModel.DataAnnotations;
namespace StudentServicesWebApi.Domain.Models;
public class Design : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public int CreatedById { get; set; }
    public bool IsValid { get; set; } = false;
    public List<PhotoSlide> Photos { get; set; } = new();
    public User CreatedBy { get; set; } = default!;
}
