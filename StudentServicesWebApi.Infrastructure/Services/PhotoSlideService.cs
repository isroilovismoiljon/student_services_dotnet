using StudentServicesWebApi.Application.DTOs.PhotoSlide;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using StudentServicesWebApi.Domain.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class PhotoSlideService : IPhotoSlideService
{
    private readonly IPhotoSlideRepository _photoSlideRepository;
    private readonly IDtoMappingService _dtoMappingService;
    private readonly IFileUploadService _fileUploadService;

    public PhotoSlideService(
        IPhotoSlideRepository photoSlideRepository,
        IDtoMappingService dtoMappingService,
        IFileUploadService fileUploadService)
    {
        _photoSlideRepository = photoSlideRepository;
        _dtoMappingService = dtoMappingService;
        _fileUploadService = fileUploadService;
    }

    public async Task<PhotoSlideDto> CreatePhotoSlideAsync(CreatePhotoSlideDto createPhotoSlideDto, CancellationToken ct = default)
    {
        var filePath = await _fileUploadService.UploadPresentationFileAsync(createPhotoSlideDto.Photo, null, ct);

        var photoSlide = new PhotoSlide
        {
            PhotoPath = filePath,
            OriginalFileName = createPhotoSlideDto.Photo.FileName,
            FileSize = createPhotoSlideDto.Photo.Length,
            ContentType = createPhotoSlideDto.Photo.ContentType,
            Left = createPhotoSlideDto.Left,
            Top = createPhotoSlideDto.Top,
            Width = createPhotoSlideDto.Width,
            Height = createPhotoSlideDto.Height
        };

        var createdPhotoSlide = await _photoSlideRepository.AddAsync(photoSlide, ct);
        return _dtoMappingService.MapToPhotoSlideDto(createdPhotoSlide);
    }

    public async Task<PhotoSlideDto?> UpdatePhotoSlideAsync(int id, UpdatePhotoSlideDto updatePhotoSlideDto, CancellationToken ct = default)
    {
        var existingPhotoSlide = await _photoSlideRepository.GetByIdAsync(id, ct);
        if (existingPhotoSlide == null)
        {
            return null;
        }

        if (updatePhotoSlideDto.Photo != null)
        {
            var newFilePath = await _fileUploadService.UploadPresentationFileAsync(updatePhotoSlideDto.Photo, id, ct);

            try
            {
                if (File.Exists(existingPhotoSlide.PhotoPath))
                {
                    File.Delete(existingPhotoSlide.PhotoPath);
                }
            }
            catch { }

            existingPhotoSlide.PhotoPath = newFilePath;
            existingPhotoSlide.OriginalFileName = updatePhotoSlideDto.Photo.FileName;
            existingPhotoSlide.FileSize = updatePhotoSlideDto.Photo.Length;
            existingPhotoSlide.ContentType = updatePhotoSlideDto.Photo.ContentType;
        }

        if (updatePhotoSlideDto.Left.HasValue)
            existingPhotoSlide.Left = updatePhotoSlideDto.Left.Value;
        
        if (updatePhotoSlideDto.Top.HasValue)
            existingPhotoSlide.Top = updatePhotoSlideDto.Top.Value;
        
        if (updatePhotoSlideDto.Width.HasValue)
            existingPhotoSlide.Width = updatePhotoSlideDto.Width.Value;
        
        if (updatePhotoSlideDto.Height.HasValue)
        {
            if(updatePhotoSlideDto.Height.Value == 0)
            {
                existingPhotoSlide.Height = null;
            }
            else
            {
            existingPhotoSlide.Height = updatePhotoSlideDto.Height.Value;
            }

        }

        existingPhotoSlide.UpdatedAt = DateTime.UtcNow;

        var updatedPhotoSlide = await _photoSlideRepository.UpdateAsync(existingPhotoSlide, ct);
        return _dtoMappingService.MapToPhotoSlideDto(updatedPhotoSlide);
    }

    public async Task<PhotoSlideDto?> GetPhotoSlideByIdAsync(int id, CancellationToken ct = default)
    {
        var photoSlide = await _photoSlideRepository.GetByIdAsync(id, ct);
        return photoSlide != null ? _dtoMappingService.MapToPhotoSlideDto(photoSlide) : null;
    }

    public async Task<(List<PhotoSlideSummaryDto> PhotoSlides, int TotalCount)> GetPagedPhotoSlidesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var (photoSlides, totalCount) = await _photoSlideRepository.GetPagedByCreationDateAsync(false, pageNumber, pageSize, ct);
        var photoSlideSummaries = photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
        return (photoSlideSummaries, totalCount);
    }

    public async Task<List<PhotoSlideSummaryDto>> SearchPhotoSlidesByFilenameAsync(string pattern, CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.SearchByFilenameAsync(pattern, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    public async Task<bool> DeletePhotoSlideAsync(int id, bool deleteFile = true, CancellationToken ct = default)
    {
        var photoSlide = await _photoSlideRepository.GetByIdAsync(id, ct);
        if (photoSlide == null)
        {
            return false;
        }

        if (deleteFile)
        {
            try
            {
                if (File.Exists(photoSlide.PhotoPath))
                {
                    File.Delete(photoSlide.PhotoPath);
                }
            }
            catch
            {
                // Continue with database deletion even if file deletion fails
            }
        }

        await _photoSlideRepository.DeleteAsync(photoSlide, ct);
        return true;
    }

    public async Task<BulkPhotoSlideUploadResultDto> BulkCreatePhotoSlidesAsync(BulkCreatePhotoSlideDto bulkCreateDto, CancellationToken ct = default)
    {
        var results = new List<PhotoSlideUploadResultDto>();
        var successfulUploads = 0;

        for (int i = 0; i < bulkCreateDto.Photos.Count; i++)
        {
            var photo = bulkCreateDto.Photos[i];
            var result = new PhotoSlideUploadResultDto
            {
                OriginalFileName = photo.FileName
            };

            try
            {
                // Calculate position based on layout direction
                var position = CalculatePosition(i, bulkCreateDto);

                var createDto = new CreatePhotoSlideDto
                {
                    Photo = photo,
                    Left = position.Left,
                    Top = position.Top,
                    Width = bulkCreateDto.DefaultWidth,
                    Height = bulkCreateDto.DefaultHeight
                };

                var photoSlideDto = await CreatePhotoSlideAsync(createDto, ct);
                result.Success = true;
                result.PhotoSlide = photoSlideDto;
                successfulUploads++;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            results.Add(result);
        }

        return new BulkPhotoSlideUploadResultDto
        {
            TotalAttempted = bulkCreateDto.Photos.Count,
            SuccessfulUploads = successfulUploads,
            FailedUploads = bulkCreateDto.Photos.Count - successfulUploads,
            Results = results
        };
    }

    public async Task<int> BulkDeletePhotoSlidesAsync(BulkPhotoSlideOperationDto bulkOperationDto, bool deleteFiles = true, CancellationToken ct = default)
    {
        return await _photoSlideRepository.BulkDeleteAsync(bulkOperationDto.PhotoSlideIds, deleteFiles, ct);
    }

    public async Task<PhotoSlideStatsDto> GetPhotoSlideStatsAsync(CancellationToken ct = default)
    {
        var stats = await _photoSlideRepository.GetStatsAsync(ct);

        return new PhotoSlideStatsDto
        {
            TotalPhotoSlides = stats.TotalPhotoSlides,
            TotalFileSize = stats.TotalFileSize,
            TotalFileSizeFormatted = stats.TotalFileSizeFormatted,
            AverageFileSize = stats.AverageFileSize,
            AverageFileSizeFormatted = stats.AverageFileSizeFormatted,
            LargestFileSize = stats.LargestFileSize,
            LargestFileSizeFormatted = stats.LargestFileSizeFormatted,
            SmallestFileSize = stats.SmallestFileSize,
            SmallestFileSizeFormatted = stats.SmallestFileSizeFormatted,
            UniqueFileExtensions = stats.UniqueFileExtensions,
            FileExtensionCounts = stats.FileExtensionCounts,
            ContentTypeCounts = stats.ContentTypeCounts,
            AverageWidth = stats.AverageWidth,
            AverageHeight = stats.AverageHeight,
            OrphanedFiles = stats.OrphanedFiles,
            OldestCreated = stats.OldestCreated,
            NewestCreated = stats.NewestCreated,
            MostCommonExtension = stats.MostCommonExtension,
            MostCommonContentType = stats.MostCommonContentType
        };
    }

    public async Task<bool> PhotoSlideExistsAsync(int id, CancellationToken ct = default)
    {
        var photoSlide = await _photoSlideRepository.GetByIdAsync(id, ct);
        return photoSlide != null;
    }

    public async Task<List<PhotoSlideSummaryDto>> GetRecentPhotoSlidesAsync(int count = 10, CancellationToken ct = default)
    {
        var (photoSlides, _) = await _photoSlideRepository.GetPagedByCreationDateAsync(false, 1, count, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    public async Task<PhotoSlideDto?> DuplicatePhotoSlideAsync(int id, double leftOffset = 10, double topOffset = 10, bool copyFile = true, CancellationToken ct = default)
    {
        var originalPhotoSlide = await _photoSlideRepository.GetByIdAsync(id, ct);
        if (originalPhotoSlide == null)
        {
            return null;
        }

        string newPhotoPath = originalPhotoSlide.PhotoPath;
        
        if (copyFile)
        {
            try
            {
                // Create a copy of the physical file
                var fileInfo = new FileInfo(originalPhotoSlide.PhotoPath);
                var newFileName = $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}_copy_{DateTime.UtcNow:yyyyMMddHHmmss}{fileInfo.Extension}";
                newPhotoPath = Path.Combine(fileInfo.DirectoryName!, newFileName);
                File.Copy(originalPhotoSlide.PhotoPath, newPhotoPath);
            }
            catch
            {
                // If file copy fails, use the same file path
                newPhotoPath = originalPhotoSlide.PhotoPath;
            }
        }

        var duplicatePhotoSlide = new PhotoSlide
        {
            PhotoPath = newPhotoPath,
            OriginalFileName = originalPhotoSlide.OriginalFileName,
            FileSize = originalPhotoSlide.FileSize,
            ContentType = originalPhotoSlide.ContentType,
            Left = originalPhotoSlide.Left + leftOffset,
            Top = originalPhotoSlide.Top + topOffset,
            Width = originalPhotoSlide.Width,
            Height = originalPhotoSlide.Height
        };

        var createdPhotoSlide = await _photoSlideRepository.AddAsync(duplicatePhotoSlide, ct);
        return _dtoMappingService.MapToPhotoSlideDto(createdPhotoSlide);
    }

    public async Task<PhotoSlideDto?> ReplacePhotoAsync(int id, IFormFile newPhoto, bool deleteOldFile = true, CancellationToken ct = default)
    {
        var updateDto = new UpdatePhotoSlideDto { Photo = newPhoto };
        return await UpdatePhotoSlideAsync(id, updateDto, ct);
    }

    private static (double Left, double Top) CalculatePosition(int index, BulkCreatePhotoSlideDto bulkCreateDto)
    {
        return bulkCreateDto.LayoutDirection switch
        {
            BulkLayoutDirection.Horizontal => (
                bulkCreateDto.DefaultLeft + (index * (bulkCreateDto.DefaultWidth + bulkCreateDto.Spacing)),
                bulkCreateDto.DefaultTop
            ),
            BulkLayoutDirection.Vertical => (
                bulkCreateDto.DefaultLeft,
                bulkCreateDto.DefaultTop + (index * ((bulkCreateDto.DefaultHeight ?? 100) + bulkCreateDto.Spacing))
            ),
            BulkLayoutDirection.Grid => CalculateGridPosition(index, bulkCreateDto),
            _ => (bulkCreateDto.DefaultLeft, bulkCreateDto.DefaultTop)
        };
    }

    private static (double Left, double Top) CalculateGridPosition(int index, BulkCreatePhotoSlideDto bulkCreateDto)
    {
        // Calculate grid dimensions (roughly square)
        var gridSize = (int)Math.Ceiling(Math.Sqrt(bulkCreateDto.Photos.Count));
        var row = index / gridSize;
        var col = index % gridSize;

        var left = bulkCreateDto.DefaultLeft + (col * (bulkCreateDto.DefaultWidth + bulkCreateDto.Spacing));
        var top = bulkCreateDto.DefaultTop + (row * ((bulkCreateDto.DefaultHeight ?? 100) + bulkCreateDto.Spacing));

        return (left, top);
    }
}