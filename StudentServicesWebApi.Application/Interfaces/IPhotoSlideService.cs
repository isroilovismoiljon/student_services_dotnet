using Microsoft.AspNetCore.Http;
using StudentServicesWebApi.Application.DTOs.PhotoSlide;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface IPhotoSlideService
{
    Task<PhotoSlideDto> CreatePhotoSlideAsync(CreatePhotoSlideDto createPhotoSlideDto, CancellationToken ct = default);
    Task<PhotoSlideDto?> UpdatePhotoSlideAsync(int id, UpdatePhotoSlideDto updatePhotoSlideDto, CancellationToken ct = default);
    Task<PhotoSlideDto?> GetPhotoSlideByIdAsync(int id, CancellationToken ct = default);
    Task<(List<PhotoSlideSummaryDto> PhotoSlides, int TotalCount)> GetPagedPhotoSlidesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
    Task<bool> DeletePhotoSlideAsync(int id, bool deleteFile = true, CancellationToken ct = default);
    Task<BulkPhotoSlideUploadResultDto> BulkCreatePhotoSlidesAsync(BulkCreatePhotoSlideDto bulkCreateDto, CancellationToken ct = default);
    Task<int> BulkDeletePhotoSlidesAsync(BulkPhotoSlideOperationDto bulkOperationDto, bool deleteFiles = true, CancellationToken ct = default);
    Task<PhotoSlideStatsDto> GetPhotoSlideStatsAsync(CancellationToken ct = default);
    Task<bool> PhotoSlideExistsAsync(int id, CancellationToken ct = default);
    Task<List<PhotoSlideSummaryDto>> GetRecentPhotoSlidesAsync(int count = 10, CancellationToken ct = default);
    Task<PhotoSlideDto?> DuplicatePhotoSlideAsync(int id, double leftOffset = 10, double topOffset = 10, bool copyFile = true, CancellationToken ct = default);
    Task<PhotoSlideDto?> ReplacePhotoAsync(int id, IFormFile newPhoto, bool deleteOldFile = true, CancellationToken ct = default);
    Task<PhotoSlideDto> AddPhotoToDesignAsync(int designId, AddPhotoToDesignDto addPhotoToDesignDto, CancellationToken ct = default);
}

public class PhotoSlideStatsDto
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