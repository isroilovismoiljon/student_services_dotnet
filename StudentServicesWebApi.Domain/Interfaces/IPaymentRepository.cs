using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Domain.Interfaces;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<List<Payment>> GetBySenderIdAsync(int senderId, PaymentStatus? status = null, CancellationToken ct = default);
    Task<List<Payment>> GetByProcessedByAdminIdAsync(int adminId, PaymentStatus? status = null, CancellationToken ct = default);
    Task<List<Payment>> GetPendingPaymentsAsync(CancellationToken ct = default);
    Task<List<Payment>> GetProcessedByAdminAsync(int adminId, CancellationToken ct = default);
    Task<Payment?> GetWithDetailsAsync(int paymentId, CancellationToken ct = default);
    Task<List<Payment>> GetPagedAsync(int pageNumber, int pageSize, PaymentStatus? status = null, CancellationToken ct = default);
    Task<int> GetCountAsync(PaymentStatus? status = null, CancellationToken ct = default);
    Task<bool> CanBeProcessedByAdminAsync(int paymentId, int adminId, CancellationToken ct = default);
}