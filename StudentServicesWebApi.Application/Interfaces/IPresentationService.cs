using Microsoft.AspNetCore.Http;
using StudentServicesWebApi.Application.DTOs.Presentation;
namespace StudentServicesWebApi.Application.Interfaces;
public interface IPresentationService
{
    Task<List<PresentationSummaryDto>> GetAllPresentationsAsync(CancellationToken ct = default);
    Task<PresentationDto?> GetPresentationByIdAsync(int id, CancellationToken ct = default);
    Task<PresentationDto> CreatePresentationAsync(CreatePresentationDto createDto, CancellationToken ct = default);
    Task<PresentationDto> CreatePresentationWithPositionsAsync(CreatePresentationWithPositionsDto createDto, CancellationToken ct = default);
    Task<PresentationDto> UpdatePresentationPhotosAsync(int presentationId, List<IFormFile> photos, CancellationToken ct = default);
    Task<PresentationDto?> UpdatePresentationAsync(int id, UpdatePresentationDto updateDto, CancellationToken ct = default);
    Task<bool> DeletePresentationAsync(int id, CancellationToken ct = default);
    Task<bool> PresentationExistsAsync(int id, CancellationToken ct = default);
}
