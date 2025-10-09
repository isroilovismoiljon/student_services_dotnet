namespace StudentServicesWebApi.Domain.Models;

public class PhotoSlide : BaseEntity
{
    public string PhotoPath { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public double Left { get; set; }
    public double Top { get; set; }
    public double Width { get; set; }
    public double? Height { get; set; }
    public int? DesignId { get; set; }
}
