using StudentServicesWebApi.Application.DTOs.PhotoSlide;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace StudentServicesWebApi.Infrastructure.Services;

/// <summary>
/// Service implementation for PhotoSlide business logic operations
/// </summary>
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

    /// <inheritdoc />
    public async Task<PhotoSlideDto> CreatePhotoSlideAsync(CreatePhotoSlideDto createPhotoSlideDto, CancellationToken ct = default)
    {
        // Validate for position conflicts if needed
        var isValid = await ValidatePhotoSlideCreationAsync(createPhotoSlideDto, ct);
        if (!isValid)
        {
            throw new InvalidOperationException("A photo slide already exists at the specified position.");
        }

        // Upload the photo file using the payment receipt upload service
        // TODO: Create dedicated photo upload method in FileUploadService
        var filePath = await _fileUploadService.UploadPaymentReceiptAsync(
            createPhotoSlideDto.Photo, 
            null, // No payment ID for photo slides
            ct);

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

    /// <inheritdoc />
    public async Task<PhotoSlideDto?> UpdatePhotoSlideAsync(int id, UpdatePhotoSlideDto updatePhotoSlideDto, CancellationToken ct = default)
    {
        var existingPhotoSlide = await _photoSlideRepository.GetByIdAsync(id, ct);
        if (existingPhotoSlide == null)
        {
            return null;
        }

        // Validate for position conflicts if position is being changed
        if (updatePhotoSlideDto.Left != null || updatePhotoSlideDto.Top != null)
        {
            var isValid = await ValidatePhotoSlideUpdateAsync(id, updatePhotoSlideDto, ct);
            if (!isValid)
            {
                throw new InvalidOperationException("A photo slide already exists at the specified position.");
            }
        }

        // Handle photo replacement if a new photo is provided
        if (updatePhotoSlideDto.Photo != null)
        {
            // Upload new photo using the payment receipt upload service
            // TODO: Create dedicated photo upload method in FileUploadService
            var newFilePath = await _fileUploadService.UploadPaymentReceiptAsync(
                updatePhotoSlideDto.Photo, 
                null, // No payment ID for photo slides
                ct);

            // Delete old photo file
            try
            {
                if (File.Exists(existingPhotoSlide.PhotoPath))
                {
                    File.Delete(existingPhotoSlide.PhotoPath);
                }
            }
            catch
            {
                // Continue even if old file deletion fails
            }

            // Update photo properties
            existingPhotoSlide.PhotoPath = newFilePath;
            existingPhotoSlide.OriginalFileName = updatePhotoSlideDto.Photo.FileName;
            existingPhotoSlide.FileSize = updatePhotoSlideDto.Photo.Length;
            existingPhotoSlide.ContentType = updatePhotoSlideDto.Photo.ContentType;
        }

        // Apply position and dimension updates
        if (updatePhotoSlideDto.Left.HasValue)
            existingPhotoSlide.Left = updatePhotoSlideDto.Left.Value;
        
        if (updatePhotoSlideDto.Top.HasValue)
            existingPhotoSlide.Top = updatePhotoSlideDto.Top.Value;
        
        if (updatePhotoSlideDto.Width.HasValue)
            existingPhotoSlide.Width = updatePhotoSlideDto.Width.Value;
        
        if (updatePhotoSlideDto.Height.HasValue)
            existingPhotoSlide.Height = updatePhotoSlideDto.Height.Value;

        existingPhotoSlide.UpdatedAt = DateTime.UtcNow;

        var updatedPhotoSlide = await _photoSlideRepository.UpdateAsync(existingPhotoSlide, ct);
        return _dtoMappingService.MapToPhotoSlideDto(updatedPhotoSlide);
    }

    /// <inheritdoc />
    public async Task<PhotoSlideDto?> GetPhotoSlideByIdAsync(int id, CancellationToken ct = default)
    {
        var photoSlide = await _photoSlideRepository.GetByIdAsync(id, ct);
        return photoSlide != null ? _dtoMappingService.MapToPhotoSlideDto(photoSlide) : null;
    }

    /// <inheritdoc />
    public async Task<(List<PhotoSlideSummaryDto> PhotoSlides, int TotalCount)> GetPagedPhotoSlidesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var (photoSlides, totalCount) = await _photoSlideRepository.GetPagedByCreationDateAsync(false, pageNumber, pageSize, ct);
        var photoSlideSummaries = photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
        return (photoSlideSummaries, totalCount);
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByExtensionAsync(string extension, CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.GetByFileExtensionAsync(extension, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByFileSizeRangeAsync(long minSize, long maxSize, CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.GetByFileSizeRangeAsync(minSize, maxSize, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByContentTypeAsync(string contentType, CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.GetByContentTypeAsync(contentType, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> SearchPhotoSlidesByFilenameAsync(string pattern, CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.SearchByFilenameAsync(pattern, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByPositionAreaAsync(double minLeft, double maxLeft, double minTop, double maxTop, CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.GetByPositionAreaAsync(minLeft, maxLeft, minTop, maxTop, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByDimensionsAsync(double? minWidth = null, double? maxWidth = null, double? minHeight = null, double? maxHeight = null, CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.GetByDimensionsAsync(minWidth, maxWidth, minHeight, maxHeight, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetPhotoSlidesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.GetByDateRangeAsync(startDate, endDate, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetLargestPhotoSlidesAsync(int count = 10, CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.GetLargestPhotoSlidesAsync(count, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetOrphanedPhotoSlidesAsync(CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.GetOrphanedPhotoSlidesAsync(ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetDuplicatePhotoSlidesAsync(CancellationToken ct = default)
    {
        var photoSlides = await _photoSlideRepository.GetDuplicatePhotoSlidesAsync(ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<int> BulkDeletePhotoSlidesAsync(BulkPhotoSlideOperationDto bulkOperationDto, bool deleteFiles = true, CancellationToken ct = default)
    {
        return await _photoSlideRepository.BulkDeleteAsync(bulkOperationDto.PhotoSlideIds, deleteFiles, ct);
    }

    /// <inheritdoc />
    public async Task<bool> ValidatePhotoSlideCreationAsync(CreatePhotoSlideDto createPhotoSlideDto, CancellationToken ct = default)
    {
        // Check for duplicate positions
        var duplicateExists = await _photoSlideRepository.ExistsDuplicatePositionAsync(
            createPhotoSlideDto.Left, 
            createPhotoSlideDto.Top, 
            null, 
            ct);

        return !duplicateExists;
    }

    /// <inheritdoc />
    public async Task<bool> ValidatePhotoSlideUpdateAsync(int id, UpdatePhotoSlideDto updatePhotoSlideDto, CancellationToken ct = default)
    {
        var existingPhotoSlide = await _photoSlideRepository.GetByIdAsync(id, ct);
        if (existingPhotoSlide == null)
        {
            return false;
        }

        // Use existing values if update values are null
        var left = updatePhotoSlideDto.Left ?? existingPhotoSlide.Left;
        var top = updatePhotoSlideDto.Top ?? existingPhotoSlide.Top;

        var duplicateExists = await _photoSlideRepository.ExistsDuplicatePositionAsync(left, top, id, ct);
        return !duplicateExists;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<bool> PhotoSlideExistsAsync(int id, CancellationToken ct = default)
    {
        var photoSlide = await _photoSlideRepository.GetByIdAsync(id, ct);
        return photoSlide != null;
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetRecentPhotoSlidesAsync(int count = 10, CancellationToken ct = default)
    {
        var (photoSlides, _) = await _photoSlideRepository.GetPagedByCreationDateAsync(false, 1, count, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<PhotoSlideSummaryDto>> GetRecentlyUpdatedPhotoSlidesAsync(int count = 10, CancellationToken ct = default)
    {
        var (photoSlides, _) = await _photoSlideRepository.GetPagedByUpdateDateAsync(false, 1, count, ct);
        return photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<PhotoSlideDto?> ReplacePhotoAsync(int id, IFormFile newPhoto, bool deleteOldFile = true, CancellationToken ct = default)
    {
        var updateDto = new UpdatePhotoSlideDto { Photo = newPhoto };
        return await UpdatePhotoSlideAsync(id, updateDto, ct);
    }

    /// <inheritdoc />
    public async Task<(List<PhotoSlideSummaryDto> PhotoSlides, int TotalCount)> GetPhotoSlidesByFileSizeAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var (photoSlides, totalCount) = await _photoSlideRepository.GetPagedByFileSizeAsync(ascending, pageNumber, pageSize, ct);
        var photoSlideSummaries = photoSlides.Select(ps => _dtoMappingService.MapToPhotoSlideSummaryDto(ps)).ToList();
        return (photoSlideSummaries, totalCount);
    }

    /// <inheritdoc />
    public async Task<int> CleanupOrphanedPhotoSlidesAsync(bool removeOrphanedRecords = false, CancellationToken ct = default)
    {
        var orphanedSlides = await _photoSlideRepository.GetOrphanedPhotoSlidesAsync(ct);
        
        if (removeOrphanedRecords && orphanedSlides.Any())
        {
            var orphanedIds = orphanedSlides.Select(ps => ps.Id).ToList();
            return await _photoSlideRepository.BulkDeleteAsync(orphanedIds, false, ct); // Don't try to delete files that don't exist
        }

        return orphanedSlides.Count;
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