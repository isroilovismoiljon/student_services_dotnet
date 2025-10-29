using StudentServicesWebApi.Application.DTOs.PresentationIsroilov;

namespace StudentServicesWebApi.Application.Interfaces;

public interface IPresentationIsroilovService
{
    Task<List<PresentationIsroilovDto>> GetAllPresentationsAsync(CancellationToken ct = default);
    Task<PresentationIsroilovDto?> GetPresentationByIdAsync(int id, CancellationToken ct = default);
    Task<PresentationIsroilovDto> CreatePresentationAsync(CreatePresentationIsroilovDto createDto, CancellationToken ct = default);
    Task<PresentationIsroilovDto> UpdatePresentationPhotosAsync(int id, UpdatePresentationIsroilovPhotosDto updateDto, CancellationToken ct = default);
    Task<bool> DeletePresentationAsync(int id, CancellationToken ct = default);
}
