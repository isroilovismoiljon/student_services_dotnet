using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Repositories;

public class VerificationCodeRepository : GenericRepository<VerificationCode>, IVerificationCodeRepository
{
    public VerificationCodeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<VerificationCode?> GetValidCodeAsync(int userId, string code)
    {
        var now = DateTime.UtcNow;
        return await _context.Set<VerificationCode>()
            .FirstOrDefaultAsync(vc => 
                vc.UserId == userId && 
                vc.Code == code && 
                !vc.IsUsed && 
                vc.ExpiresAt > now);
    }

    public async Task<VerificationCode?> GetByCodeAsync(string code)
    {
        var now = DateTime.UtcNow;
        return await _context.Set<VerificationCode>()
            .Include(vc => vc.User)
            .FirstOrDefaultAsync(vc => 
                vc.Code == code && 
                !vc.IsUsed &&
                vc.ExpiresAt > now);
    }

    public async Task<bool> MarkAsUsedAsync(int codeId)
    {
        var code = await GetByIdAsync(codeId);
        if (code == null) return false;

        code.IsUsed = true;
        code.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> InvalidateUserCodesAsync(int userId)
    {
        var codes = await _context.Set<VerificationCode>()
            .Where(vc => vc.UserId == userId && !vc.IsUsed)
            .ToListAsync();

        foreach (var code in codes)
        {
            code.IsUsed = true;
            code.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<VerificationCode>> GetExpiredCodesAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Set<VerificationCode>()
            .Where(vc => vc.ExpiresAt <= now && !vc.IsUsed)
            .ToListAsync();
    }
}
