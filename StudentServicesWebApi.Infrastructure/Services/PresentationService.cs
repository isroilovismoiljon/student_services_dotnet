using StudentServicesWebApi.Application.DTOs.Presentation;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class PresentationService : IPresentationService
{
    private readonly IPresentationRepository _presentationRepository;

    public PresentationService(IPresentationRepository presentationRepository)
    {
        _presentationRepository = presentationRepository;
    }

    public async Task<List<PresentationSummaryDto>> GetAllPresentationsAsync(CancellationToken ct = default)
    {
        var presentations = await _presentationRepository.GetAllWithPagesAsync(ct);
        return presentations.Select(p => new PresentationSummaryDto
        {
            Id = p.Id,
            Title = p.Title,
            Author = p.Author,
            PageCount = p.PageCount,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();
    }

    public async Task<PresentationDto?> GetPresentationByIdAsync(int id, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdWithPagesAsync(id, ct);
        if (presentation == null) return null;

        return new PresentationDto
        {
            Id = presentation.Id,
            Title = presentation.Title,
            Author = presentation.Author,
            PageCount = presentation.PageCount,
            IsActive = presentation.IsActive,
            FilePath = presentation.FilePath,
            DesignId = presentation.DesignId,
            PlanId = presentation.PlanId,
            CreatedAt = presentation.CreatedAt,
            UpdatedAt = presentation.UpdatedAt,
            Pages = presentation.PresentationPages.Select(pp => new PresentationPageSummaryDto
            {
                Id = pp.Id,
                PostCount = pp.PresentationPosts.Count,
                CreatedAt = pp.CreatedAt
            }).ToList()
        };
    }

    public async Task<PresentationDto> CreatePresentationAsync(CreatePresentationDto createDto, CancellationToken ct = default)
    {
        var presentation = new PresentationIsroilov
        {
            Title = createDto.Title,
            Author = createDto.Author,
            DesignId = createDto.DesignId,
            PlanId = createDto.PlanId,
            IsActive = createDto.IsActive,
            FilePath = createDto.FilePath,
            PageCount = 0
        };

        var createdPresentation = await _presentationRepository.AddAsync(presentation, ct);
        
        return new PresentationDto
        {
            Id = createdPresentation.Id,
            Title = createdPresentation.Title,
            Author = createdPresentation.Author,
            PageCount = createdPresentation.PageCount,
            IsActive = createdPresentation.IsActive,
            FilePath = createdPresentation.FilePath,
            DesignId = createdPresentation.DesignId,
            PlanId = createdPresentation.PlanId,
            CreatedAt = createdPresentation.CreatedAt,
            UpdatedAt = createdPresentation.UpdatedAt,
            Pages = new List<PresentationPageSummaryDto>()
        };
    }

    public async Task<PresentationDto?> UpdatePresentationAsync(int id, UpdatePresentationDto updateDto, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdAsync(id, ct);
        if (presentation == null) return null;

        if (updateDto.Title != null) presentation.Title = updateDto.Title;
        if (updateDto.Author != null) presentation.Author = updateDto.Author;
        if (updateDto.DesignId.HasValue) presentation.DesignId = updateDto.DesignId.Value;
        if (updateDto.PlanId.HasValue) presentation.PlanId = updateDto.PlanId.Value;
        if (updateDto.IsActive.HasValue) presentation.IsActive = updateDto.IsActive.Value;
        if (updateDto.FilePath != null) presentation.FilePath = updateDto.FilePath;

        presentation.UpdatedAt = DateTime.UtcNow;
        var updatedPresentation = await _presentationRepository.UpdateAsync(presentation, ct);

        return await GetPresentationByIdAsync(updatedPresentation.Id, ct);
    }

    public async Task<bool> DeletePresentationAsync(int id, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdAsync(id, ct);
        if (presentation == null) return false;

        await _presentationRepository.DeleteAsync(presentation, ct);
        return true;
    }

    public async Task<bool> PresentationExistsAsync(int id, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdAsync(id, ct);
        return presentation != null;
    }
}