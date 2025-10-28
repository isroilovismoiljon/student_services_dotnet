using System.Security.Claims;
using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface IJwtService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    bool ValidateToken(string token);
}
