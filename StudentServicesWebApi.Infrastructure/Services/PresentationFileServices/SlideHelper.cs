using GemBox.Presentation;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Services.PresentationFileServices;
public static class SlideHelper
{
    //public static void AddTextToSlide(Slide slide, string text, double left, double top, double width, double height,
    //    int fontSize = 16, bool bold = false, bool italic = false,
    //    string font = "Almonte Snow", string color = "#1A1A1A", HorizontalAlignment alignment = HorizontalAlignment.Left, 
    //    VerticalAlignment verticalAlignment = VerticalAlignment.Top)
    public static void AddTextToSlide(Slide slide, TextSlide textSlide)
    {
        var shape = TextHelper.AddTextBox(slide, textSlide.Left, textSlide.Top, textSlide.Width, textSlide.Height);

        var paragraph = shape.Text.AddParagraph();
        paragraph.Format.Alignment = textSlide.Horizontal;

        // Vertikal alignment (TextBox bo‘yicha)
        shape.Text.Format.VerticalAlignment = textSlide.Vertical;

        var textRun = paragraph.AddRun(textSlide.Text);

        //TextHelper.AddTextStyle(textRun, fontSize: fontSize, bold: bold, italic: italic, font: font, color: color);
        TextHelper.AddTextStyle(textRun, textSlide: textSlide);
    }


    public static void AddPhotoToSlide(Slide slide, PhotoSlide photoSlide)
    {
        // Rasm nisbatini olish
        double ratio = ImageHelper.GetImageAspectRatio(photoSlide.PhotoPath);
        // Create first picture from resource data.
        Picture? picture2 = null;

        double myWidth = photoSlide.Width;
        double myHeight;

        if (photoSlide.Height.HasValue && photoSlide.Height.Value > photoSlide.Width)
        {
            // height null emas va width dan katta bo'lsa
            double limitedHeight = Math.Min(photoSlide.Height.Value, 10); // maximum 10
            myHeight = limitedHeight;
            myWidth = myHeight * ratio;
        }
        else
        {
            // Eski logika
            myHeight = photoSlide.Height ?? photoSlide.Width / ratio;
        }
        using (var stream = File.OpenRead(photoSlide.PhotoPath))
            picture2 = slide.Content.AddPicture(PictureContentType.Png, stream,
            photoSlide.Left, photoSlide.Top, myWidth, myHeight, LengthUnit.Centimeter);
    }

    public static void AddBackgroundPhotoToSlide(Slide slide, PhotoSlide backgroundPhoto)
    {
        // Rasm nisbatini olish
        double ratio = ImageHelper.GetImageAspectRatio(backgroundPhoto.PhotoPath);

        // Create first picture from resource data.
        Picture picture2 = null;
        using (var stream = File.OpenRead(backgroundPhoto.PhotoPath))
            picture2 = slide.Content.AddPicture(PictureContentType.Png, stream,
                backgroundPhoto.Left, backgroundPhoto.Top, backgroundPhoto.Width, backgroundPhoto.Height ?? 19.05, LengthUnit.Centimeter);
    }

}
