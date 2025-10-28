using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Repositories;
public class PresentationPageRepository : GenericRepository<PresentationPage>, IPresentationPageRepository
{
    public PresentationPageRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<List<PresentationPage>> GetByPresentationIdAsync(int presentationId, CancellationToken ct = default)
    {
        return await _context.Set<PresentationPage>()
            .Where(pp => pp.PresentationIsroilovId == presentationId)
            .Include(pp => pp.PresentationPosts)
            .OrderBy(pp => pp.CreatedAt)
            .ToListAsync(ct);
    }
    public async Task<PresentationPage?> GetByIdWithPostsAsync(int id, CancellationToken ct = default)
    {
        return await _context.Set<PresentationPage>()
            .Include(pp => pp.PresentationPosts)
            .FirstOrDefaultAsync(pp => pp.Id == id, ct);
    }
}
