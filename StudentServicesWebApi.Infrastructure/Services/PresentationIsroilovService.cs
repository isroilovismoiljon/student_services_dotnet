using StudentServicesWebApi.Application.DTOs.Design;
using StudentServicesWebApi.Application.DTOs.PresentationIsroilov;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    private readonly AppDbContext _context;

    public PresentationIsroilovService(
        IPresentationIsroilovRepository presentationRepository,
        ITextSlideService textSlideService,
        IPlanService planService,
        IDesignService designService,
        IDesignRepository designRepository,
        IPhotoSlideService photoSlideService,
        IPresentationPageRepository presentationPageRepository,
        IPresentationPostRepository presentationPostRepository,
        IDtoMappingService mappingService,
        AppDbContext context)
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
        _context = context;
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
        // Validate DesignId exists
        var design = await _designRepository.GetByIdWithPhotosAsync(createDto.DesignId, ct);
        if (design == null)
        {
            throw new ArgumentException($"Design with ID {createDto.DesignId} not found");
        }

        // Create Title and Author text slides
        var titleSlide = await _textSlideService.CreateTextSlideAsync(createDto.Title, ct);
        var authorSlide = await _textSlideService.CreateTextSlideAsync(createDto.Author, ct);
        
        // Create Plan
        var plan = await _planService.CreateAsync(createDto.Plan, ct);

        // Create PresentationIsroilov with navigation properties
        var presentation = new PresentationIsroilov
        {
            TitleId = titleSlide.Id,
            AuthorId = authorSlide.Id,
            PlanId = plan.Id,
            DesignId = createDto.DesignId,
            WithPhoto = createDto.WithPhoto,
            PageCount = createDto.PageCount,
            IsActive = true,
            FilePath = string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PresentationPages = new List<PresentationPage>()
        };

        // Create PresentationPages and PresentationPosts using navigation properties
        if (createDto.PresentationPages != null && createDto.PresentationPages.Any())
        {
            foreach (var pageDto in createDto.PresentationPages)
            {
                var page = new PresentationPage
                {
                    WithPhoto = createDto.WithPhoto,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PresentationPosts = new List<PresentationPost>()
                };

                // Create PresentationPosts for this page
                if (pageDto.PresentationPosts != null && pageDto.PresentationPosts.Any())
                {
                    foreach (var postDto in pageDto.PresentationPosts)
                    {
                        // Create Title TextSlide
                        var postTitleSlide = await _textSlideService.CreateTextSlideAsync(postDto.Title, ct);
                        
                        // Create Text TextSlide
                        var postTextSlide = await _textSlideService.CreateTextSlideAsync(postDto.Text, ct);

                        // Create PresentationPost
                        var post = new PresentationPost
                        {
                            TitleId = postTitleSlide.Id,
                            TextId = postTextSlide.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        page.PresentationPosts.Add(post);
                    }
                }
                
                presentation.PresentationPages.Add(page);
            }
        }

        // Add presentation with all nested entities in one go
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
            PlanId = presentation.PlanId,
            Plan = _mappingService.MapToPlanDto(presentation.Plan),
            DesignId = presentation.DesignId,
            Design = _mappingService.MapToDesignDto(presentation.Design),
            WithPhoto = presentation.WithPhoto,
            PageCount = presentation.PageCount,
            FilePath = presentation.FilePath,
            IsActive = presentation.IsActive,
            CreatedAt = presentation.CreatedAt,
            UpdatedAt = presentation.UpdatedAt,
            PresentationPages = presentation.PresentationPages?.Select(_mappingService.MapToPresentationPageDto).ToList() ?? new()
        };
    }
}
