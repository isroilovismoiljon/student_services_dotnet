using Microsoft.AspNetCore.Http;
using StudentServicesWebApi.Application.DTOs.PhotoSlide;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

/// <summary>
/// Service interface for PhotoSlide business logic operations
/// </summary>
public interface IPhotoSlideService
{
    /// <summary>
    /// Creates a new photo slide with file upload
    /// </summary>
    /// <param name="createPhotoSlideDto">Photo slide creation data with file</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Created photo slide DTO</returns>
    Task<PhotoSlideDto> CreatePhotoSlideAsync(CreatePhotoSlideDto createPhotoSlideDto, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing photo slide
    /// </summary>
    /// <param name="id">Photo slide ID</param>
    /// <param name="updatePhotoSlideDto">Photo slide update data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated photo slide DTO or null if not found</returns>
    Task<PhotoSlideDto?> UpdatePhotoSlideAsync(int id, UpdatePhotoSlideDto updatePhotoSlideDto, CancellationToken ct = default);

    /// <summary>
    /// Gets a photo slide by ID
    /// </summary>
    /// <param name="id">Photo slide ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Photo slide DTO or null if not found</returns>
    Task<PhotoSlideDto?> GetPhotoSlideByIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Gets all photo slides with pagination
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Paginated photo slides and total count</returns>
    Task<(List<PhotoSlideSummaryDto> PhotoSlides, int TotalCount)> GetPagedPhotoSlidesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides by file extension
    /// </summary>
    /// <param name="extension">File extension (e.g., "jpg", "png")</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByExtensionAsync(string extension, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides within a specific file size range
    /// </summary>
    /// <param name="minSize">Minimum file size in bytes</param>
    /// <param name="maxSize">Maximum file size in bytes</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByFileSizeRangeAsync(long minSize, long maxSize, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides by content type
    /// </summary>
    /// <param name="contentType">MIME type (e.g., "image/jpeg")</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByContentTypeAsync(string contentType, CancellationToken ct = default);

    /// <summary>
    /// Searches photo slides by filename pattern
    /// </summary>
    /// <param name="pattern">Filename pattern to search for</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> SearchPhotoSlidesByFilenameAsync(string pattern, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides within a specific position area
    /// </summary>
    /// <param name="minLeft">Minimum left position</param>
    /// <param name="maxLeft">Maximum left position</param>
    /// <param name="minTop">Minimum top position</param>
    /// <param name="maxTop">Maximum top position</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByPositionAreaAsync(double minLeft, double maxLeft, double minTop, double maxTop, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides with specific dimensions
    /// </summary>
    /// <param name="minWidth">Minimum width (optional)</param>
    /// <param name="maxWidth">Maximum width (optional)</param>
    /// <param name="minHeight">Minimum height (optional)</param>
    /// <param name="maxHeight">Maximum height (optional)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByDimensionsAsync(double? minWidth = null, double? maxWidth = null, double? minHeight = null, double? maxHeight = null, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides created within a specific date range
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default);

    /// <summary>
    /// Gets the largest photo slides by file size
    /// </summary>
    /// <param name="count">Number of photo slides to retrieve</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of the largest photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> GetLargestPhotoSlidesAsync(int count = 10, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides that have orphaned files (files that don't exist on disk)
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of photo slide summaries with missing files</returns>
    Task<List<PhotoSlideSummaryDto>> GetOrphanedPhotoSlidesAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets duplicate photo slides based on file path
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of duplicate photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> GetDuplicatePhotoSlidesAsync(CancellationToken ct = default);

    /// <summary>
    /// Deletes a photo slide by ID
    /// </summary>
    /// <param name="id">Photo slide ID</param>
    /// <param name="deleteFile">Whether to also delete the physical file</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if deleted successfully, false if not found</returns>
    Task<bool> DeletePhotoSlideAsync(int id, bool deleteFile = true, CancellationToken ct = default);

    /// <summary>
    /// Bulk creates photo slides with multiple files
    /// </summary>
    /// <param name="bulkCreateDto">Bulk creation data with files</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Bulk upload result with individual results</returns>
    Task<BulkPhotoSlideUploadResultDto> BulkCreatePhotoSlidesAsync(BulkCreatePhotoSlideDto bulkCreateDto, CancellationToken ct = default);

