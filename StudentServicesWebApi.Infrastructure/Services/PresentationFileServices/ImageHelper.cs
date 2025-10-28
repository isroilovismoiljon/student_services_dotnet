namespace StudentServicesWebApi.Infrastructure.Services.PresentationFileServices;
public class ImageHelper
{
    private static readonly HttpClient _httpClient = new HttpClient();
    public static async Task DownloadImageAsync(string imageUrl, string imageName)
    {
        try
        {
            var response = await _httpClient.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();
            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            var fullPath = Path.GetDirectoryName($"images/presentationId/");
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            fullPath = $"{fullPath}/{imageName}";
            await File.WriteAllBytesAsync(fullPath, imageBytes);
            Console.WriteLine("Rasm muvaffaqiyatli yuklandi: " + fullPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Xato: " + ex.Message);
        }
    }
    public static double GetImageAspectRatio(string imagePath)
    {
        using (var image = System.Drawing.Image.FromFile(imagePath))
        {
            int width = image.Width;
            int height = image.Height;
            double ratio = (double)width / height;
            return ratio;
        }
    }
}
