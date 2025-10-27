using StudentServicesWebApi.Application.DTOs.OpenaiKey;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Application.Services;

public class OpenaiKeyService(IOpenaiKeyRepository repository, IDtoMappingService mapper) : IOpenaiKeyService
{
    private readonly IOpenaiKeyRepository _repository = repository;
    private readonly IDtoMappingService _mapper = mapper;

    public async Task<OpenaiKeyDto> CreateAsync(CreateOpenaiKeyDto createDto, CancellationToken cancellationToken = default)
    {
        if (await _repository.KeyExistsAsync(createDto.Key, null, cancellationToken))
            throw new InvalidOperationException("An OpenAI key with this value already exists.");

        var openaiKey = new OpenaiKey
        {
            Key = createDto.Key,
            UseCount = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(openaiKey, cancellationToken);
        return await _mapper.MapToOpenaiKeyDtoAsync(created);
    }

    public async Task<OpenaiKeyDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var openaiKey = await _repository.GetByIdAsync(id, cancellationToken);
        return openaiKey == null ? null : await _mapper.MapToOpenaiKeyDtoAsync(openaiKey);
    }

    public async Task<List<OpenaiKeySummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var keys = await _repository.GetAllAsync(cancellationToken);
        var dtos = new List<OpenaiKeySummaryDto>();
        foreach (var key in keys)
        {
            dtos.Add(await _mapper.MapToOpenaiKeySummaryDtoAsync(key));
        }
        return dtos;
    }

    public async Task<OpenaiKeyDto?> UpdateAsync(int id, UpdateOpenaiKeyDto updateDto, CancellationToken cancellationToken = default)
    {
        var existingKey = await _repository.GetByIdAsync(id, cancellationToken);
        if (existingKey == null) return null;

        if (!string.IsNullOrEmpty(updateDto.Key))
        {
            if (await _repository.KeyExistsAsync(updateDto.Key, id, cancellationToken))
                throw new InvalidOperationException("An OpenAI key with this value already exists.");
            
            existingKey.Key = updateDto.Key;
        }

        if (updateDto.ResetUseCount)
            existingKey.UseCount = 0;

        var updated = await _repository.UpdateAsync(existingKey, cancellationToken);
        return updated == null ? null : await _mapper.MapToOpenaiKeyDtoAsync(updated);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.ExistsAsync(id, cancellationToken);
    }

    //public async Task<OpenaiKeyDto?> IncrementUsageAsync(int id, IncrementUsageDto? incrementDto = null, CancellationToken cancellationToken = default)
    //{
    //    var incrementBy = incrementDto?.IncrementBy ?? 1;
    //    var key = await _repository.IncrementUsageAsync(id, incrementBy, cancellationToken);
    //    return key == null ? null : await _mapper.MapToOpenaiKeyDtoAsync(key);
    //}

    public Task<string> MaskKeyForDisplay(string key)
    {
        if (string.IsNullOrEmpty(key) || key.Length < 10)
            return Task.FromResult("***");

        return Task.FromResult($"{key[..4]}****{key[^4..]}");
    }
}