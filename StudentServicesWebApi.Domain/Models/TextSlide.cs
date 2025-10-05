namespace StudentServicesWebApi.Domain.Models;
using GemBox.Presentation;

public class TextSlide : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public int Size { get; set; }
    public string Font {  get; set; } = string.Empty;
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }
    public string ColorHex { get; set; } = string.Empty;

    public double Left { get; set; }
    public double Top { get; set; }
    public double Width { get; set; }
    public double? Height { get; set; }

    public HorizontalAlignment Horizontal { get; set; } = HorizontalAlignment.Left;
    public VerticalAlignment Vertical { get; set; } = VerticalAlignment.Top;
}
