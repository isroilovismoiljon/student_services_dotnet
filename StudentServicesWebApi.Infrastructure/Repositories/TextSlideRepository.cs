using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for TextSlide entity operations
/// </summary>
public class TextSlideRepository : GenericRepository<TextSlide>, ITextSlideRepository
{
    public TextSlideRepository(AppDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<List<TextSlide>> GetByFontAsync(string font, CancellationToken ct = default)
    {
        return await _context.Set<TextSlide>()
            .Where(ts => ts.Font.ToLower() == font.ToLower())
            .OrderBy(ts => ts.CreatedAt)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<List<TextSlide>> GetBySizeRangeAsync(int minSize, int maxSize, CancellationToken ct = default)
    {
        return await _context.Set<TextSlide>()
            .Where(ts => ts.Size >= minSize && ts.Size <= maxSize)
            .OrderBy(ts => ts.Size)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<List<TextSlide>> GetByColorAsync(string colorHex, CancellationToken ct = default)
    {
        return await _context.Set<TextSlide>()
            .Where(ts => ts.ColorHex.ToLower() == colorHex.ToLower())
            .OrderBy(ts => ts.CreatedAt)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<List<TextSlide>> GetByFormattingAsync(bool? isBold = null, bool? isItalic = null, CancellationToken ct = default)
    {
        var query = _context.Set<TextSlide>().AsQueryable();

        if (isBold.HasValue)
        {
            query = query.Where(ts => ts.IsBold == isBold.Value);
        }

        if (isItalic.HasValue)
        {
            query = query.Where(ts => ts.IsItalic == isItalic.Value);
        }

        return await query
            .OrderBy(ts => ts.CreatedAt)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<List<TextSlide>> SearchByTextAsync(string searchTerm, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return new List<TextSlide>();
        }

        return await _context.Set<TextSlide>()
            .Where(ts => ts.Text.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(ts => ts.Text.Length) // Shorter matches first
            .ThenBy(ts => ts.CreatedAt)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<List<TextSlide>> GetByPositionAreaAsync(double minLeft, double maxLeft, double minTop, double maxTop, CancellationToken ct = default)
    {
        return await _context.Set<TextSlide>()
            .Where(ts => ts.Left >= minLeft && ts.Left <= maxLeft &&
                         ts.Top >= minTop && ts.Top <= maxTop)
            .OrderBy(ts => ts.Left)
            .ThenBy(ts => ts.Top)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<List<TextSlide>> GetByDimensionsAsync(double? minWidth = null, double? maxWidth = null, double? minHeight = null, double? maxHeight = null, CancellationToken ct = default)
    {
        var query = _context.Set<TextSlide>().AsQueryable();

        if (minWidth.HasValue)
        {
            query = query.Where(ts => ts.Width >= minWidth.Value);
        }

        if (maxWidth.HasValue)
        {
            query = query.Where(ts => ts.Width <= maxWidth.Value);
        }

        if (minHeight.HasValue)
        {
            query = query.Where(ts => ts.Height >= minHeight.Value);
        }

        if (maxHeight.HasValue)
        {
            query = query.Where(ts => ts.Height <= maxHeight.Value);
        }

        return await query
            .OrderBy(ts => ts.Width)
            .ThenBy(ts => ts.Height)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<(List<TextSlide> TextSlides, int TotalCount)> GetPagedByUpdateDateAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var query = _context.Set<TextSlide>().AsQueryable();

        query = ascending 
            ? query.OrderBy(ts => ts.UpdatedAt)
            : query.OrderByDescending(ts => ts.UpdatedAt);

        var totalCount = await query.CountAsync(ct);

        var textSlides = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (textSlides, totalCount);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<List<TextSlide>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        return await _context.Set<TextSlide>()
            .Where(ts => ts.CreatedAt >= startDate && ts.CreatedAt <= endDate)
            .OrderBy(ts => ts.CreatedAt)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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
}