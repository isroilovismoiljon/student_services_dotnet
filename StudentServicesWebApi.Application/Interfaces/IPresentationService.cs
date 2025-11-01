using StudentServicesWebApi.Application.DTOs.Presentation;

namespace StudentServicesWebApi.Application.Interfaces;

public interface IPresentationService
{
    Task<List<PresentationDto>> GetAllAsync(CancellationToken ct = default);
    Task<PresentationDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PresentationDto> CreateAsync(CreatePresentationDto createDto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
