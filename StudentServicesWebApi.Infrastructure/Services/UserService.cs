using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Infrastructure.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IDtoMappingService _dtoMappingService;
    public UserService(
        IUserRepository userRepository,
        IDtoMappingService dtoMappingService)
    {
        _userRepository = userRepository;
        _dtoMappingService = dtoMappingService;
    }
    public async Task<List<UserResponseDto>> GetUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetUsersByRoleAsync(role, cancellationToken);
        return users.Select(user => _dtoMappingService.MapToUserResponseDto(user)).ToList();
    }
    public async Task<UserResponseDto?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        return user != null ? _dtoMappingService.MapToUserResponseDto(user) : null;
    }
    public async Task<UserResponseDto> ResetUserVerificationAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.ResetUserVerificationAsync(userId, cancellationToken);
        return _dtoMappingService.MapToUserResponseDto(user);
    }
    public async Task<(List<UserResponseDto> Users, int TotalCount)> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var (users, totalCount) = await _userRepository.GetAllUsersAsync(pageNumber, pageSize, cancellationToken);
        var userDtos = users.Select(user => _dtoMappingService.MapToUserResponseDto(user)).ToList();
        return (userDtos, totalCount);
    }
    public async Task<UserResponseDto> SoftDeleteUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.SoftDeleteUserAsync(userId, cancellationToken);
        return _dtoMappingService.MapToUserResponseDto(user);
    }
}
