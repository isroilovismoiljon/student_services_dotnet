using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Infrastructure.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Interfaces;
public interface ITextSlideService
{
    Task<TextSlideDto> CreateTextSlideAsync(CreateTextSlideDto createTextSlideDto, CancellationToken ct = default);
    Task<TextSlideDto?> UpdateTextSlideAsync(int id, UpdateTextSlideDto updateTextSlideDto, CancellationToken ct = default);
    Task<TextSlideDto?> GetTextSlideByIdAsync(int id, CancellationToken ct = default);
    Task<(List<TextSlideSummaryDto> TextSlides, int TotalCount)> GetPagedTextSlidesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
    Task<List<TextSlideSummaryDto>> SearchTextSlidesAsync(string searchTerm, CancellationToken ct = default);
    Task<bool> DeleteTextSlideAsync(int id, CancellationToken ct = default);
    Task<List<TextSlideDto>> BulkCreateTextSlidesAsync(BulkCreateTextSlideDto bulkCreateDto, CancellationToken ct = default);
    Task<int> BulkDeleteTextSlidesAsync(BulkTextSlideOperationDto bulkOperationDto, CancellationToken ct = default);
    Task<bool> ValidateTextSlideCreationAsync(CreateTextSlideDto createTextSlideDto, CancellationToken ct = default);
    Task<bool> ValidateTextSlideUpdateAsync(int id, UpdateTextSlideDto updateTextSlideDto, CancellationToken ct = default);
    Task<TextSlideStatsDto> GetTextSlideStatsAsync(CancellationToken ct = default);
    Task<bool> TextSlideExistsAsync(int id, CancellationToken ct = default);
    Task<List<TextSlideSummaryDto>> GetTextSlidesByPresentationPostIdAsync(int presentationPostId, CancellationToken ct = default);
}
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
