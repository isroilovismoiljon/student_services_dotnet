using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Repositories;

public class PresentationRepository : GenericRepository<PresentationIsroilov>, IPresentationRepository
{
    public PresentationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<PresentationIsroilov>> GetAllWithPagesAsync(CancellationToken ct = default)
    {
        return await _context.Set<PresentationIsroilov>()
            .Where(p => !p.IsDeleted)
            .Include(p => p.Title)
            .Include(p => p.Author)
            .Include(p => p.PresentationPages)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<PresentationIsroilov?> GetByIdWithPagesAsync(int id, CancellationToken ct = default)
    {
        return await _context.Set<PresentationIsroilov>()
            .Where(p => !p.IsDeleted)
            .Include(p => p.Title)
            .Include(p => p.Author)
            .Include(p => p.PresentationPages)
                .ThenInclude(pp => pp.PresentationPosts)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task UpdatePageCountAsync(int presentationId, CancellationToken ct = default)
    {
        var presentation = await GetByIdAsync(presentationId, ct);
        if (presentation != null)
        {
            var pageCount = await _context.Set<PresentationPage>()
                .Where(pp => !pp.IsDeleted)
                .CountAsync(pp => pp.PresentationIsroilovId == presentationId, ct);
            
            presentation.PageCount = pageCount;
            await UpdateAsync(presentation, ct);
        }
    }
}