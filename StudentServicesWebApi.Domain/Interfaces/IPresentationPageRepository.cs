using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface IPresentationPageRepository : IGenericRepository<PresentationPage>
{
    Task<List<PresentationPage>> GetByPresentationIdAsync(int presentationId, CancellationToken ct = default);
    Task<PresentationPage?> GetByIdWithPostsAsync(int id, CancellationToken ct = default);
}
