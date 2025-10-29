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
    private readonly IPhotoSlideService _photoSlideService;
    private readonly IPresentationPageRepository _presentationPageRepository;
    private readonly IPresentationPostRepository _presentationPostRepository;
    private readonly IDtoMappingService _mappingService;

    public PresentationIsroilovService(
        IPresentationIsroilovRepository presentationRepository,
        ITextSlideService textSlideService,
        IPlanService planService,
        IDesignService designService,
        IPhotoSlideService photoSlideService,
        IPresentationPageRepository presentationPageRepository,
        IPresentationPostRepository presentationPostRepository,
        IDtoMappingService mappingService)
    {
        _presentationRepository = presentationRepository;
        _textSlideService = textSlideService;
        _planService = planService;
        _designService = designService;
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
        // Validate page count
        if (createDto.PageCount < 5)
        {
            throw new ArgumentException("Page count must be at least 5");
        }

        // Note: Design existence will be validated by foreign key constraint
        // We'll fetch design info only if we need background photos
        DesignDto? design = null;

        // Create Title and Author text slides
        var titleSlide = await _textSlideService.CreateTextSlideAsync(createDto.Title, ct);
        var authorSlide = await _textSlideService.CreateTextSlideAsync(createDto.Author, ct);

        // Create Plan
        var plan = await _planService.CreateAsync(createDto.Plan, ct);

        // Create PresentationIsroilov
        var presentation = new PresentationIsroilov
        {
            TitleId = titleSlide.Id,
            AuthorId = authorSlide.Id,
            WithPhoto = createDto.WithPhoto,
            PageCount = createDto.PageCount,
            DesignId = createDto.DesignId,
            PlanId = plan.Id,
            IsActive = true,
            FilePath = string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdPresentation = await _presentationRepository.AddAsync(presentation, ct);

        // Validate that provided pages count matches PageCount
        if (createDto.PresentationPages.Count != createDto.PageCount)
        {
            throw new ArgumentException($"Number of presentation pages ({createDto.PresentationPages.Count}) must match PageCount ({createDto.PageCount})");
        }

        // Create PresentationPages from provided data
        for (int i = 0; i < createDto.PresentationPages.Count; i++)
        {
            var pageDto = createDto.PresentationPages[i];
            
            // Determine if this page should have photos based on position and WithPhoto setting
            bool pageWithPhoto = false;
            if (createDto.WithPhoto && i >= 2) // Pages 1-2 (index 0-1) never have photos
            {
                // Pattern: page 3, 6, 9, 12, etc. (index 2, 5, 8, 11) have photos
                int adjustedPage = i - 2; // Start counting from page 3
                pageWithPhoto = (adjustedPage % 3 == 0);
            }

            // Get background photo from design
            int? backgroundPhotoId = pageDto.BackgroundPhotoId;
            if (!backgroundPhotoId.HasValue)
            {
                // Lazy load design only when needed for background photos
                if (design == null)
                {
                    design = await _designService.GetDesignByIdAsync(createDto.DesignId, ct);
                }
                
                if (design != null && design.Photos != null && design.Photos.Count > 0)
                {
                    backgroundPhotoId = design.Photos[i % design.Photos.Count].Id;
                }
            }

            var presentationPage = new PresentationPage
            {
                PresentationIsroilovId = createdPresentation.Id,
                PhotoId = pageDto.PhotoId,
                BackgroundPhotoId = backgroundPhotoId,
                WithPhoto = pageWithPhoto,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdPage = await _presentationPageRepository.AddAsync(presentationPage, ct);

            // Create presentation posts for this page
            if (pageDto.PresentationPosts != null)
            {
                foreach (var postDto in pageDto.PresentationPosts)
                {
                    // Create title text slide if provided
                    int? titleSlideId = null;
                    if (postDto.Title != null)
                    {
                        var titleTextSlide = await _textSlideService.CreateTextSlideAsync(postDto.Title, ct);
                        titleSlideId = titleTextSlide.Id;
                    }

                    // Create text slide for content
                    var textSlide = await _textSlideService.CreateTextSlideAsync(postDto.Text, ct);

                    var presentationPost = new PresentationPost
                    {
                        PresentationPageId = createdPage.Id,
                        TitleId = titleSlideId,
                        TextId = textSlide.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _presentationPostRepository.AddAsync(presentationPost, ct);
                }
            }
        }

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

        // Get pages that should have photos
        var pagesWithPhoto = presentation.PresentationPages
            .Where(p => p.WithPhoto)
            .OrderBy(p => p.Id)
            .ToList();

        if (updateDto.Photos.Count != pagesWithPhoto.Count)
        {
            throw new ArgumentException($"Expected {pagesWithPhoto.Count} photos, but received {updateDto.Photos.Count}");
        }

        // Update photos for each page
        for (int i = 0; i < pagesWithPhoto.Count; i++)
        {
            var page = pagesWithPhoto[i];
            var photo = updateDto.Photos[i];

            // Create or update photo slide
            if (page.PhotoId.HasValue)
            {
                // Update existing photo
                await _photoSlideService.UpdatePhotoSlideFileAsync(page.PhotoId.Value, photo, ct);
            }
            else
            {
                // Create new photo slide
                var createPhotoDto = new Application.DTOs.PhotoSlide.CreatePhotoSlideDto
                {
                    Photo = photo,
                    Left = 0,
                    Top = 0,
                    Width = 100,
                    Height = 100
                };
                var createdPhoto = await _photoSlideService.CreatePhotoSlideAsync(createPhotoDto, ct);
                page.PhotoId = createdPhoto.Id;
                page.UpdatedAt = DateTime.UtcNow;
                await _presentationPageRepository.UpdateAsync(page, ct);
            }
        }

        return await GetPresentationByIdAsync(id, ct) 
            ?? throw new InvalidOperationException("Failed to retrieve updated presentation");
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
            DesignId = presentation.DesignId,
            Plan = _mappingService.MapToPlanDto(presentation.Plan),
            FilePath = presentation.FilePath,
            IsActive = presentation.IsActive,
            CreatedAt = presentation.CreatedAt,
            UpdatedAt = presentation.UpdatedAt,
            PresentationPages = presentation.PresentationPages
                .Select(pp => _mappingService.MapToPresentationPageDto(pp))
                .ToList()
        };
    }
}
