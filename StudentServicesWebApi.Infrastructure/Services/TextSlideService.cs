using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

/// <summary>
/// Service implementation for TextSlide business logic operations
/// </summary>
public class TextSlideService : ITextSlideService
{
    private readonly ITextSlideRepository _textSlideRepository;
    private readonly IDtoMappingService _dtoMappingService;

    public TextSlideService(
        ITextSlideRepository textSlideRepository,
        IDtoMappingService dtoMappingService)
    {
        _textSlideRepository = textSlideRepository;
        _dtoMappingService = dtoMappingService;
    }

    /// <inheritdoc />
    public async Task<TextSlideDto> CreateTextSlideAsync(CreateTextSlideDto createTextSlideDto, CancellationToken ct = default)
    {
        // Validate for duplicates if needed
        var isValid = await ValidateTextSlideCreationAsync(createTextSlideDto, ct);
        if (!isValid)
        {
            throw new InvalidOperationException("A text slide with the same content and position already exists.");
        }

        var textSlide = new TextSlide
        {
            Text = createTextSlideDto.Text,
            Size = createTextSlideDto.Size,
            Font = createTextSlideDto.Font,
            IsBold = createTextSlideDto.IsBold,
            IsItalic = createTextSlideDto.IsItalic,
            ColorHex = createTextSlideDto.ColorHex,
            Left = createTextSlideDto.Left,
            Top = createTextSlideDto.Top,
            Width = createTextSlideDto.Width,
            Height = createTextSlideDto.Height,
            Horizontal = createTextSlideDto.Horizontal,
            Vertical = createTextSlideDto.Vertical
        };

        var createdTextSlide = await _textSlideRepository.AddAsync(textSlide, ct);
        return _dtoMappingService.MapToTextSlideDto(createdTextSlide);
    }

    /// <inheritdoc />
    public async Task<TextSlideDto?> UpdateTextSlideAsync(int id, UpdateTextSlideDto updateTextSlideDto, CancellationToken ct = default)
    {
        var existingTextSlide = await _textSlideRepository.GetByIdAsync(id, ct);
        if (existingTextSlide == null)
        {
            return null;
        }

        // Validate for duplicates if position or text is being changed
        if (updateTextSlideDto.Text != null || updateTextSlideDto.Left != null || updateTextSlideDto.Top != null)
        {
            var isValid = await ValidateTextSlideUpdateAsync(id, updateTextSlideDto, ct);
            if (!isValid)
            {
                throw new InvalidOperationException("A text slide with the same content and position already exists.");
            }
        }

        // Apply updates only for non-null properties
        if (updateTextSlideDto.Text != null)
            existingTextSlide.Text = updateTextSlideDto.Text;
        
        if (updateTextSlideDto.Size.HasValue)
            existingTextSlide.Size = updateTextSlideDto.Size.Value;
        
        if (updateTextSlideDto.Font != null)
            existingTextSlide.Font = updateTextSlideDto.Font;
        
        if (updateTextSlideDto.IsBold.HasValue)
            existingTextSlide.IsBold = updateTextSlideDto.IsBold.Value;
        
        if (updateTextSlideDto.IsItalic.HasValue)
            existingTextSlide.IsItalic = updateTextSlideDto.IsItalic.Value;
        
        if (updateTextSlideDto.ColorHex != null)
            existingTextSlide.ColorHex = updateTextSlideDto.ColorHex;
        
        if (updateTextSlideDto.Left.HasValue)
            existingTextSlide.Left = updateTextSlideDto.Left.Value;
        
        if (updateTextSlideDto.Top.HasValue)
            existingTextSlide.Top = updateTextSlideDto.Top.Value;
        
        if (updateTextSlideDto.Width.HasValue)
            existingTextSlide.Width = updateTextSlideDto.Width.Value;
        
        if (updateTextSlideDto.Height.HasValue)
            existingTextSlide.Height = updateTextSlideDto.Height.Value;
        
        if (updateTextSlideDto.Horizontal.HasValue)
            existingTextSlide.Horizontal = updateTextSlideDto.Horizontal.Value;
        
        if (updateTextSlideDto.Vertical.HasValue)
            existingTextSlide.Vertical = updateTextSlideDto.Vertical.Value;

        existingTextSlide.UpdatedAt = DateTime.UtcNow;

        var updatedTextSlide = await _textSlideRepository.UpdateAsync(existingTextSlide, ct);
        return _dtoMappingService.MapToTextSlideDto(updatedTextSlide);
    }

