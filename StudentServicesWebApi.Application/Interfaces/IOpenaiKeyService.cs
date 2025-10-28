using StudentServicesWebApi.Application.DTOs.OpenaiKey;
namespace StudentServicesWebApi.Application.Interfaces;
public interface IOpenaiKeyService
{
    Task<OpenaiKeyDto> CreateAsync(CreateOpenaiKeyDto createDto, CancellationToken cancellationToken = default);
    Task<OpenaiKeyDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<OpenaiKeySummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OpenaiKeyDto?> UpdateAsync(int id, UpdateOpenaiKeyDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<string> MaskKeyForDisplay(string key);
}
