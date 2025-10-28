using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface IPresentationRepository : IGenericRepository<PresentationIsroilov>
{
    Task<List<PresentationIsroilov>> GetAllWithPagesAsync(CancellationToken ct = default);
    Task<PresentationIsroilov?> GetByIdWithPagesAsync(int id, CancellationToken ct = default);
    Task UpdatePageCountAsync(int presentationId, CancellationToken ct = default);
}
