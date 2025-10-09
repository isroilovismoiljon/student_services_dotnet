using StudentServicesWebApi.Application.DTOs.Design;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class DesignService : IDesignService
{
    private readonly IDesignRepository _designRepository;
    private readonly IPhotoSlideService _photoSlideService;
    private readonly IDtoMappingService _dtoMappingService;

    public DesignService(
        IDesignRepository designRepository,
        IDtoMappingService dtoMappingService,
        IPhotoSlideService photoSlideService)
    {
        _designRepository = designRepository;
        _dtoMappingService = dtoMappingService;
        _photoSlideService = photoSlideService;
    }

    public async Task<DesignDto> CreateDesignAsync(DesignCreateWithPhotosDto createDesignWithPhotosDto, int createdByUserId, CancellationToken cancellationToken = default)
    {
        if (createDesignWithPhotosDto.Photos == null || createDesignWithPhotosDto.Photos.Length < 4)
        {
            throw new ArgumentException("At least 4 photos are required to create a design.");
        }

        var design = await _designRepository.CreateDesignWithPhotosAsync(
            createDesignWithPhotosDto.Title,
            createdByUserId,
            createDesignWithPhotosDto.Photos,
            cancellationToken);

        if (design == null)
            throw new InvalidOperationException("Failed to create design with photos");

        return _dtoMappingService.MapToDesignDto(design);
    }

    public async Task<DesignDto?> GetDesignByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var design = await _designRepository.GetByIdWithPhotosAsync(id, cancellationToken);
        return design != null ? _dtoMappingService.MapToDesignDto(design) : null;
    }

    public async Task<(List<DesignSummaryDto> Designs, int TotalCount)> GetPagedDesignsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var (designs, totalCount) = await _designRepository.GetPagedAsync(pageNumber, pageSize, cancellationToken);
        var designSummaries = designs.Select(d => _dtoMappingService.MapToDesignSummaryDto(d)).ToList();
        return (designSummaries, totalCount);
    }

    public async Task<DesignDto?> UpdateDesignAsync(int id, UpdateDesignDto updateDesignDto, CancellationToken cancellationToken = default)
    {
        var design = await _designRepository.GetByIdAsync(id, cancellationToken);
        if (design == null)
            return null;

        if (!string.IsNullOrWhiteSpace(updateDesignDto.Title))
            design.Title = updateDesignDto.Title;

        design.UpdatedAt = DateTime.UtcNow;

        await _designRepository.UpdateAsync(design, cancellationToken);
        
        var updatedDesign = await _designRepository.GetByIdWithPhotosAsync(id, cancellationToken);
        return updatedDesign != null ? _dtoMappingService.MapToDesignDto(updatedDesign) : null;
    }

    public async Task<bool> DeleteDesignAsync(int id, CancellationToken cancellationToken = default)
    {
        var design = await _designRepository.GetByIdAsync(id, cancellationToken);
        if (design == null)
            return false;

        var designDto = _dtoMappingService.MapToDesignDto(design);

        List<int> photoSlideIds = designDto.Photos.Select(p => p.Id).ToList();
        try
        {
            await _photoSlideService.BulkDeletePhotoSlidesAsync(
                new Application.DTOs.PhotoSlide.BulkPhotoSlideOperationDto{
                    PhotoSlideIds = photoSlideIds
                }
            );
        }
        catch { }

        await _designRepository.DeleteAsync(design, cancellationToken);
        return true;
    }
}