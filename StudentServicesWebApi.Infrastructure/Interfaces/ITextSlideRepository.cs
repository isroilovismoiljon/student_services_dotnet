using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

/// <summary>
/// Repository interface for TextSlide entity operations
/// </summary>
public interface ITextSlideRepository : IGenericRepository<TextSlide>
{
    /// <summary>
    /// Gets text slides by font family
    /// </summary>
    /// <param name="font">Font family name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slides using the specified font</returns>
    Task<List<TextSlide>> GetByFontAsync(string font, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides within a specific size range
    /// </summary>
    /// <param name="minSize">Minimum font size</param>
    /// <param name="maxSize">Maximum font size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slides within the size range</returns>
    Task<List<TextSlide>> GetBySizeRangeAsync(int minSize, int maxSize, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides by color
    /// </summary>
    /// <param name="colorHex">Hex color code</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slides with the specified color</returns>
    Task<List<TextSlide>> GetByColorAsync(string colorHex, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides with specific formatting (bold/italic)
    /// </summary>
    /// <param name="isBold">Filter by bold formatting</param>
    /// <param name="isItalic">Filter by italic formatting</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slides matching formatting criteria</returns>
    Task<List<TextSlide>> GetByFormattingAsync(bool? isBold = null, bool? isItalic = null, CancellationToken ct = default);

    /// <summary>
    /// Searches text slides by text content
    /// </summary>
    /// <param name="searchTerm">Text to search for</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slides containing the search term</returns>
    Task<List<TextSlide>> SearchByTextAsync(string searchTerm, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides within a specific position area
    /// </summary>
    /// <param name="minLeft">Minimum left position</param>
    /// <param name="maxLeft">Maximum left position</param>
    /// <param name="minTop">Minimum top position</param>
    /// <param name="maxTop">Maximum top position</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slides within the specified position area</returns>
    Task<List<TextSlide>> GetByPositionAreaAsync(double minLeft, double maxLeft, double minTop, double maxTop, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides with specific dimensions
    /// </summary>
    /// <param name="minWidth">Minimum width</param>
    /// <param name="maxWidth">Maximum width</param>
    /// <param name="minHeight">Minimum height (optional)</param>
    /// <param name="maxHeight">Maximum height (optional)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slides within the dimension criteria</returns>
    Task<List<TextSlide>> GetByDimensionsAsync(double? minWidth = null, double? maxWidth = null, double? minHeight = null, double? maxHeight = null, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides ordered by creation date
    /// </summary>
    /// <param name="ascending">True for ascending order, false for descending</param>
    /// <param name="pageNumber">Page number for pagination</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Tuple containing the text slides and total count</returns>
    Task<(List<TextSlide> TextSlides, int TotalCount)> GetPagedByCreationDateAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides ordered by update date
    /// </summary>
    /// <param name="ascending">True for ascending order, false for descending</param>
    /// <param name="pageNumber">Page number for pagination</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Tuple containing the text slides and total count</returns>
    Task<(List<TextSlide> TextSlides, int TotalCount)> GetPagedByUpdateDateAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);

    /// <summary>
    /// Checks if a text slide with the same content and position already exists
    /// </summary>
    /// <param name="text">Text content</param>
    /// <param name="left">Left position</param>
    /// <param name="top">Top position</param>
    /// <param name="excludeId">ID to exclude from the check (for updates)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if a duplicate exists, false otherwise</returns>
    Task<bool> ExistsDuplicateAsync(string text, double left, double top, int? excludeId = null, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides created within a specific date range
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slides created within the date range</returns>
    Task<List<TextSlide>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default);

    /// <summary>
    /// Bulk delete text slides by IDs
    /// </summary>
    /// <param name="ids">List of text slide IDs to delete</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Number of deleted text slides</returns>
    Task<int> BulkDeleteAsync(List<int> ids, CancellationToken ct = default);

    /// <summary>
    /// Gets text slide statistics
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Statistics about text slides</returns>
    Task<TextSlideStatsResult> GetStatsAsync(CancellationToken ct = default);
}

/// <summary>
/// Result class for text slide statistics
/// </summary>
public class TextSlideStatsResult
{
    public int TotalTextSlides { get; set; }
    public int UniqueFonts { get; set; }
    public int UniqueColors { get; set; }
    public int BoldTextSlides { get; set; }
    public int ItalicTextSlides { get; set; }
    public double AverageTextLength { get; set; }
    public int MinSize { get; set; }
    public int MaxSize { get; set; }
    public double AverageSize { get; set; }
    public DateTime? OldestCreated { get; set; }
    public DateTime? NewestCreated { get; set; }
}