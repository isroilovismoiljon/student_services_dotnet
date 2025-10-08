using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Domain.Interfaces;

public interface IPresentationPostRepository : IGenericRepository<PresentationPost>
{
    Task<List<PresentationPost>> GetByPresentationPageIdAsync(int presentationPageId, CancellationToken ct = default);
}