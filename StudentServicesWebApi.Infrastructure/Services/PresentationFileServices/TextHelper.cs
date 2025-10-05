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

        shape.Format.Fill.SetNone(); // fonni o‘chiradi
        shape.Format.Outline.Fill.SetNone(); // borderni o‘chiradi
        return shape;
    }

    //public static void AddTextStyle(TextRun text, int fontSize = 16, bool bold = false, bool italic = false,
    //    string font = "Almonte Snow", string color = "#1A1A1A")

    public static void AddTextStyle(TextRun text, TextSlide textSlide)
    {
        text.Format.Size = textSlide.Size;
        text.Format.Bold = textSlide.IsBold;
        text.Format.Italic = textSlide.IsItalic;
        text.Format.Font = textSlide.Font;
        text.Format.Fill.SetSolid(Color.FromHexString(textSlide.ColorHex));
    }
}
