using StudentServicesWebApi.Application.DTOs.Design;
using StudentServicesWebApi.Application.DTOs.PresentationIsroilov;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class PresentationIsroilovService : IPresentationIsroilovService
{
    private readonly IPresentationIsroilovRepository _presentationRepository;
    private readonly ITextSlideService _textSlideService;
    private readonly IPlanService _planService;
    private readonly IDesignService _designService;
    private readonly IDesignRepository _designRepository;
    private readonly IPhotoSlideService _photoSlideService;
    private readonly IPresentationPageRepository _presentationPageRepository;
    private readonly IPresentationPostRepository _presentationPostRepository;
    private readonly IDtoMappingService _mappingService;

    public PresentationIsroilovService(
        IPresentationIsroilovRepository presentationRepository,
        ITextSlideService textSlideService,
        IPlanService planService,
        IDesignService designService,
        IDesignRepository designRepository,
        IPhotoSlideService photoSlideService,
        IPresentationPageRepository presentationPageRepository,
        IPresentationPostRepository presentationPostRepository,
        IDtoMappingService mappingService)
    {
        _presentationRepository = presentationRepository;
        _textSlideService = textSlideService;
        _planService = planService;
        _designService = designService;
        _designRepository = designRepository;
        _photoSlideService = photoSlideService;
        _presentationPageRepository = presentationPageRepository;
        _presentationPostRepository = presentationPostRepository;
        _mappingService = mappingService;
    }

    public async Task<List<PresentationIsroilovDto>> GetAllPresentationsAsync(CancellationToken ct = default)
    {
        var presentations = await _presentationRepository.GetAllWithDetailsAsync(ct);
        return presentations.Select(MapToDto).ToList();
    }

    public async Task<PresentationIsroilovDto?> GetPresentationByIdAsync(int id, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdWithDetailsAsync(id, ct);
        return presentation != null ? MapToDto(presentation) : null;
    }

    public async Task<PresentationIsroilovDto> CreatePresentationAsync(CreatePresentationIsroilovDto createDto, CancellationToken ct = default)
    {
        // Create Title and Author text slides
        var titleSlide = await _textSlideService.CreateTextSlideAsync(createDto.Title, ct);
        var authorSlide = await _textSlideService.CreateTextSlideAsync(createDto.Author, ct);

        // Create PresentationIsroilov
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

        return await GetPresentationByIdAsync(createdPresentation.Id, ct) 
            ?? throw new InvalidOperationException("Failed to retrieve created presentation");
    }

    public async Task<PresentationIsroilovDto> UpdatePresentationPhotosAsync(int id, UpdatePresentationIsroilovPhotosDto updateDto, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdWithDetailsAsync(id, ct);
        if (presentation == null)
        {
            throw new ArgumentException($"Presentation with ID {id} not found");
        }

        if (!presentation.WithPhoto)
        {
            throw new InvalidOperationException("Cannot update photos for a presentation created with WithPhoto=false");
        }

        // For now, this is a placeholder implementation
        // You would need to implement page management separately
        throw new NotImplementedException("Photo update functionality requires PresentationPage management");
    }

    public async Task<bool> DeletePresentationAsync(int id, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdAsync(id, ct);
        if (presentation == null) return false;

        await _presentationRepository.DeleteAsync(presentation, ct);
        return true;
    }

    private PresentationIsroilovDto MapToDto(PresentationIsroilov presentation)
    {
        return new PresentationIsroilovDto
        {
            Id = presentation.Id,
            Title = _mappingService.MapToTextSlideDto(presentation.Title),
            Author = _mappingService.MapToTextSlideDto(presentation.Author),
            WithPhoto = presentation.WithPhoto,
            PageCount = presentation.PageCount,
            FilePath = presentation.FilePath,
            IsActive = presentation.IsActive,
            CreatedAt = presentation.CreatedAt,
            UpdatedAt = presentation.UpdatedAt
        };
    }
}
