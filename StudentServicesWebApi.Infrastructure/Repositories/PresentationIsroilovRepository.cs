using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Repositories;

public class PresentationIsroilovRepository : GenericRepository<PresentationIsroilov>, IPresentationIsroilovRepository
{
    public PresentationIsroilovRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<PresentationIsroilov>> GetAllWithDetailsAsync(CancellationToken ct = default)
    {
        return await _context.Set<PresentationIsroilov>()
            .Where(p => !p.IsDeleted)
            .Include(p => p.Title)
            .Include(p => p.Author)
            .Include(p => p.Plan)
                .ThenInclude(plan => plan.PlanText)
            .Include(p => p.Plan)
                .ThenInclude(plan => plan.Plans)
            .Include(p => p.Design)
                .ThenInclude(design => design.Photos)
            .Include(p => p.Design)
                .ThenInclude(design => design.CreatedBy)
            .Include(p => p.PresentationPages)
                .ThenInclude(page => page.PresentationPosts)
                    .ThenInclude(post => post.Title)
            .Include(p => p.PresentationPages)
                .ThenInclude(page => page.PresentationPosts)
                    .ThenInclude(post => post.Text)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<PresentationIsroilov?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
    {
        return await _context.Set<PresentationIsroilov>()
            .Where(p => !p.IsDeleted)
            .Include(p => p.Title)
            .Include(p => p.Author)
            .Include(p => p.Plan)
                .ThenInclude(plan => plan.PlanText)
            .Include(p => p.Plan)
                .ThenInclude(plan => plan.Plans)
            .Include(p => p.Design)
                .ThenInclude(design => design.Photos)
            .Include(p => p.Design)
                .ThenInclude(design => design.CreatedBy)
            .Include(p => p.PresentationPages)
                .ThenInclude(page => page.PresentationPosts)
                    .ThenInclude(post => post.Title)
            .Include(p => p.PresentationPages)
                .ThenInclude(page => page.PresentationPosts)
                    .ThenInclude(post => post.Text)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }
}
