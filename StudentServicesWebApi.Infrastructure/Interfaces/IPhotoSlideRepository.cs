using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

/// <summary>
/// Repository interface for PhotoSlide entity operations
/// </summary>
public interface IPhotoSlideRepository : IGenericRepository<PhotoSlide>
{
    /// <summary>
    /// Gets photo slides by file extension
    /// </summary>
    /// <param name="extension">File extension (e.g., ".jpg", ".png")</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slides with the specified file extension</returns>
    Task<List<PhotoSlide>> GetByFileExtensionAsync(string extension, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides within a specific file size range
    /// </summary>
    /// <param name="minSize">Minimum file size in bytes</param>
    /// <param name="maxSize">Maximum file size in bytes</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slides within the size range</returns>
    Task<List<PhotoSlide>> GetByFileSizeRangeAsync(long minSize, long maxSize, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides within a specific position area
    /// </summary>
    /// <param name="minLeft">Minimum left position</param>
    /// <param name="maxLeft">Maximum left position</param>
    /// <param name="minTop">Minimum top position</param>
    /// <param name="maxTop">Maximum top position</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slides within the specified position area</returns>
    Task<List<PhotoSlide>> GetByPositionAreaAsync(double minLeft, double maxLeft, double minTop, double maxTop, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides with specific dimensions
    /// </summary>
    /// <param name="minWidth">Minimum width (optional)</param>
    /// <param name="maxWidth">Maximum width (optional)</param>
    /// <param name="minHeight">Minimum height (optional)</param>
    /// <param name="maxHeight">Maximum height (optional)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slides within the dimension criteria</returns>
    Task<List<PhotoSlide>> GetByDimensionsAsync(double? minWidth = null, double? maxWidth = null, double? minHeight = null, double? maxHeight = null, CancellationToken ct = default);

    /// <summary>
    /// Searches photo slides by filename pattern
    /// </summary>
    /// <param name="pattern">Filename pattern to search for</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slides matching the filename pattern</returns>
    Task<List<PhotoSlide>> SearchByFilenameAsync(string pattern, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides ordered by creation date
    /// </summary>
    /// <param name="ascending">True for ascending order, false for descending</param>
    /// <param name="pageNumber">Page number for pagination</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Tuple containing the photo slides and total count</returns>
    Task<(List<PhotoSlide> PhotoSlides, int TotalCount)> GetPagedByCreationDateAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides ordered by update date
    /// </summary>
    /// <param name="ascending">True for ascending order, false for descending</param>
    /// <param name="pageNumber">Page number for pagination</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Tuple containing the photo slides and total count</returns>
    Task<(List<PhotoSlide> PhotoSlides, int TotalCount)> GetPagedByUpdateDateAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides ordered by file size
    /// </summary>
    /// <param name="ascending">True for ascending order, false for descending</param>
    /// <param name="pageNumber">Page number for pagination</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Tuple containing the photo slides and total count</returns>
    Task<(List<PhotoSlide> PhotoSlides, int TotalCount)> GetPagedByFileSizeAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);

    /// <summary>
    /// Checks if a photo slide with the same position already exists
    /// </summary>
    /// <param name="left">Left position</param>
    /// <param name="top">Top position</param>
    /// <param name="excludeId">ID to exclude from the check (for updates)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if a duplicate position exists, false otherwise</returns>
    Task<bool> ExistsDuplicatePositionAsync(double left, double top, int? excludeId = null, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides created within a specific date range
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slides created within the date range</returns>
    Task<List<PhotoSlide>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides that have orphaned files (files that don't exist on disk)
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slides with missing files</returns>
    Task<List<PhotoSlide>> GetOrphanedPhotoSlidesAsync(CancellationToken ct = default);

    /// <summary>
    /// Bulk delete photo slides by IDs
    /// </summary>
    /// <param name="ids">List of photo slide IDs to delete</param>
    /// <param name="deleteFiles">Whether to also delete the physical files</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Number of deleted photo slides</returns>
    Task<int> BulkDeleteAsync(List<int> ids, bool deleteFiles = true, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slide statistics
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Statistics about photo slides</returns>
    Task<PhotoSlideStatsResult> GetStatsAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides by content type (MIME type)
    /// </summary>
    /// <param name="contentType">MIME type (e.g., "image/jpeg", "image/png")</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slides with the specified content type</returns>
    Task<List<PhotoSlide>> GetByContentTypeAsync(string contentType, CancellationToken ct = default);

    /// <summary>
    /// Gets the largest photo slides by file size
    /// </summary>
    /// <param name="count">Number of photo slides to retrieve</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of the largest photo slides</returns>
    Task<List<PhotoSlide>> GetLargestPhotoSlidesAsync(int count = 10, CancellationToken ct = default);

    /// <summary>
    /// Gets duplicate photo slides based on file path
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slides that have duplicate file paths</returns>
    Task<List<PhotoSlide>> GetDuplicatePhotoSlidesAsync(CancellationToken ct = default);

    /// <summary>
    /// Updates the photo path for a photo slide
    /// </summary>
    /// <param name="id">Photo slide ID</param>
    /// <param name="newPhotoPath">New photo path</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if updated successfully, false if not found</returns>
    Task<bool> UpdatePhotoPathAsync(int id, string newPhotoPath, CancellationToken ct = default);
}

/// <summary>
/// Result class for photo slide statistics
/// </summary>
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