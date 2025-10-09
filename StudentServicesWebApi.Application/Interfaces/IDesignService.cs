using StudentServicesWebApi.Application.DTOs.Design;

namespace StudentServicesWebApi.Application.Interfaces;

public interface IDesignService
{
    Task<DesignDto> CreateDesignAsync(DesignCreateWithPhotosDto createDesignWithPhotosDto, int createdByUserId, CancellationToken cancellationToken = default);
    Task<DesignDto?> GetDesignByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(List<DesignSummaryDto> Designs, int TotalCount)> GetPagedDesignsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<DesignDto?> UpdateDesignAsync(int id, UpdateDesignDto updateDesignDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteDesignAsync(int id, CancellationToken cancellationToken = default);
}
