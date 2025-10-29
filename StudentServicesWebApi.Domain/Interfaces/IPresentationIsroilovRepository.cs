using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Domain.Interfaces;

public interface IPresentationIsroilovRepository : IGenericRepository<PresentationIsroilov>
{
    Task<List<PresentationIsroilov>> GetAllWithDetailsAsync(CancellationToken ct = default);
    Task<PresentationIsroilov?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
}
