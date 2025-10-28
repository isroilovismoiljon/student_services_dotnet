using StudentServicesWebApi.Application.DTOs.Plan;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;
namespace StudentServicesWebApi.Application.Services;
public class PlanService(IPlanRepository repository, IDtoMappingService mapper) : IPlanService
{
    private readonly IPlanRepository _repository = repository;
    private readonly IDtoMappingService _mapper = mapper;
    public async Task<PlanDto> CreateAsync(CreatePlanDto createDto, CancellationToken cancellationToken = default)
    {
        var plan = new Plan
        {
            PlanText = MapCreateTextSlideToTextSlide(createDto.PlanText),
            Plans = MapCreateTextSlideToTextSlide(createDto.Plans),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var created = await _repository.CreateAsync(plan, cancellationToken);
        return await _mapper.MapToPlanDtoAsync(created);
    }
    public async Task<PlanDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _repository.GetByIdAsync(id, cancellationToken);
        return plan == null ? null : await _mapper.MapToPlanDtoAsync(plan);
    }
    public async Task<List<PlanDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var plans = await _repository.GetAllAsync(cancellationToken);
        var dtos = new List<PlanDto>();
        foreach (var plan in plans)
        {
            dtos.Add(await _mapper.MapToPlanDtoAsync(plan));
        }
        return dtos;
    }
    public async Task<(List<PlanSummaryDto> Plans, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;
        var (plans, totalCount) = await _repository.GetPagedAsync(pageNumber, pageSize, cancellationToken);
        var summaryDtos = new List<PlanSummaryDto>();
        foreach (var plan in plans)
        {
            summaryDtos.Add(await _mapper.MapToPlanSummaryDtoAsync(plan));
        }
        return (summaryDtos, totalCount);
    }
    public async Task<PlanDto?> UpdateAsync(int id, UpdatePlanDto updateDto, CancellationToken cancellationToken = default)
    {
        var existingPlan = await _repository.GetByIdAsync(id, cancellationToken);
        if (existingPlan == null) return null;
        if (updateDto.PlanText != null)
        {
            UpdateTextSlide(existingPlan.PlanText, updateDto.PlanText);
        }
        if (updateDto.Plans != null)
        {
            UpdateTextSlide(existingPlan.Plans, updateDto.Plans);
        }
        existingPlan.UpdatedAt = DateTime.UtcNow;
        var updated = await _repository.UpdateAsync(existingPlan, cancellationToken);
        return updated == null ? null : await _mapper.MapToPlanDtoAsync(updated);
    }
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.ExistsAsync(id, cancellationToken);
    }
    private static TextSlide MapCreateTextSlideToTextSlide(CreateTextSlideDto dto)
    {
        return new TextSlide
        {
            Text = dto.Text,
            Size = dto.Size,
            Font = dto.Font,
            IsBold = dto.IsBold,
            IsItalic = dto.IsItalic,
            ColorHex = dto.ColorHex,
            Left = dto.Left,
            Top = dto.Top,
            Width = dto.Width,
            Height = dto.Height,
            Horizontal = dto.Horizontal,
            Vertical = dto.Vertical,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    private static void UpdateTextSlide(TextSlide textSlide, UpdateTextSlideDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Text))
            textSlide.Text = dto.Text;
        if (dto.Size.HasValue)
            textSlide.Size = dto.Size.Value;
        if (!string.IsNullOrEmpty(dto.Font))
            textSlide.Font = dto.Font;
        if (dto.IsBold.HasValue)
            textSlide.IsBold = dto.IsBold.Value;
        if (dto.IsItalic.HasValue)
            textSlide.IsItalic = dto.IsItalic.Value;
        if (!string.IsNullOrEmpty(dto.ColorHex))
            textSlide.ColorHex = dto.ColorHex;
        if (dto.Left.HasValue)
            textSlide.Left = dto.Left.Value;
        if (dto.Top.HasValue)
            textSlide.Top = dto.Top.Value;
        if (dto.Width.HasValue)
            textSlide.Width = dto.Width.Value;
        if (dto.Height.HasValue)
            textSlide.Height = dto.Height.Value;
        if (dto.Horizontal.HasValue)
            textSlide.Horizontal = dto.Horizontal.Value;
        if (dto.Vertical.HasValue)
            textSlide.Vertical = dto.Vertical.Value;
        textSlide.UpdatedAt = DateTime.UtcNow;
    }
}
