using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

/// <summary>
/// Service interface for TextSlide business logic operations
/// </summary>
public interface ITextSlideService
{
    /// <summary>
    /// Creates a new text slide
    /// </summary>
    /// <param name="createTextSlideDto">Text slide creation data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Created text slide DTO</returns>
    Task<TextSlideDto> CreateTextSlideAsync(CreateTextSlideDto createTextSlideDto, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing text slide
    /// </summary>
    /// <param name="id">Text slide ID</param>
    /// <param name="updateTextSlideDto">Text slide update data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated text slide DTO or null if not found</returns>
    Task<TextSlideDto?> UpdateTextSlideAsync(int id, UpdateTextSlideDto updateTextSlideDto, CancellationToken ct = default);

    /// <summary>
    /// Gets a text slide by ID
    /// </summary>
    /// <param name="id">Text slide ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Text slide DTO or null if not found</returns>
    Task<TextSlideDto?> GetTextSlideByIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Gets all text slides with pagination
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Paginated text slides and total count</returns>
    Task<(List<TextSlideSummaryDto> TextSlides, int TotalCount)> GetPagedTextSlidesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides by font family
    /// </summary>
    /// <param name="font">Font family name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slide summaries</returns>
    Task<List<TextSlideSummaryDto>> GetTextSlidesByFontAsync(string font, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides within a specific size range
    /// </summary>
    /// <param name="minSize">Minimum font size</param>
    /// <param name="maxSize">Maximum font size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slide summaries</returns>
    Task<List<TextSlideSummaryDto>> GetTextSlidesBySizeRangeAsync(int minSize, int maxSize, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides by color
    /// </summary>
    /// <param name="colorHex">Hex color code</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slide summaries</returns>
    Task<List<TextSlideSummaryDto>> GetTextSlidesByColorAsync(string colorHex, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides with specific formatting
    /// </summary>
    /// <param name="isBold">Filter by bold formatting</param>
    /// <param name="isItalic">Filter by italic formatting</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slide summaries</returns>
    Task<List<TextSlideSummaryDto>> GetTextSlidesByFormattingAsync(bool? isBold = null, bool? isItalic = null, CancellationToken ct = default);

    /// <summary>
    /// Searches text slides by text content
    /// </summary>
    /// <param name="searchTerm">Text to search for</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slide summaries</returns>
    Task<List<TextSlideSummaryDto>> SearchTextSlidesAsync(string searchTerm, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides within a specific position area
    /// </summary>
    /// <param name="minLeft">Minimum left position</param>
    /// <param name="maxLeft">Maximum left position</param>
    /// <param name="minTop">Minimum top position</param>
    /// <param name="maxTop">Maximum top position</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slide summaries</returns>
    Task<List<TextSlideSummaryDto>> GetTextSlidesByPositionAreaAsync(double minLeft, double maxLeft, double minTop, double maxTop, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides with specific dimensions
    /// </summary>
    /// <param name="minWidth">Minimum width</param>
    /// <param name="maxWidth">Maximum width</param>
    /// <param name="minHeight">Minimum height (optional)</param>
    /// <param name="maxHeight">Maximum height (optional)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slide summaries</returns>
    Task<List<TextSlideSummaryDto>> GetTextSlidesByDimensionsAsync(double? minWidth = null, double? maxWidth = null, double? minHeight = null, double? maxHeight = null, CancellationToken ct = default);

    /// <summary>
    /// Gets text slides created within a specific date range
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of text slide summaries</returns>
    Task<List<TextSlideSummaryDto>> GetTextSlidesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default);

    /// <summary>
    /// Deletes a text slide by ID
    /// </summary>
    /// <param name="id">Text slide ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if deleted successfully, false if not found</returns>
    Task<bool> DeleteTextSlideAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Bulk creates text slides
    /// </summary>
    /// <param name="bulkCreateDto">Bulk creation data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of created text slide DTOs</returns>
    Task<List<TextSlideDto>> BulkCreateTextSlidesAsync(BulkCreateTextSlideDto bulkCreateDto, CancellationToken ct = default);

    /// <summary>
    /// Bulk deletes text slides
    /// </summary>
    /// <param name="bulkOperationDto">Bulk operation data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Number of deleted text slides</returns>
    Task<int> BulkDeleteTextSlidesAsync(BulkTextSlideOperationDto bulkOperationDto, CancellationToken ct = default);

    /// <summary>
    /// Validates if a text slide can be created (checks for duplicates)
    /// </summary>
    /// <param name="createTextSlideDto">Text slide creation data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if valid, false if duplicate exists</returns>
    Task<bool> ValidateTextSlideCreationAsync(CreateTextSlideDto createTextSlideDto, CancellationToken ct = default);

    /// <summary>
    /// Validates if a text slide can be updated (checks for duplicates)
    /// </summary>
    /// <param name="id">Text slide ID being updated</param>
    /// <param name="updateTextSlideDto">Text slide update data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if valid, false if duplicate exists</returns>
    Task<bool> ValidateTextSlideUpdateAsync(int id, UpdateTextSlideDto updateTextSlideDto, CancellationToken ct = default);

    /// <summary>
    /// Gets text slide statistics
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Text slide statistics</returns>
    Task<TextSlideStatsDto> GetTextSlideStatsAsync(CancellationToken ct = default);

    /// <summary>
    /// Checks if a text slide exists by ID
    /// </summary>
    /// <param name="id">Text slide ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> TextSlideExistsAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Gets the most recently created text slides
    /// </summary>
    /// <param name="count">Number of text slides to retrieve</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of recent text slide summaries</returns>
    Task<List<TextSlideSummaryDto>> GetRecentTextSlidesAsync(int count = 10, CancellationToken ct = default);

    /// <summary>
    /// Gets the most recently updated text slides
    /// </summary>
    /// <param name="count">Number of text slides to retrieve</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of recently updated text slide summaries</returns>
    Task<List<TextSlideSummaryDto>> GetRecentlyUpdatedTextSlidesAsync(int count = 10, CancellationToken ct = default);

    /// <summary>
    /// Duplicates a text slide with optional position offset
    /// </summary>
    /// <param name="id">ID of the text slide to duplicate</param>
    /// <param name="leftOffset">Offset for left position</param>
    /// <param name="topOffset">Offset for top position</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Duplicated text slide DTO or null if original not found</returns>
    Task<TextSlideDto?> DuplicateTextSlideAsync(int id, double leftOffset = 10, double topOffset = 10, CancellationToken ct = default);
}

/// <summary>
/// DTO for text slide statistics
/// </summary>
public class TextSlideStatsDto
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
    public string MostUsedFont { get; set; } = string.Empty;
    public string MostUsedColor { get; set; } = string.Empty;
}