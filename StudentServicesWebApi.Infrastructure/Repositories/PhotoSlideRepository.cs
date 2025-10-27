using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Repositories;
public class PhotoSlideRepository : GenericRepository<PhotoSlide>, IPhotoSlideRepository
{
    public PhotoSlideRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<List<PhotoSlide>> GetByFileExtensionAsync(string extension, CancellationToken ct = default)
    {
        // Normalize extension to include the dot
        if (!extension.StartsWith("."))
            extension = "." + extension;

        return await _context.Set<PhotoSlide>()
            .Where(ps => ps.PhotoPath.ToLower().EndsWith(extension.ToLower()))
            .OrderBy(ps => ps.CreatedAt)
            .ToListAsync(ct);
    }
    public async Task<List<PhotoSlide>> GetByFileSizeRangeAsync(long minSize, long maxSize, CancellationToken ct = default)
    {
        return await _context.Set<PhotoSlide>()
            .Where(ps => ps.FileSize >= minSize && ps.FileSize <= maxSize)
            .OrderBy(ps => ps.FileSize)
            .ToListAsync(ct);
    }
    public async Task<List<PhotoSlide>> GetByPositionAreaAsync(double minLeft, double maxLeft, double minTop, double maxTop, CancellationToken ct = default)
    {
        return await _context.Set<PhotoSlide>()
            .Where(ps => ps.Left >= minLeft && ps.Left <= maxLeft &&
                         ps.Top >= minTop && ps.Top <= maxTop)
            .OrderBy(ps => ps.Left)
            .ThenBy(ps => ps.Top)
            .ToListAsync(ct);
    }
    public async Task<List<PhotoSlide>> GetByDimensionsAsync(double? minWidth = null, double? maxWidth = null, double? minHeight = null, double? maxHeight = null, CancellationToken ct = default)
    {
        var query = _context.Set<PhotoSlide>().AsQueryable();

        if (minWidth.HasValue)
        {
            query = query.Where(ps => ps.Width >= minWidth.Value);
        }

        if (maxWidth.HasValue)
        {
            query = query.Where(ps => ps.Width <= maxWidth.Value);
        }

        if (minHeight.HasValue)
        {
            query = query.Where(ps => ps.Height >= minHeight.Value);
        }

        if (maxHeight.HasValue)
        {
            query = query.Where(ps => ps.Height <= maxHeight.Value);
        }

        return await query
            .OrderBy(ps => ps.Width)
            .ThenBy(ps => ps.Height)
            .ToListAsync(ct);
    }

    public async Task<(List<PhotoSlide> PhotoSlides, int TotalCount)> GetPagedByCreationDateAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var query = _context.Set<PhotoSlide>().AsQueryable();

        query = ascending 
            ? query.OrderBy(ps => ps.CreatedAt)
            : query.OrderByDescending(ps => ps.CreatedAt);

        var totalCount = await query.CountAsync(ct);

