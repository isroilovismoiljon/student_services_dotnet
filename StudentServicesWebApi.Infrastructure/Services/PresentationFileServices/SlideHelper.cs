using GemBox.Presentation;
using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Infrastructure.Services.PresentationFileServices;
public static class SlideHelper
{
    public static void AddTextToSlide(Slide slide, TextSlide textSlide)
    {
        var shape = TextHelper.AddTextBox(slide, textSlide.Left, textSlide.Top, textSlide.Width, textSlide.Height);
        var paragraph = shape.Text.AddParagraph();
        paragraph.Format.Alignment = textSlide.Horizontal;
        shape.Text.Format.VerticalAlignment = textSlide.Vertical;
        var textRun = paragraph.AddRun(textSlide.Text);
        TextHelper.AddTextStyle(textRun, textSlide: textSlide);
    }
    public static void AddPhotoToSlide(Slide slide, PhotoSlide photoSlide)
    {
        double ratio = ImageHelper.GetImageAspectRatio(photoSlide.PhotoPath);
        Picture? picture2 = null;
        double myWidth = photoSlide.Width;
        double myHeight;
        if (photoSlide.Height.HasValue && photoSlide.Height.Value > photoSlide.Width)
        {
            double limitedHeight = Math.Min(photoSlide.Height.Value, 10); 
            myHeight = limitedHeight;
            myWidth = myHeight * ratio;
        }
        else
        {
            myHeight = photoSlide.Height ?? photoSlide.Width / ratio;
        }
        using (var stream = File.OpenRead(photoSlide.PhotoPath))
            picture2 = slide.Content.AddPicture(PictureContentType.Png, stream,
            photoSlide.Left, photoSlide.Top, myWidth, myHeight, LengthUnit.Centimeter);
    }
    public static void AddBackgroundPhotoToSlide(Slide slide, PhotoSlide backgroundPhoto)
    {
        double ratio = ImageHelper.GetImageAspectRatio(backgroundPhoto.PhotoPath);
        Picture picture2 = null;
        using (var stream = File.OpenRead(backgroundPhoto.PhotoPath))
            picture2 = slide.Content.AddPicture(PictureContentType.Png, stream,
                backgroundPhoto.Left, backgroundPhoto.Top, backgroundPhoto.Width, backgroundPhoto.Height ?? 19.05, LengthUnit.Centimeter);
    }
}
