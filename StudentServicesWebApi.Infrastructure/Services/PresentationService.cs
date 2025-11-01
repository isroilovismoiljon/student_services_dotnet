using StudentServicesWebApi.Application.DTOs.Presentation;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class PresentationService : IPresentationService
{
    private readonly IPresentationRepository _presentationRepository;
    private readonly ITextSlideService _textSlideService;
    private readonly IDtoMappingService _mappingService;

    public PresentationService(
        IPresentationRepository presentationRepository,
        ITextSlideService textSlideService,
        IDtoMappingService mappingService)
    {
        _presentationRepository = presentationRepository;
        _textSlideService = textSlideService;
        _mappingService = mappingService;
    }

    public async Task<List<PresentationDto>> GetAllAsync(CancellationToken ct = default)
    {
        var presentations = await _presentationRepository.GetAllAsync(ct);
        return presentations.Select(MapToDto).ToList();
    }

    public async Task<PresentationDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdAsync(id, ct);
        return presentation != null ? MapToDto(presentation) : null;
    }

    public async Task<PresentationDto> CreateAsync(CreatePresentationDto createDto, CancellationToken ct = default)
    {
        // Create Title and Author text slides
        var titleSlide = await _textSlideService.CreateTextSlideAsync(createDto.Title, ct);
        var authorSlide = await _textSlideService.CreateTextSlideAsync(createDto.Author, ct);

        // Create Presentation
        var presentation = new PresentationIsroilov
        {
            TitleId = titleSlide.Id,
            AuthorId = authorSlide.Id,
            WithPhoto = createDto.WithPhoto,
            PageCount = createDto.PageCount,
            IsActive = true,
            FilePath = string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdPresentation = await _presentationRepository.AddAsync(presentation, ct);

        return await GetByIdAsync(createdPresentation.Id, ct) 
            ?? throw new InvalidOperationException("Failed to retrieve created presentation");
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdAsync(id, ct);
        if (presentation == null) return false;

        await _presentationRepository.DeleteAsync(presentation, ct);
        return true;
    }

    private PresentationDto MapToDto(PresentationIsroilov presentation)
    {
        return new PresentationDto
        {
            Id = presentation.Id,
            Title = _mappingService.MapToTextSlideDto(presentation.Title),
            Author = _mappingService.MapToTextSlideDto(presentation.Author),
            WithPhoto = presentation.WithPhoto,
            PageCount = presentation.PageCount,
            IsActive = presentation.IsActive,
            FilePath = presentation.FilePath,
            CreatedAt = presentation.CreatedAt,
            UpdatedAt = presentation.UpdatedAt
        };
    }
}
