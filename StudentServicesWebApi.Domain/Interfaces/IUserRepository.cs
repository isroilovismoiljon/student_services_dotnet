using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByPhoneNumberAsync(string phoneNumber);
    Task<User?> GetByTelegramIdAsync(string telegramId);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsAsync(string? username = null, string? phoneNumber = null, string? telegramId = null, string? email = null);
    Task<User?> AuthenticateAsync(string username, string password);
    Task<bool> UpdateTelegramIdAsync(int userId, string telegramId);
    Task<bool> TelegramIdExistsAsync(string telegramId);
    Task<IEnumerable<User>> GetReferralsByUserIdAsync(int userId);
    Task<bool> VerifiedUsernameExistsAsync(string username);
    Task<User?> GetVerifiedUserByUsernameAsync(string username);
    Task<bool> SetUserVerifiedAsync(int userId);
    Task<bool> DeleteUnverifiedUsersByUsernameAsync(string username);
    Task<List<User>> GetUsersByRoleAsync(Domain.Enums.UserRole role, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);
}
