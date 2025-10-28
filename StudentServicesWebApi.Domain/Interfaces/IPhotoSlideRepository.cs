using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface IPhotoSlideRepository : IGenericRepository<PhotoSlide>
{
    Task<(List<PhotoSlide> PhotoSlides, int TotalCount)> GetPagedByCreationDateAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
    Task<int> BulkDeleteAsync(List<int> ids, bool deleteFiles = true, CancellationToken ct = default);
    Task<PhotoSlideStatsResult> GetStatsAsync(CancellationToken ct = default);
    Task<bool> UpdatePhotoPathAsync(int id, string newPhotoPath, CancellationToken ct = default);
}
public class PhotoSlideStatsResult
{
    public int TotalPhotoSlides { get; set; }
    public long TotalFileSize { get; set; }
    public string TotalFileSizeFormatted { get; set; } = string.Empty;
    public double AverageFileSize { get; set; }
    public string AverageFileSizeFormatted { get; set; } = string.Empty;
    public long LargestFileSize { get; set; }
    public string LargestFileSizeFormatted { get; set; } = string.Empty;
    public long SmallestFileSize { get; set; }
    public string SmallestFileSizeFormatted { get; set; } = string.Empty;
    public int UniqueFileExtensions { get; set; }
    public Dictionary<string, int> FileExtensionCounts { get; set; } = new();
    public Dictionary<string, int> ContentTypeCounts { get; set; } = new();
    public double AverageWidth { get; set; }
    public double AverageHeight { get; set; }
    public int OrphanedFiles { get; set; }
    public DateTime? OldestCreated { get; set; }
    public DateTime? NewestCreated { get; set; }
    public string MostCommonExtension { get; set; } = string.Empty;
    public string MostCommonContentType { get; set; } = string.Empty;
}
