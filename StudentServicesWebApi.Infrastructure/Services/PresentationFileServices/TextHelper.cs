using GemBox.Presentation;
using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Infrastructure.Services.PresentationFileServices;
public static class TextHelper
{
    public static Shape AddTextBox(Slide slide, double left, double top, double width, double? height)
    {
        var shape = slide.Content.AddShape(
            ShapeGeometryType.RoundedRectangle,
            left, top, width, height ?? 0, LengthUnit.Centimeter);
        shape.Format.Fill.SetNone(); 
        shape.Format.Outline.Fill.SetNone(); 
        return shape;
    }
    public static void AddTextStyle(TextRun text, TextSlide textSlide)
    {
        text.Format.Size = textSlide.Size;
        text.Format.Bold = textSlide.IsBold;
        text.Format.Italic = textSlide.IsItalic;
        text.Format.Font = textSlide.Font;
        text.Format.Fill.SetSolid(Color.FromHexString(textSlide.ColorHex));
    }
}
