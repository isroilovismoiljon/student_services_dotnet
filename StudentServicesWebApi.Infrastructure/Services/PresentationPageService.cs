using StudentServicesWebApi.Application.DTOs.PresentationPage;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Services;

public class PresentationPageService : IPresentationPageService
{
    private readonly IPresentationPageRepository _pageRepository;
    private readonly IPresentationRepository _presentationRepository;

    public PresentationPageService(
        IPresentationPageRepository pageRepository,
        IPresentationRepository presentationRepository)
    {
        _pageRepository = pageRepository;
        _presentationRepository = presentationRepository;
    }

    public async Task<List<PresentationPageDto>> GetPagesByPresentationIdAsync(int presentationId, CancellationToken ct = default)
    {
        var pages = await _pageRepository.GetByPresentationIdAsync(presentationId, ct);
        return pages.Select(p => new PresentationPageDto
        {
            Id = p.Id,
            PresentationIsroilovId = p.PresentationIsroilovId,
            PhotoId = p.Photo?.Id,
            BackgroundPhotoId = p.BackgroundPhoto?.Id,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            Posts = p.PresentationPosts.Select(pp => new PresentationPostSummaryDto
            {
                Id = pp.Id,
                TitleId = pp.TitleId,
                TextId = pp.TextId,
                CreatedAt = pp.CreatedAt
            }).ToList()
        }).ToList();
    }

    public async Task<PresentationPageDto?> GetPageByIdAsync(int id, CancellationToken ct = default)
    {
        var page = await _pageRepository.GetByIdWithPostsAsync(id, ct);
        if (page == null) return null;

        return new PresentationPageDto
        {
            Id = page.Id,
            PresentationIsroilovId = page.PresentationIsroilovId,
            PhotoId = page.Photo?.Id,
            BackgroundPhotoId = page.BackgroundPhoto?.Id,
            CreatedAt = page.CreatedAt,
            UpdatedAt = page.UpdatedAt,
            Posts = page.PresentationPosts.Select(pp => new PresentationPostSummaryDto
            {
                Id = pp.Id,
                TitleId = pp.TitleId,
                TextId = pp.TextId,
                CreatedAt = pp.CreatedAt
            }).ToList()
        };
    }

    public async Task<PresentationPageDto> CreatePageAsync(CreatePresentationPageDto createDto, CancellationToken ct = default)
    {
        var page = new PresentationPage
        {
            PresentationIsroilovId = createDto.PresentationIsroilovId
        };

        var createdPage = await _pageRepository.AddAsync(page, ct);
        
        await _presentationRepository.UpdatePageCountAsync(createDto.PresentationIsroilovId, ct);

        return new PresentationPageDto
        {
            Id = createdPage.Id,
            PresentationIsroilovId = createdPage.PresentationIsroilovId,
            PhotoId = createdPage.Photo?.Id,
            BackgroundPhotoId = createdPage.BackgroundPhoto?.Id,
            CreatedAt = createdPage.CreatedAt,
            UpdatedAt = createdPage.UpdatedAt,
            Posts = new List<PresentationPostSummaryDto>()
        };
    }

    public async Task<PresentationPageDto?> UpdatePageAsync(int id, UpdatePresentationPageDto updateDto, CancellationToken ct = default)
    {
        var page = await _pageRepository.GetByIdAsync(id, ct);
        if (page == null) return null;

        if (updateDto.PresentationIsroilovId.HasValue)
            page.PresentationIsroilovId = updateDto.PresentationIsroilovId.Value;

        page.UpdatedAt = DateTime.UtcNow;
        var updatedPage = await _pageRepository.UpdateAsync(page, ct);

        return await GetPageByIdAsync(updatedPage.Id, ct);
    }

    public async Task<bool> DeletePageAsync(int id, CancellationToken ct = default)
    {
        var page = await _pageRepository.GetByIdAsync(id, ct);
        if (page == null) return false;

        var presentationId = page.PresentationIsroilovId;
        await _pageRepository.DeleteAsync(page, ct);
        await _presentationRepository.UpdatePageCountAsync(presentationId, ct);
        
        return true;
    }

    public async Task<bool> PageExistsAsync(int id, CancellationToken ct = default)
    {
        var page = await _pageRepository.GetByIdAsync(id, ct);
        return page != null;
    }
}