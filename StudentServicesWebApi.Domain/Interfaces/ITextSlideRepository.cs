using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface ITextSlideRepository : IGenericRepository<TextSlide>
{
    Task<List<TextSlide>> SearchByTextAsync(string searchTerm, CancellationToken ct = default);
    Task<(List<TextSlide> TextSlides, int TotalCount)> GetPagedByCreationDateAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
    Task<bool> ExistsDuplicateAsync(string text, double left, double top, int? excludeId = null, CancellationToken ct = default);
    Task<int> BulkDeleteAsync(List<int> ids, CancellationToken ct = default);
    Task<TextSlideStatsResult> GetStatsAsync(CancellationToken ct = default);
    Task<List<TextSlide>> GetByPresentationPostIdAsync(int presentationPostId, CancellationToken ct = default);
}
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
