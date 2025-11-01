using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Repositories;

public class PresentationRepository : IPresentationRepository
{
    private readonly AppDbContext _context;

    public PresentationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PresentationIsroilov>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.PresentationIsroilovs
            .Include(p => p.Title)
            .Include(p => p.Author)
            .Where(p => !p.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<PresentationIsroilov?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.PresentationIsroilovs
            .Include(p => p.Title)
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct);
    }

    public async Task<PresentationIsroilov> AddAsync(PresentationIsroilov presentation, CancellationToken ct = default)
    {
        await _context.PresentationIsroilovs.AddAsync(presentation, ct);
        await _context.SaveChangesAsync(ct);
        return presentation;
    }

    public async Task<PresentationIsroilov> UpdateAsync(PresentationIsroilov presentation, CancellationToken ct = default)
    {
        presentation.UpdatedAt = DateTime.UtcNow;
        _context.PresentationIsroilovs.Update(presentation);
        await _context.SaveChangesAsync(ct);
        return presentation;
    }

    public async Task DeleteAsync(PresentationIsroilov presentation, CancellationToken ct = default)
    {
        presentation.IsDeleted = true;
        presentation.DeletedAt = DateTime.UtcNow;
        await UpdateAsync(presentation, ct);
    }
}
