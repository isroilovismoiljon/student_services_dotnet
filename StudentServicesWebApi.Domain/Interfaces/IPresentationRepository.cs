using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Domain.Interfaces;

public interface IPresentationRepository
{
    Task<List<PresentationIsroilov>> GetAllAsync(CancellationToken ct = default);
    Task<PresentationIsroilov?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PresentationIsroilov> AddAsync(PresentationIsroilov presentation, CancellationToken ct = default);
    Task<PresentationIsroilov> UpdateAsync(PresentationIsroilov presentation, CancellationToken ct = default);
    Task DeleteAsync(PresentationIsroilov presentation, CancellationToken ct = default);
}
