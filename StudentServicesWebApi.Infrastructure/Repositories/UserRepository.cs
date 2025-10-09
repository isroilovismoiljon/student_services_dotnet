using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<User?> GetByTelegramIdAsync(string telegramId)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.TelegramId == telegramId);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        // Assuming you might add email field later
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == email);
    }

    public async Task<bool> ExistsAsync(string? username = null, string? phoneNumber = null, string? telegramId = null, string? email = null)
    {
        var query = _context.Set<User>().AsQueryable();

        if (!string.IsNullOrEmpty(username))
            query = query.Where(u => u.Username.ToLower() == username.ToLower());

        if (!string.IsNullOrEmpty(phoneNumber))
            query = query.Where(u => u.PhoneNumber == phoneNumber);

        if (!string.IsNullOrEmpty(telegramId))
            query = query.Where(u => u.TelegramId == telegramId);

        return await query.AnyAsync();
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        // Hash the password for comparison (assuming you'll implement password hashing)
        var hashedPassword = HashPassword(password);

        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == hashedPassword);
    }

    public async Task<bool> UpdateTelegramIdAsync(int userId, string telegramId)
    {
        var user = await GetByIdAsync(userId);
        if (user == null) return false;

        // Check if TelegramId already exists for another user
        var existsForOtherUser = await _context.Set<User>()
            .AnyAsync(u => u.TelegramId == telegramId && u.Id != userId);
            
        if (existsForOtherUser)
        {
            throw new InvalidOperationException($"TelegramId {telegramId} is already linked to another user");
        }

        user.TelegramId = telegramId;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TelegramIdExistsAsync(string telegramId)
    {
        return await _context.Set<User>()
            .AnyAsync(u => u.TelegramId == telegramId);
    }

    public async Task<IEnumerable<User>> GetReferralsByUserIdAsync(int userId)
    {
        return await _context.Set<User>()
            .Where(u => u.ReferralId == userId)
            .ToListAsync();
    }

    public async Task<bool> VerifiedUsernameExistsAsync(string username)
    {
        return await _context.Set<User>()
            .AnyAsync(u => u.Username.ToLower() == username.ToLower() && u.IsVerified);
    }

    public async Task<User?> GetVerifiedUserByUsernameAsync(string username)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower() && u.IsVerified);
    }

    public async Task<bool> SetUserVerifiedAsync(int userId)
    {
        var user = await GetByIdAsync(userId);
        if (user == null) return false;

        user.IsVerified = true;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUnverifiedUsersByUsernameAsync(string username)
    {
        var unverifiedUsers = await _context.Set<User>()
            .Where(u => u.Username.ToLower() == username.ToLower() && !u.IsVerified)
            .ToListAsync();

        if (!unverifiedUsers.Any()) return false;

        _context.Set<User>().RemoveRange(unverifiedUsers);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<User>> GetUsersByRoleAsync(Domain.Enums.UserRole role, CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>()
            .Where(u => u.UserRole == role)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    // Simple password hashing - in production use BCrypt or similar
    private string HashPassword(string password)
    {
        // TODO: Implement proper password hashing (BCrypt, Argon2, etc.)
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "salt"));
    }
}
