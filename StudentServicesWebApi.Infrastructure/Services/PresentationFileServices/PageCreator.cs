using GemBox.Presentation;
using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Infrastructure.Services.PresentationFileServices;
public static class PageCreator
{
    public static void CreatePage(PresentationDocument presentation, PresentationPage page, int index, string backgroudImage, string imagePath = "images/presentationId/image-1.jpg")
    {
        var slide = presentation.Slides.AddNew(SlideLayoutType.Custom);
        var photo = page.Photo;
        var backgroundPhoto = page.BackgroundPhoto;
        var title = page.PresentationPosts[index].Title;
        var text = page.PresentationPosts[index].Text;
        SlideHelper.AddBackgroundPhotoToSlide(slide, backgroundPhoto);
        if(photo is null)
        {
            for (int i = 0; i < index; i++)
            {
                if (title is not null)
                {
                    SlideHelper.AddTextToSlide(slide, title);
                    SlideHelper.AddTextToSlide(slide, text);
                }
                else
                {
                    SlideHelper.AddTextToSlide(slide, text);
                }
            }
        }
        else
        {
            SlideHelper.AddPhotoToSlide(slide, photo);
            for (int i = 0; i < index; i++)
            {
                if(title is not null)
                {
                    SlideHelper.AddTextToSlide(slide, title);
                    SlideHelper.AddTextToSlide(slide, text);
                }else
                {
                    SlideHelper.AddTextToSlide(slide, text);
                }
            }
        }
    }
}
