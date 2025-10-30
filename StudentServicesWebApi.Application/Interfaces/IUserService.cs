using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Enums;
namespace StudentServicesWebApi.Application.Interfaces;
public interface IUserService
{
    Task<List<UserResponseDto>> GetUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default);
    Task<UserResponseDto?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserResponseDto> ResetUserVerificationAsync(int userId, CancellationToken cancellationToken = default);
    Task<(List<UserResponseDto> Users, int TotalCount)> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<UserResponseDto> SoftDeleteUserAsync(int userId, CancellationToken cancellationToken = default);
}
