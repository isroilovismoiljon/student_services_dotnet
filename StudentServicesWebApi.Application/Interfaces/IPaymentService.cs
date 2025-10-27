using StudentServicesWebApi.Application.DTOs.Payment;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface IPaymentService
{
    Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createPaymentDto, int senderId, CancellationToken ct = default);
    Task<PaymentProcessResult> ProcessPaymentAsync(int paymentId, ProcessPaymentDto processPaymentDto, int adminId, CancellationToken ct = default);
    Task<PaymentDto?> GetPaymentByIdAsync(int paymentId, CancellationToken ct = default);
    Task<List<PaymentSummaryDto>> GetUserPaymentsAsync(int userId, PaymentStatus? status = null, CancellationToken ct = default);
    Task<List<PaymentSummaryDto>> GetAdminPaymentsAsync(int adminId, PaymentStatus? status = null, CancellationToken ct = default);
    Task<List<PaymentSummaryDto>> GetPendingPaymentsAsync(int? adminId = null, CancellationToken ct = default);
    Task<(List<PaymentSummaryDto> Payments, int TotalCount)> GetPagedPaymentsAsync(int pageNumber, int pageSize, PaymentStatus? status = null, CancellationToken ct = default);
    Task<bool> CanCreatePaymentAsync(int senderId, CancellationToken ct = default);
    Task<bool> CanProcessPaymentAsync(int paymentId, int adminId, CancellationToken ct = default);
    Task<PaymentStatsDto> GetPaymentStatsAsync(CancellationToken ct = default);
}
public class PaymentStatsDto
{
    public int TotalPayments { get; set; }
    public int PendingPayments { get; set; }
    public int ApprovedPayments { get; set; }
    public int RejectedPayments { get; set; }
    public decimal TotalRequestedAmount { get; set; }
    public decimal TotalApprovedAmount { get; set; }
}