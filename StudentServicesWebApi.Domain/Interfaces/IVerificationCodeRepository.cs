using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Domain.Interfaces;
public interface IVerificationCodeRepository : IGenericRepository<VerificationCode>
{
    Task<VerificationCode?> GetValidCodeAsync(int userId, string code);
    Task<VerificationCode?> GetByCodeAsync(string code);
    Task<bool> MarkAsUsedAsync(int codeId);
    Task<bool> InvalidateUserCodesAsync(int userId);
    Task<IEnumerable<VerificationCode>> GetExpiredCodesAsync();
}
