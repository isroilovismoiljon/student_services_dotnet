using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Repositories;

public class DesignRepository : GenericRepository<Design>, IDesignRepository
{
    private readonly IFileUploadService _fileUploadService;
    
    public DesignRepository(AppDbContext context, IFileUploadService fileUploadService) : base(context)
    {
        _fileUploadService = fileUploadService;
    }

    public async Task<Design?> GetByIdWithPhotosAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Design>()
            .Include(d => d.CreatedBy)
            .Include(d => d.Photos.OrderBy(p => p.CreatedAt))
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<(List<Design> Designs, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Design>()
            .Include(d => d.CreatedBy)
            .Include(d => d.Photos)
            .OrderByDescending(d => d.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);
        
        var designs = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (designs, totalCount);
    }

    public async Task<Design?> CreateDesignWithDefaultPhotoSlideAsync(string title, int createdByUserId, CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var design = new Design
            {
                Title = title,
                CreatedById = createdByUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Set<Design>().Add(design);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return await GetByIdWithPhotosAsync(design.Id, cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<Design?> CreateDesignWithPhotosAsync(string title, int createdByUserId, IFormFile[] photos, CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            // Create the design
            var design = new Design
            {
                Title = title,
                CreatedById = createdByUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Set<Design>().Add(design);
            await _context.SaveChangesAsync(cancellationToken);

            // Create PhotoSlides for each uploaded photo
            var photoSlides = new List<PhotoSlide>();
            
            for (int i = 0; i < photos.Length; i++)
            {
                var photo = photos[i];
                
                // Validate the file
                if (!_fileUploadService.IsValidPresentationFile(photo))
                {
                    throw new ArgumentException($"Invalid file: {photo.FileName}. Only image files are allowed.");
                }
                
                // Upload the file
                var photoPath = await _fileUploadService.UploadPresentationFileAsync(photo, null, cancellationToken);
                
                // Create PhotoSlide with default layout values
                var photoSlide = new PhotoSlide
                {
                    DesignId = design.Id,
                    PhotoPath = photoPath,
                    OriginalFileName = photo.FileName,
                    FileSize = photo.Length,
                    ContentType = photo.ContentType,
                    Top = 0,
                    Left = 0,
                    Width = 33.867,
                    Height = 19.05,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                photoSlides.Add(photoSlide);
            }
            
            _context.Set<PhotoSlide>().AddRange(photoSlides);
            await _context.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);

            return await GetByIdWithPhotosAsync(design.Id, cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