        var photoSlides = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (photoSlides, totalCount);
    }

    public async Task<(List<PhotoSlide> PhotoSlides, int TotalCount)> GetPagedByUpdateDateAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var query = _context.Set<PhotoSlide>().AsQueryable();

        query = ascending 
            ? query.OrderBy(ps => ps.UpdatedAt)
            : query.OrderByDescending(ps => ps.UpdatedAt);

        var totalCount = await query.CountAsync(ct);

        var photoSlides = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (photoSlides, totalCount);
    }

    public async Task<(List<PhotoSlide> PhotoSlides, int TotalCount)> GetPagedByFileSizeAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var query = _context.Set<PhotoSlide>().AsQueryable();

        query = ascending 
            ? query.OrderBy(ps => ps.FileSize)
            : query.OrderByDescending(ps => ps.FileSize);

        var totalCount = await query.CountAsync(ct);

        var photoSlides = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (photoSlides, totalCount);
    }

    public async Task<List<PhotoSlide>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        return await _context.Set<PhotoSlide>()
            .Where(ps => ps.CreatedAt >= startDate && ps.CreatedAt <= endDate)
            .OrderBy(ps => ps.CreatedAt)
            .ToListAsync(ct);
    }
    public async Task<List<PhotoSlide>> GetOrphanedPhotoSlidesAsync(CancellationToken ct = default)
    {
        var photoSlides = await _context.Set<PhotoSlide>().ToListAsync(ct);
        var orphanedSlides = new List<PhotoSlide>();

        foreach (var slide in photoSlides)
        {
            if (!string.IsNullOrEmpty(slide.PhotoPath) && !System.IO.File.Exists(slide.PhotoPath))
            {
                orphanedSlides.Add(slide);
            }
        }

        return orphanedSlides;
    }
    public async Task<int> BulkDeleteAsync(List<int> ids, bool deleteFiles = true, CancellationToken ct = default)
    {
        if (!ids.Any())
        {
            return 0;
        }

        var photoSlidesToDelete = await _context.Set<PhotoSlide>()
            .Where(ps => ids.Contains(ps.Id))
            .ToListAsync(ct);

        if (photoSlidesToDelete.Any())
        {
            if (deleteFiles)
            {
                // Delete physical files
                foreach (var slide in photoSlidesToDelete)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(slide.PhotoPath) && System.IO.File.Exists($"wwwroot/uploads/presentation-files/{slide.PhotoPath}"))
                        {
                            System.IO.File.Delete($"wwwroot/uploads/presentation-files/{slide.PhotoPath}");
                        }
                    }
                    catch { }
                }
            }

            _context.Set<PhotoSlide>().RemoveRange(photoSlidesToDelete);
            await _context.SaveChangesAsync(ct);
        }

        return photoSlidesToDelete.Count;
    }
    public async Task<PhotoSlideStatsResult> GetStatsAsync(CancellationToken ct = default)
    {
        var photoSlides = await _context.Set<PhotoSlide>().ToListAsync(ct);

        if (!photoSlides.Any())
        {
            return new PhotoSlideStatsResult();
        }

        // Calculate file extension counts
        var extensionCounts = photoSlides
            .GroupBy(ps => Path.GetExtension(ps.PhotoPath).ToLowerInvariant())
            .ToDictionary(g => g.Key, g => g.Count());

        // Calculate content type counts
        var contentTypeCounts = photoSlides
            .GroupBy(ps => ps.ContentType)
            .ToDictionary(g => g.Key, g => g.Count());

        // Check for orphaned files
        var orphanedCount = 0;
        foreach (var slide in photoSlides)
        {
            if (!string.IsNullOrEmpty(slide.PhotoPath) && !File.Exists(slide.PhotoPath))
            {
                orphanedCount++;
            }
        }

        var totalFileSize = photoSlides.Sum(ps => ps.FileSize);
        var averageFileSize = photoSlides.Average(ps => ps.FileSize);
        var largestFileSize = photoSlides.Max(ps => ps.FileSize);
        var smallestFileSize = photoSlides.Min(ps => ps.FileSize);

        return new PhotoSlideStatsResult
        {
            TotalPhotoSlides = photoSlides.Count,
            TotalFileSize = totalFileSize,
            TotalFileSizeFormatted = FormatFileSize(totalFileSize),
            AverageFileSize = averageFileSize,
            AverageFileSizeFormatted = FormatFileSize((long)averageFileSize),
            LargestFileSize = largestFileSize,
            LargestFileSizeFormatted = FormatFileSize(largestFileSize),
            SmallestFileSize = smallestFileSize,
            SmallestFileSizeFormatted = FormatFileSize(smallestFileSize),
            UniqueFileExtensions = extensionCounts.Count,
            FileExtensionCounts = extensionCounts,
            ContentTypeCounts = contentTypeCounts,
            AverageWidth = photoSlides.Average(ps => ps.Width),
            AverageHeight = photoSlides.Where(ps => ps.Height.HasValue).Average(ps => ps.Height!.Value),
            OrphanedFiles = orphanedCount,
            OldestCreated = photoSlides.Min(ps => ps.CreatedAt),
            NewestCreated = photoSlides.Max(ps => ps.CreatedAt),
            MostCommonExtension = extensionCounts.OrderByDescending(kv => kv.Value).FirstOrDefault().Key ?? string.Empty,
            MostCommonContentType = contentTypeCounts.OrderByDescending(kv => kv.Value).FirstOrDefault().Key ?? string.Empty
        };
    }
    public async Task<List<PhotoSlide>> GetByContentTypeAsync(string contentType, CancellationToken ct = default)
    {
        return await _context.Set<PhotoSlide>()
            .Where(ps => ps.ContentType.ToLower() == contentType.ToLower())
            .OrderBy(ps => ps.CreatedAt)
            .ToListAsync(ct);
    }
    public async Task<List<PhotoSlide>> GetLargestPhotoSlidesAsync(int count = 10, CancellationToken ct = default)
    {
        return await _context.Set<PhotoSlide>()
            .OrderByDescending(ps => ps.FileSize)
            .Take(count)
            .ToListAsync(ct);
    }
    public async Task<List<PhotoSlide>> GetDuplicatePhotoSlidesAsync(CancellationToken ct = default)
    {
        var duplicateGroups = await _context.Set<PhotoSlide>()
            .GroupBy(ps => ps.PhotoPath)
            .Where(g => g.Count() > 1)
            .ToListAsync(ct);

        var duplicateSlides = new List<PhotoSlide>();
        foreach (var group in duplicateGroups)
        {
            duplicateSlides.AddRange(group);
        }

        return duplicateSlides.OrderBy(ps => ps.PhotoPath).ToList();
    }

    public async Task<bool> UpdatePhotoPathAsync(int id, string newPhotoPath, CancellationToken ct = default)
    {
        var photoSlide = await _context.Set<PhotoSlide>()
            .FirstOrDefaultAsync(ps => ps.Id == id, ct);

        if (photoSlide == null)
        {
            return false;
        }

        photoSlide.PhotoPath = newPhotoPath;
        photoSlide.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(ct);
        return true;
    }

    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        
        return $"{len:0.##} {sizes[order]}";
    }
}