    /// <inheritdoc />
    public async Task<TextSlideDto?> GetTextSlideByIdAsync(int id, CancellationToken ct = default)
    {
        var textSlide = await _textSlideRepository.GetByIdAsync(id, ct);
        return textSlide != null ? _dtoMappingService.MapToTextSlideDto(textSlide) : null;
    }

    /// <inheritdoc />
    public async Task<(List<TextSlideSummaryDto> TextSlides, int TotalCount)> GetPagedTextSlidesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var (textSlides, totalCount) = await _textSlideRepository.GetPagedByCreationDateAsync(false, pageNumber, pageSize, ct);
        var textSlideSummaries = textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
        return (textSlideSummaries, totalCount);
    }

    /// <inheritdoc />
    public async Task<List<TextSlideSummaryDto>> GetTextSlidesByFontAsync(string font, CancellationToken ct = default)
    {
        var textSlides = await _textSlideRepository.GetByFontAsync(font, ct);
        return textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TextSlideSummaryDto>> GetTextSlidesBySizeRangeAsync(int minSize, int maxSize, CancellationToken ct = default)
    {
        var textSlides = await _textSlideRepository.GetBySizeRangeAsync(minSize, maxSize, ct);
        return textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TextSlideSummaryDto>> GetTextSlidesByColorAsync(string colorHex, CancellationToken ct = default)
    {
        var textSlides = await _textSlideRepository.GetByColorAsync(colorHex, ct);
        return textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TextSlideSummaryDto>> GetTextSlidesByFormattingAsync(bool? isBold = null, bool? isItalic = null, CancellationToken ct = default)
    {
        var textSlides = await _textSlideRepository.GetByFormattingAsync(isBold, isItalic, ct);
        return textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TextSlideSummaryDto>> SearchTextSlidesAsync(string searchTerm, CancellationToken ct = default)
    {
        var textSlides = await _textSlideRepository.SearchByTextAsync(searchTerm, ct);
        return textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TextSlideSummaryDto>> GetTextSlidesByPositionAreaAsync(double minLeft, double maxLeft, double minTop, double maxTop, CancellationToken ct = default)
    {
        var textSlides = await _textSlideRepository.GetByPositionAreaAsync(minLeft, maxLeft, minTop, maxTop, ct);
        return textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TextSlideSummaryDto>> GetTextSlidesByDimensionsAsync(double? minWidth = null, double? maxWidth = null, double? minHeight = null, double? maxHeight = null, CancellationToken ct = default)
    {
        var textSlides = await _textSlideRepository.GetByDimensionsAsync(minWidth, maxWidth, minHeight, maxHeight, ct);
        return textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TextSlideSummaryDto>> GetTextSlidesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        var textSlides = await _textSlideRepository.GetByDateRangeAsync(startDate, endDate, ct);
        return textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<bool> DeleteTextSlideAsync(int id, CancellationToken ct = default)
    {
        var textSlide = await _textSlideRepository.GetByIdAsync(id, ct);
        if (textSlide == null)
        {
            return false;
        }

        await _textSlideRepository.DeleteAsync(textSlide, ct);
        return true;
    }

    /// <inheritdoc />
    public async Task<List<TextSlideDto>> BulkCreateTextSlidesAsync(BulkCreateTextSlideDto bulkCreateDto, CancellationToken ct = default)
    {
        var createdTextSlides = new List<TextSlide>();

        foreach (var createDto in bulkCreateDto.TextSlides)
        {
            // Validate each text slide
            var isValid = await ValidateTextSlideCreationAsync(createDto, ct);
            if (!isValid)
            {
                throw new InvalidOperationException($"A text slide with the same content and position already exists: '{createDto.Text}' at ({createDto.Left}, {createDto.Top})");
            }

            var textSlide = new TextSlide
            {
                Text = createDto.Text,
                Size = createDto.Size,
                Font = createDto.Font,
                IsBold = createDto.IsBold,
                IsItalic = createDto.IsItalic,
                ColorHex = createDto.ColorHex,
                Left = createDto.Left,
                Top = createDto.Top,
                Width = createDto.Width,
                Height = createDto.Height,
                Horizontal = createDto.Horizontal,
                Vertical = createDto.Vertical
            };

            var createdTextSlide = await _textSlideRepository.AddAsync(textSlide, ct);
            createdTextSlides.Add(createdTextSlide);
        }

        return createdTextSlides.Select(ts => _dtoMappingService.MapToTextSlideDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<int> BulkDeleteTextSlidesAsync(BulkTextSlideOperationDto bulkOperationDto, CancellationToken ct = default)
    {
        return await _textSlideRepository.BulkDeleteAsync(bulkOperationDto.TextSlideIds, ct);
    }

    /// <inheritdoc />
    public async Task<bool> ValidateTextSlideCreationAsync(CreateTextSlideDto createTextSlideDto, CancellationToken ct = default)
    {
        // Check for duplicate text slides with same content and position
        var duplicateExists = await _textSlideRepository.ExistsDuplicateAsync(
            createTextSlideDto.Text, 
            createTextSlideDto.Left, 
            createTextSlideDto.Top, 
            null, 
            ct);

        return !duplicateExists;
    }

    /// <inheritdoc />
    public async Task<bool> ValidateTextSlideUpdateAsync(int id, UpdateTextSlideDto updateTextSlideDto, CancellationToken ct = default)
    {
        var existingTextSlide = await _textSlideRepository.GetByIdAsync(id, ct);
        if (existingTextSlide == null)
        {
            return false;
        }

        // Use existing values if update values are null
        var text = updateTextSlideDto.Text ?? existingTextSlide.Text;
        var left = updateTextSlideDto.Left ?? existingTextSlide.Left;
        var top = updateTextSlideDto.Top ?? existingTextSlide.Top;

        var duplicateExists = await _textSlideRepository.ExistsDuplicateAsync(text, left, top, id, ct);
        return !duplicateExists;
    }

    /// <inheritdoc />
    public async Task<TextSlideStatsDto> GetTextSlideStatsAsync(CancellationToken ct = default)
    {
        var stats = await _textSlideRepository.GetStatsAsync(ct);
        
        // Get the most used font and color
        var allTextSlides = await _textSlideRepository.ListAsync(ct: ct);
        var mostUsedFont = allTextSlides.GroupBy(ts => ts.Font)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key ?? string.Empty;
        
        var mostUsedColor = allTextSlides.GroupBy(ts => ts.ColorHex)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key ?? string.Empty;

        return new TextSlideStatsDto
        {
            TotalTextSlides = stats.TotalTextSlides,
            UniqueFonts = stats.UniqueFonts,
            UniqueColors = stats.UniqueColors,
            BoldTextSlides = stats.BoldTextSlides,
            ItalicTextSlides = stats.ItalicTextSlides,
            AverageTextLength = stats.AverageTextLength,
            MinSize = stats.MinSize,
            MaxSize = stats.MaxSize,
            AverageSize = stats.AverageSize,
            OldestCreated = stats.OldestCreated,
            NewestCreated = stats.NewestCreated,
            MostUsedFont = mostUsedFont,
            MostUsedColor = mostUsedColor
        };
    }

    /// <inheritdoc />
    public async Task<bool> TextSlideExistsAsync(int id, CancellationToken ct = default)
    {
        var textSlide = await _textSlideRepository.GetByIdAsync(id, ct);
        return textSlide != null;
    }

    /// <inheritdoc />
    public async Task<List<TextSlideSummaryDto>> GetRecentTextSlidesAsync(int count = 10, CancellationToken ct = default)
    {
        var (textSlides, _) = await _textSlideRepository.GetPagedByCreationDateAsync(false, 1, count, ct);
        return textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TextSlideSummaryDto>> GetRecentlyUpdatedTextSlidesAsync(int count = 10, CancellationToken ct = default)
    {
        var (textSlides, _) = await _textSlideRepository.GetPagedByUpdateDateAsync(false, 1, count, ct);
        return textSlides.Select(ts => _dtoMappingService.MapToTextSlideSummaryDto(ts)).ToList();
    }

    /// <inheritdoc />
    public async Task<TextSlideDto?> DuplicateTextSlideAsync(int id, double leftOffset = 10, double topOffset = 10, CancellationToken ct = default)
    {
        var originalTextSlide = await _textSlideRepository.GetByIdAsync(id, ct);
        if (originalTextSlide == null)
        {
            return null;
        }

        var duplicateTextSlide = new TextSlide
        {
            Text = originalTextSlide.Text,
            Size = originalTextSlide.Size,
            Font = originalTextSlide.Font,
            IsBold = originalTextSlide.IsBold,
            IsItalic = originalTextSlide.IsItalic,
            ColorHex = originalTextSlide.ColorHex,
            Left = originalTextSlide.Left + leftOffset,
            Top = originalTextSlide.Top + topOffset,
            Width = originalTextSlide.Width,
            Height = originalTextSlide.Height,
            Horizontal = originalTextSlide.Horizontal,
            Vertical = originalTextSlide.Vertical
        };

        var createdTextSlide = await _textSlideRepository.AddAsync(duplicateTextSlide, ct);
        return _dtoMappingService.MapToTextSlideDto(createdTextSlide);
    }
}