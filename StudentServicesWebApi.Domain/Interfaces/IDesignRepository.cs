using Microsoft.AspNetCore.Http;
using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface IDesignRepository : IGenericRepository<Design>
{
    Task<Design?> GetByIdWithPhotosAsync(int id, CancellationToken cancellationToken = default);
    Task<(List<Design> Designs, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<Design?> CreateDesignWithDefaultPhotoSlideAsync(string title, int createdByUserId, CancellationToken cancellationToken = default);
    Task<Design?> CreateDesignWithPhotosAsync(string title, int createdByUserId, IFormFile[] photos, CancellationToken cancellationToken = default);
}
