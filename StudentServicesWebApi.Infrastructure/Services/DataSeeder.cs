using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Infrastructure.Interfaces;

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
            // Ensure database is created
            await _context.Database.EnsureCreatedAsync();
            
            // Check if SuperAdmin user already exists
            var superAdminExists = await _context.Users
                .AnyAsync(u => u.UserRole == UserRole.SuperAdmin || u.Username == "Isroilov");

            if (!superAdminExists)
            {
                _logger.LogInformation("Creating default SuperAdmin user...");

                // Create default SuperAdmin user
                var superAdminUser = new User
                {
                    Username = "Isroilov",
                    FirstName = "Super",
                    LastName = "Admin",
                    PasswordHash = HashPassword("Ismoiljon4515"),
                    TelegramId = "1364757999",
                    UserRole = UserRole.SuperAdmin,
                    Balance = 0,
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
        // Using the same hashing method as in UserService for consistency
        // TODO: In production, use BCrypt, Argon2, or similar secure hashing
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "salt"));
    }
}