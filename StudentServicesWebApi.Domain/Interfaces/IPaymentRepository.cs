using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Domain.Interfaces;

public interface IPaymentRepository : IGenericRepository<Payment>
{

    /// Gets payments by sender (user) ID with optional status filter
    Task<List<Payment>> GetBySenderIdAsync(int senderId, PaymentStatus? status = null, CancellationToken ct = default);

    /// Gets payments by processed admin ID with optional status filter
    Task<List<Payment>> GetByProcessedByAdminIdAsync(int adminId, PaymentStatus? status = null, CancellationToken ct = default);

    /// Gets payments that can be processed (status = Waiting)
    Task<List<Payment>> GetPendingPaymentsAsync(CancellationToken ct = default);

    /// Gets payments processed by a specific admin    
    Task<List<Payment>> GetProcessedByAdminAsync(int adminId, CancellationToken ct = default);

    /// Gets payment with full details (includes navigation properties)
    Task<Payment?> GetWithDetailsAsync(int paymentId, CancellationToken ct = default);

    /// Gets payments with pagination    
    Task<List<Payment>> GetPagedAsync(int pageNumber, int pageSize, PaymentStatus? status = null, CancellationToken ct = default);

    /// Gets total count of payments with optional status filter
    Task<int> GetCountAsync(PaymentStatus? status = null, CancellationToken ct = default);

    /// Checks if a payment can be processed by a specific admin
    Task<bool> CanBeProcessedByAdminAsync(int paymentId, int adminId, CancellationToken ct = default);
}