using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Repositories;
public class TextSlideRepository : GenericRepository<TextSlide>, ITextSlideRepository
{
    public TextSlideRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<List<TextSlide>> SearchByTextAsync(string searchTerm, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return new List<TextSlide>();
        }
        return await _context.Set<TextSlide>()
            .Where(ts => ts.Text.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(ts => ts.Text.Length) 
            .ThenBy(ts => ts.CreatedAt)
            .ToListAsync(ct);
    }
    public async Task<(List<TextSlide> TextSlides, int TotalCount)> GetPagedByCreationDateAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var query = _context.Set<TextSlide>().AsQueryable();
        query = ascending 
            ? query.OrderBy(ts => ts.CreatedAt)
            : query.OrderByDescending(ts => ts.CreatedAt);
        var totalCount = await query.CountAsync(ct);
        var textSlides = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        return (textSlides, totalCount);
    }
    public async Task<bool> ExistsDuplicateAsync(string text, double left, double top, int? excludeId = null, CancellationToken ct = default)
    {
        var query = _context.Set<TextSlide>()
            .Where(ts => ts.Text == text && ts.Left == left && ts.Top == top);
        if (excludeId.HasValue)
        {
            query = query.Where(ts => ts.Id != excludeId.Value);
        }
        return await query.AnyAsync(ct);
    }
    public async Task<int> BulkDeleteAsync(List<int> ids, CancellationToken ct = default)
    {
        if (!ids.Any())
        {
            return 0;
        }
        var textSlidesToDelete = await _context.Set<TextSlide>()
            .Where(ts => ids.Contains(ts.Id))
            .ToListAsync(ct);
        if (textSlidesToDelete.Any())
        {
            _context.Set<TextSlide>().RemoveRange(textSlidesToDelete);
            await _context.SaveChangesAsync(ct);
        }
        return textSlidesToDelete.Count;
    }
    public async Task<TextSlideStatsResult> GetStatsAsync(CancellationToken ct = default)
    {
        var textSlides = await _context.Set<TextSlide>().ToListAsync(ct);
        if (!textSlides.Any())
        {
            return new TextSlideStatsResult();
        }
        return new TextSlideStatsResult
        {
            TotalTextSlides = textSlides.Count,
            UniqueFonts = textSlides.Select(ts => ts.Font).Distinct().Count(),
            UniqueColors = textSlides.Select(ts => ts.ColorHex).Distinct().Count(),
            BoldTextSlides = textSlides.Count(ts => ts.IsBold),
            ItalicTextSlides = textSlides.Count(ts => ts.IsItalic),
            AverageTextLength = textSlides.Average(ts => ts.Text.Length),
            MinSize = textSlides.Min(ts => ts.Size),
            MaxSize = textSlides.Max(ts => ts.Size),
            AverageSize = textSlides.Average(ts => ts.Size),
            OldestCreated = textSlides.Min(ts => ts.CreatedAt),
            NewestCreated = textSlides.Max(ts => ts.CreatedAt)
        };
    }
    public async Task<List<TextSlide>> GetByPresentationPostIdAsync(int presentationPostId, CancellationToken ct = default)
    {
        return await _context.Set<TextSlide>()
            .Where(ts => _context.Set<PresentationPost>()
                .Any(pp => pp.Id == presentationPostId && (pp.TitleId == ts.Id || pp.TextId == ts.Id)))
            .OrderBy(ts => ts.CreatedAt)
            .ToListAsync(ct);
    }
}
