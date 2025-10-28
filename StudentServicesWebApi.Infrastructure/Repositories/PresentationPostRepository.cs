using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Repositories;
public class PresentationPostRepository : GenericRepository<PresentationPost>, IPresentationPostRepository
{
    public PresentationPostRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<List<PresentationPost>> GetByPresentationPageIdAsync(int presentationPageId, CancellationToken ct = default)
    {
        return await _context.Set<PresentationPost>()
            .Where(pp => pp.PresentationPageId == presentationPageId)
            .OrderBy(pp => pp.CreatedAt)
            .ToListAsync(ct);
    }
}
