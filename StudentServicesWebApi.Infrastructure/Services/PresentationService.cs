using Microsoft.AspNetCore.Http;
using StudentServicesWebApi.Application.DTOs.Presentation;
using StudentServicesWebApi.Application.DTOs.PresentationPage;
using StudentServicesWebApi.Application.DTOs.PresentationPost;
using StudentServicesWebApi.Application.DTOs.PhotoSlide;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class PresentationService : IPresentationService
{
    private readonly IPresentationRepository _presentationRepository;
    private readonly ITextSlideService _textSlideService;
    private readonly IPresentationPageService _presentationPageService;
    private readonly IPresentationPostService _presentationPostService;
    private readonly IPhotoSlideService _photoSlideService;
    private readonly IPlanService _planService;
    private readonly IDesignService _designService;
    private readonly IDtoMappingService _mappingService;

    public PresentationService(
        IPresentationRepository presentationRepository,
        ITextSlideService textSlideService,
        IPresentationPageService presentationPageService,
        IPresentationPostService presentationPostService,
        IPhotoSlideService photoSlideService,
        IPlanService planService,
        IDesignService designService,
        IDtoMappingService mappingService)
    {
        _presentationRepository = presentationRepository;
        _textSlideService = textSlideService;
        _presentationPageService = presentationPageService;
        _presentationPostService = presentationPostService;
        _photoSlideService = photoSlideService;
        _planService = planService;
        _designService = designService;
        _mappingService = mappingService;
    }

    public async Task<List<PresentationSummaryDto>> GetAllPresentationsAsync(CancellationToken ct = default)
    {
        var presentations = await _presentationRepository.GetAllWithPagesAsync(ct);
        return presentations.Select(p => new PresentationSummaryDto
        {
            Id = p.Id,
            Title = _mappingService.MapToTextSlideSummaryDto(p.Title),
            Author = _mappingService.MapToTextSlideSummaryDto(p.Author),
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
            Title = _mappingService.MapToTextSlideDto(presentation.Title),
            Author = _mappingService.MapToTextSlideDto(presentation.Author),
            PageCount = presentation.PageCount,
            IsActive = presentation.IsActive,
            FilePath = presentation.FilePath,
            DesignId = presentation.DesignId,
            PlanId = presentation.PlanId,
            CreatedAt = presentation.CreatedAt,
            UpdatedAt = presentation.UpdatedAt,
            Pages = presentation.PresentationPages.Select(pp => new Application.DTOs.Presentation.PresentationPageSummaryDto
            {
                Id = pp.Id,
                PostCount = pp.PresentationPosts.Count,
                CreatedAt = pp.CreatedAt
            }).ToList()
        };
    }

    public async Task<PresentationDto> CreatePresentationAsync(CreatePresentationDto createDto, CancellationToken ct = default)
    {
        // Validate photo requirement
        if (createDto.WithPhoto)
        {
            var requiredPhotoCount = (createDto.PageCount - 2) / 2;
            if (createDto.Photos.Count != requiredPhotoCount)
            {
                throw new ArgumentException($"With WithPhoto=true and PageCount={createDto.PageCount}, exactly {requiredPhotoCount} photos are required, but {createDto.Photos.Count} were provided.");
            }
        }
        else if (createDto.Photos.Any())
        {
            throw new ArgumentException("Photos provided but WithPhoto is false.");
        }

        // Get the design to access background photos
        var design = await _designService.GetDesignByIdAsync(createDto.DesignId, ct);
        if (design == null)
        {
            throw new ArgumentException($"Design with ID {createDto.DesignId} not found.");
        }

        // Create TextSlide entities for Title and Author
        var titleTextSlide = await _textSlideService.CreateTextSlideAsync(createDto.Title, ct);
        var authorTextSlide = await _textSlideService.CreateTextSlideAsync(createDto.Author, ct);
        
        // Create Plan with its TextSlides
        var createdPlan = await _planService.CreateAsync(createDto.PlanData, ct);
        
        // Create PhotoSlides from uploaded photos if WithPhoto is true
        var createdPhotos = new List<PhotoSlideDto>();
        if (createDto.WithPhoto)
        {
            foreach (var photoDto in createDto.Photos)
            {
                var createPhotoSlideDto = new CreatePhotoSlideDto
                {
                    Photo = photoDto.Photo,
                    Left = photoDto.Left,
                    Top = photoDto.Top,
                    Width = photoDto.Width,
                    Height = photoDto.Height
                };
                var createdPhoto = await _photoSlideService.CreatePhotoSlideAsync(createPhotoSlideDto, ct);
                createdPhotos.Add(createdPhoto);
            }
        }
        
        // Create TextSlides for post texts
        var postTextSlides = new List<TextSlideDto>();
        foreach (var postText in createDto.PostTexts)
        {
            var createdPostText = await _textSlideService.CreateTextSlideAsync(postText, ct);
            postTextSlides.Add(createdPostText);
        }
        
        var presentation = new PresentationIsroilov
        {
            TitleId = titleTextSlide.Id,
            AuthorId = authorTextSlide.Id,
            DesignId = createDto.DesignId,
            PlanId = createdPlan.Id,
            WithPhoto = createDto.WithPhoto,
            PageCount = createDto.PageCount,
            IsActive = createDto.IsActive,
            FilePath = createDto.FilePath
        };

        var createdPresentation = await _presentationRepository.AddAsync(presentation, ct);
        
        // Create pages with automatic background assignment and photo linking
        int photoIndex = 0;
        int postTextIndex = 0;
        
        for (int i = 0; i < createDto.PageCount; i++)
        {
            // Get background photo from design (cycling through available photos)
            var backgroundPhotoId = design.Photos?.Count > 0 ? design.Photos[i % design.Photos.Count].Id : (int?)null;
            
            // Get user photo if WithPhoto is true (distributed across pages)
            int? userPhotoId = null;
            if (createDto.WithPhoto && photoIndex < createdPhotos.Count)
            {
                userPhotoId = createdPhotos[photoIndex].Id;
                photoIndex++;
            }
            
            var createPageDto = new CreatePresentationPageDto
            {
                PhotoId = userPhotoId,
                BackgroundPhotoId = backgroundPhotoId
            };
            
            var createdPage = await _presentationPageService.CreatePresentationPageAsync(createdPresentation.Id, createPageDto, ct);
            
            // Create up to 3 presentation posts per page
            var postsToCreateForThisPage = Math.Min(3, postTextSlides.Count - postTextIndex);
            
            for (int j = 0; j < postsToCreateForThisPage; j++)
            {
                var createPostDto = new CreatePresentationPostDto
                {
                    PresentationPageId = createdPage.Id,
                    TextId = postTextSlides[postTextIndex].Id,
                    TitleId = null // Can be extended later if needed
                };
                
                await _presentationPostService.CreatePostAsync(createPostDto, ct);
                postTextIndex++;
            }
        }
        
        return await GetPresentationByIdAsync(createdPresentation.Id, ct) ?? throw new InvalidOperationException("Failed to retrieve created presentation");
    }

    public async Task<PresentationDto?> UpdatePresentationAsync(int id, UpdatePresentationDto updateDto, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdAsync(id, ct);
        if (presentation == null) return null;

        if (updateDto.Title != null)
        {
            await _textSlideService.UpdateTextSlideAsync(presentation.TitleId, updateDto.Title, ct);
        }
        
        if (updateDto.Author != null)
        {
            await _textSlideService.UpdateTextSlideAsync(presentation.AuthorId, updateDto.Author, ct);
        }
        
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

    public async Task<PresentationDto> CreatePresentationWithPositionsAsync(CreatePresentationWithPositionsDto createDto, CancellationToken ct = default)
    {
        // Validate photo positions requirement
        if (createDto.WithPhoto)
        {
            var requiredPhotoCount = (createDto.PageCount - 2) / 2;
            if (createDto.PhotoPositions.Count != requiredPhotoCount)
            {
                throw new ArgumentException($"With WithPhoto=true and PageCount={createDto.PageCount}, exactly {requiredPhotoCount} photo positions are required, but {createDto.PhotoPositions.Count} were provided.");
            }
        }
        else if (createDto.PhotoPositions.Any())
        {
            throw new ArgumentException("Photo positions provided but WithPhoto is false.");
        }

        // Get the design to access background photos
        var design = await _designService.GetDesignByIdAsync(createDto.DesignId, ct);
        if (design == null)
        {
            throw new ArgumentException($"Design with ID {createDto.DesignId} not found.");
        }

        // Create TextSlide entities for Title and Author
        var titleTextSlide = await _textSlideService.CreateTextSlideAsync(new CreateTextSlideDto
        {
            Text = createDto.TitleText,
            Font = createDto.TitleFont,
            Size = createDto.TitleSize,
            Left = createDto.TitleLeft,
            Top = createDto.TitleTop,
            Width = createDto.TitleWidth,
            Height = createDto.TitleHeight
        }, ct);
        
        var authorTextSlide = await _textSlideService.CreateTextSlideAsync(new CreateTextSlideDto
        {
            Text = createDto.AuthorText,
            Font = createDto.AuthorFont,
            Size = createDto.AuthorSize,
            Left = createDto.AuthorLeft,
            Top = createDto.AuthorTop,
            Width = createDto.AuthorWidth,
            Height = createDto.AuthorHeight
        }, ct);
        
        // Create Plan with its TextSlides
        var createdPlan = await _planService.CreateAsync(new Application.DTOs.Plan.CreatePlanDto
        {
            PlanText = new CreateTextSlideDto
            {
                Text = createDto.PlanText,
                Font = createDto.PlanFont,
                Size = createDto.PlanSize,
                Left = createDto.PlanLeft,
                Top = createDto.PlanTop,
                Width = createDto.PlanWidth,
                Height = createDto.PlanHeight
            },
            Plans = new CreateTextSlideDto
            {
                Text = createDto.PlanText,
                Font = createDto.PlanFont,
                Size = createDto.PlanSize,
                Left = createDto.PlanLeft,
                Top = createDto.PlanTop + createDto.PlanHeight + 1,
                Width = createDto.PlanWidth,
                Height = createDto.PlanHeight
            }
        }, ct);
        
        // Create placeholder PhotoSlides with positions (no actual files yet)
        var placeholderPhotos = new List<PhotoSlideDto>();
        if (createDto.WithPhoto)
        {
            foreach (var position in createDto.PhotoPositions)
            {
                // Create a placeholder PhotoSlide without an actual file
                var placeholderPhoto = new PhotoSlide
                {
                    PhotoPath = "placeholder", // Will be updated when photos are uploaded
                    OriginalFileName = "placeholder",
                    FileSize = 0,
                    ContentType = "image/placeholder",
                    Left = position.Left,
                    Top = position.Top,
                    Width = position.Width,
                    Height = position.Height ?? 50 // Default height if not provided
                };
                
                // Add to repository to get an ID
                var createdPlaceholder = await _photoSlideService.CreatePlaceholderPhotoSlideAsync(placeholderPhoto, ct);
                placeholderPhotos.Add(createdPlaceholder);
            }
        }
        
        // Create TextSlides for post texts
        var postTextSlides = new List<TextSlideDto>();
        foreach (var postText in createDto.PostTexts)
        {
            var createdPostText = await _textSlideService.CreateTextSlideAsync(postText, ct);
            postTextSlides.Add(createdPostText);
        }
        
        var presentation = new PresentationIsroilov
        {
            TitleId = titleTextSlide.Id,
            AuthorId = authorTextSlide.Id,
            DesignId = createDto.DesignId,
            PlanId = createdPlan.Id,
            WithPhoto = createDto.WithPhoto,
            PageCount = createDto.PageCount,
            IsActive = createDto.IsActive,
            FilePath = createDto.FilePath
        };

        var createdPresentation = await _presentationRepository.AddAsync(presentation, ct);
        
        // Create pages with WithPhoto settings and placeholder photos
        int photoIndex = 0;
        int postTextIndex = 0;
        
        for (int i = 0; i < createDto.PageCount; i++)
        {
            // Get background photo from design (cycling through available photos)
            var backgroundPhotoId = design.Photos?.Count > 0 ? design.Photos[i % design.Photos.Count].Id : (int?)null;
            
            // Determine if this page should have a photo
            bool pageWithPhoto = false;
            if (createDto.PageSettings.Any())
            {
                // Use user-specified settings
                var pageSetting = createDto.PageSettings.FirstOrDefault(p => p.PageNumber == i + 1);
                pageWithPhoto = pageSetting?.WithPhoto ?? false;
            }
            else
            {
                // Default logic: first 2 pages never have photos, rest based on WithPhoto flag
                pageWithPhoto = createDto.WithPhoto && i >= 2;
            }
            
            // Get placeholder photo if this page should have one
            int? placeholderPhotoId = null;
            if (pageWithPhoto && photoIndex < placeholderPhotos.Count)
            {
                placeholderPhotoId = placeholderPhotos[photoIndex].Id;
                photoIndex++;
            }
            
            var presentationPage = new PresentationPage
            {
                PresentationIsroilovId = createdPresentation.Id,
                PhotoId = placeholderPhotoId,
                BackgroundPhotoId = backgroundPhotoId,
                WithPhoto = pageWithPhoto
            };
            
            var createdPage = await _presentationPageService.CreatePageDirectAsync(presentationPage, ct);
            
            // Create up to 3 presentation posts per page
            var postsToCreateForThisPage = Math.Min(3, postTextSlides.Count - postTextIndex);
            
            for (int j = 0; j < postsToCreateForThisPage; j++)
            {
                var createPostDto = new CreatePresentationPostDto
                {
                    PresentationPageId = createdPage.Id,
                    TextId = postTextSlides[postTextIndex].Id,
                    TitleId = null
                };
                
                await _presentationPostService.CreatePostAsync(createPostDto, ct);
                postTextIndex++;
            }
        }
        
        return await GetPresentationByIdAsync(createdPresentation.Id, ct) ?? throw new InvalidOperationException("Failed to retrieve created presentation");
    }

    public async Task<PresentationDto> UpdatePresentationPhotosAsync(int presentationId, List<IFormFile> photos, CancellationToken ct = default)
    {
        // Get presentation with pages
        var presentation = await _presentationRepository.GetByIdWithPagesAsync(presentationId, ct);
        if (presentation == null)
        {
            throw new ArgumentException($"Presentation with ID {presentationId} not found.");
        }

        if (!presentation.WithPhoto)
        {
            throw new ArgumentException("This presentation was created with WithPhoto=false. Photos cannot be added.");
        }

        // Get pages that should have photos (WithPhoto = true)
        var pagesWithPhotos = presentation.PresentationPages
            .Where(p => p.WithPhoto)
            .OrderBy(p => p.Id)
            .ToList();

        if (photos.Count != pagesWithPhotos.Count)
        {
            throw new ArgumentException($"Expected {pagesWithPhotos.Count} photos for pages that have WithPhoto=true, but got {photos.Count}.");
        }

        // Update each page's PhotoSlide with the actual uploaded photo
        for (int i = 0; i < pagesWithPhotos.Count; i++)
        {
            var page = pagesWithPhotos[i];
            var photo = photos[i];

            if (page.PhotoId.HasValue)
            {
                // Update the existing placeholder PhotoSlide with the actual photo
                await _photoSlideService.UpdatePhotoSlideFileAsync(page.PhotoId.Value, photo, ct);
            }
        }

        return await GetPresentationByIdAsync(presentationId, ct) ?? throw new InvalidOperationException("Failed to retrieve updated presentation");
    }

    public async Task<bool> PresentationExistsAsync(int id, CancellationToken ct = default)
    {
        var presentation = await _presentationRepository.GetByIdAsync(id, ct);
        return presentation != null;
    }
}
