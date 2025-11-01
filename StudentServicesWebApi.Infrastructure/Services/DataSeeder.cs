using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Interfaces;
namespace StudentServicesWebApi.Infrastructure.Services;
public class DataSeeder : IDataSeeder
{
    private readonly AppDbContext _context;
    private readonly ILogger<DataSeeder> _logger;
    public DataSeeder(AppDbContext context, ILogger<DataSeeder> logger)
    {   
        _context = context;
        _logger = logger;
    }
    public async Task SeedAsync()
    {
        try
        {
            await _context.Database.EnsureCreatedAsync();
            var superAdminExists = await _context.Users
                .Where(u => !u.IsDeleted)
                .AnyAsync(u => u.UserRole == UserRole.SuperAdmin || u.Username == "Isroilov");
            if (!superAdminExists)
            {
                // Delete any existing unverified or deleted users with this username
                var existingUsers = await _context.Users
                    .Where(u => u.Username == "Isroilov")
                    .ToListAsync();
                if (existingUsers.Any())
                {
                    _context.Users.RemoveRange(existingUsers);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Removed {existingUsers.Count} existing user(s) with username 'Isroilov'");
                }
                _logger.LogInformation("Creating default SuperAdmin user...");
                var superAdminUser = new User
                {
                    Username = "Isroilov",
                    FirstName = "Super",
                    LastName = "Admin",
                    PasswordHash = HashPassword("Ismoiljon4515"),
                    TelegramId = "1364757999",
                    UserRole = UserRole.SuperAdmin,
                    Balance = 0,
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _context.Users.AddAsync(superAdminUser);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Default SuperAdmin user created successfully!");
                _logger.LogInformation("Username: Isroilov");
                _logger.LogInformation("TelegramId: 1364757999");
                _logger.LogInformation("Role: SuperAdmin");
            }
            else
            {
                _logger.LogInformation("SuperAdmin user already exists, skipping creation.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while seeding database");
            throw;
        }
    }
    private string HashPassword(string password)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "salt"));
    }
}
