using StudentServicesWebApi.Application.DTOs.Payment;
using StudentServicesWebApi.Domain.Enums;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface IPaymentService
{
    /// <summary>
    /// Creates a new payment (user submits payment request)
    /// </summary>
    Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createPaymentDto, int senderId, CancellationToken ct = default);

    /// <summary>
    /// Processes a payment (admin approves or rejects)
    /// </summary>
    Task<PaymentProcessResult> ProcessPaymentAsync(int paymentId, ProcessPaymentDto processPaymentDto, int adminId, CancellationToken ct = default);

    /// <summary>
    /// Gets payment details by ID
    /// </summary>
    Task<PaymentDto?> GetPaymentByIdAsync(int paymentId, CancellationToken ct = default);

    /// <summary>
    /// Gets payments for a specific user
    /// </summary>
    Task<List<PaymentSummaryDto>> GetUserPaymentsAsync(int userId, PaymentStatus? status = null, CancellationToken ct = default);

    /// <summary>
    /// Gets payments assigned to a specific admin
    /// </summary>
    Task<List<PaymentSummaryDto>> GetAdminPaymentsAsync(int adminId, PaymentStatus? status = null, CancellationToken ct = default);

    /// <summary>
    /// Gets pending payments that need admin processing
    /// </summary>
    Task<List<PaymentSummaryDto>> GetPendingPaymentsAsync(int? adminId = null, CancellationToken ct = default);

    /// <summary>
    /// Gets payments with pagination
    /// </summary>
    Task<(List<PaymentSummaryDto> Payments, int TotalCount)> GetPagedPaymentsAsync(int pageNumber, int pageSize, PaymentStatus? status = null, CancellationToken ct = default);

    /// <summary>
    /// Validates if a user can create a payment
    /// </summary>
    Task<bool> CanCreatePaymentAsync(int senderId, CancellationToken ct = default);

    /// <summary>
    /// Validates if an admin can process a specific payment
    /// </summary>
    Task<bool> CanProcessPaymentAsync(int paymentId, int adminId, CancellationToken ct = default);

    /// <summary>
    /// Gets payment statistics
    /// </summary>
    Task<PaymentStatsDto> GetPaymentStatsAsync(CancellationToken ct = default);
}

/// <summary>
/// DTO for payment statistics
/// </summary>
public class PaymentStatsDto
{
    public int TotalPayments { get; set; }
    public int PendingPayments { get; set; }
    public int ApprovedPayments { get; set; }
    public int RejectedPayments { get; set; }
    public decimal TotalRequestedAmount { get; set; }
    public decimal TotalApprovedAmount { get; set; }
}