    /// <summary>
    /// Bulk deletes photo slides
    /// </summary>
    /// <param name="bulkOperationDto">Bulk operation data</param>
    /// <param name="deleteFiles">Whether to also delete the physical files</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Number of deleted photo slides</returns>
    Task<int> BulkDeletePhotoSlidesAsync(BulkPhotoSlideOperationDto bulkOperationDto, bool deleteFiles = true, CancellationToken ct = default);

    /// <summary>
    /// Validates if a photo slide can be created (checks for position conflicts)
    /// </summary>
    /// <param name="createPhotoSlideDto">Photo slide creation data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if valid, false if position conflict exists</returns>
    Task<bool> ValidatePhotoSlideCreationAsync(CreatePhotoSlideDto createPhotoSlideDto, CancellationToken ct = default);

    /// <summary>
    /// Validates if a photo slide can be updated (checks for position conflicts)
    /// </summary>
    /// <param name="id">Photo slide ID being updated</param>
    /// <param name="updatePhotoSlideDto">Photo slide update data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if valid, false if position conflict exists</returns>
    Task<bool> ValidatePhotoSlideUpdateAsync(int id, UpdatePhotoSlideDto updatePhotoSlideDto, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slide statistics
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Photo slide statistics</returns>
    Task<PhotoSlideStatsDto> GetPhotoSlideStatsAsync(CancellationToken ct = default);

    /// <summary>
    /// Checks if a photo slide exists by ID
    /// </summary>
    /// <param name="id">Photo slide ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> PhotoSlideExistsAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Gets the most recently created photo slides
    /// </summary>
    /// <param name="count">Number of photo slides to retrieve</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of recent photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> GetRecentPhotoSlidesAsync(int count = 10, CancellationToken ct = default);

    /// <summary>
    /// Gets the most recently updated photo slides
    /// </summary>
    /// <param name="count">Number of photo slides to retrieve</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of recently updated photo slide summaries</returns>
    Task<List<PhotoSlideSummaryDto>> GetRecentlyUpdatedPhotoSlidesAsync(int count = 10, CancellationToken ct = default);

    /// <summary>
    /// Duplicates a photo slide with optional position offset
    /// </summary>
    /// <param name="id">ID of the photo slide to duplicate</param>
    /// <param name="leftOffset">Offset for left position</param>
    /// <param name="topOffset">Offset for top position</param>
    /// <param name="copyFile">Whether to create a copy of the physical file</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Duplicated photo slide DTO or null if original not found</returns>
    Task<PhotoSlideDto?> DuplicatePhotoSlideAsync(int id, double leftOffset = 10, double topOffset = 10, bool copyFile = true, CancellationToken ct = default);

    /// <summary>
    /// Replaces the photo of an existing photo slide
    /// </summary>
    /// <param name="id">Photo slide ID</param>
    /// <param name="newPhoto">New photo file</param>
    /// <param name="deleteOldFile">Whether to delete the old photo file</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated photo slide DTO or null if not found</returns>
    Task<PhotoSlideDto?> ReplacePhotoAsync(int id, IFormFile newPhoto, bool deleteOldFile = true, CancellationToken ct = default);

    /// <summary>
    /// Gets photo slides ordered by file size
    /// </summary>
    /// <param name="ascending">True for ascending order, false for descending</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Paginated photo slides ordered by file size</returns>
    Task<(List<PhotoSlideSummaryDto> PhotoSlides, int TotalCount)> GetPhotoSlidesByFileSizeAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);

    /// <summary>
    /// Cleans up orphaned photo slides and optionally removes orphaned files
    /// </summary>
    /// <param name="removeOrphanedRecords">Whether to remove database records for orphaned files</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Number of cleaned up records</returns>
    Task<int> CleanupOrphanedPhotoSlidesAsync(bool removeOrphanedRecords = false, CancellationToken ct = default);
}

/// <summary>
/// DTO for photo slide statistics
/// </summary>
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