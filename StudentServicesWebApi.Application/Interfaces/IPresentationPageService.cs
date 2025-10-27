using StudentServicesWebApi.Application.DTOs.PresentationPage;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Application.Interfaces;

public interface IPresentationPageService
{
    Task<List<PresentationPageDto>> GetPagesByPresentationIdAsync(int presentationId, CancellationToken ct = default);
    Task<PresentationPageDto?> GetPageByIdAsync(int id, CancellationToken ct = default);
    Task<PresentationPageDto> CreatePageAsync(CreatePresentationPageDto createDto, CancellationToken ct = default);
    Task<PresentationPageDto> CreatePresentationPageAsync(int presentationId, CreatePresentationPageDto createDto, CancellationToken ct = default);
    Task<PresentationPage> CreatePageDirectAsync(PresentationPage presentationPage, CancellationToken ct = default);
    Task<PresentationPageDto?> UpdatePageAsync(int id, UpdatePresentationPageDto updateDto, CancellationToken ct = default);
    Task<bool> DeletePageAsync(int id, CancellationToken ct = default);
    Task<bool> PageExistsAsync(int id, CancellationToken ct = default);
}
