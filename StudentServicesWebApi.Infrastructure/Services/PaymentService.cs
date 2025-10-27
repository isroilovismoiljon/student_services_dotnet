using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Application.DTOs.Payment;
using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDtoMappingService _dtoMappingService;
    private readonly AppDbContext _context;
    private readonly IFileUploadService _fileUploadService;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IUserRepository userRepository,
        IDtoMappingService dtoMappingService,
        AppDbContext context,
        IFileUploadService fileUploadService)
    {
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
        _dtoMappingService = dtoMappingService;
        _context = context;
        _fileUploadService = fileUploadService;
    }

    public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createPaymentDto, int senderId, CancellationToken ct = default)
    {
        // Validate sender is a User
        var sender = await _userRepository.GetByIdAsync(senderId, ct);
        if (sender == null || sender.UserRole != UserRole.User)
        {
            throw new UnauthorizedAccessException("Only users with User role can create payments.");
        }

        // Handle photo upload using the file upload service
        string photoPath = await _fileUploadService.UploadPaymentReceiptAsync(createPaymentDto.Photo, null, ct);

        var payment = new Payment
        {
            SenderId = senderId,
            RequestedAmount = createPaymentDto.RequestedAmount,
            Photo = photoPath,
            Description = createPaymentDto.Description,
            PaymentStatus = PaymentStatus.Waiting,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdPayment = await _paymentRepository.AddAsync(payment, ct);
        var paymentWithDetails = await _paymentRepository.GetWithDetailsAsync(createdPayment.Id, ct);

        return _dtoMappingService.MapToPaymentDto(paymentWithDetails!);
    }

    public async Task<PaymentProcessResult> ProcessPaymentAsync(int paymentId, ProcessPaymentDto processPaymentDto, int adminId, CancellationToken ct = default)
    {
        // Validate admin role
        var admin = await _userRepository.GetByIdAsync(adminId, ct);
        if (admin == null || (admin.UserRole != UserRole.Admin && admin.UserRole != UserRole.SuperAdmin))
        {
            return new PaymentProcessResult
            {
                Success = false,
                Message = "Only Admins or SuperAdmins can process payments.",
                ResultCode = PaymentProcessResultCode.Unauthorized
            };
        }

        // Get payment with details
        var payment = await _paymentRepository.GetWithDetailsAsync(paymentId, ct);
        if (payment == null)
        {
            return new PaymentProcessResult
            {
                Success = false,
                Message = "Payment not found.",
                ResultCode = PaymentProcessResultCode.NotFound
            };
        }

        // Validate payment status transitions
        var transitionResult = ValidateStatusTransition(payment.PaymentStatus, processPaymentDto.PaymentStatus);
        if (!transitionResult.IsValid)
        {
            return new PaymentProcessResult
            {
                Success = transitionResult.ResultCode == PaymentProcessResultCode.AlreadySuccess, // AlreadySuccess is considered successful
                Message = transitionResult.Message,
                ResultCode = transitionResult.ResultCode,
                Payment = transitionResult.ResultCode == PaymentProcessResultCode.AlreadySuccess ? _dtoMappingService.MapToPaymentDto(payment) : null
            };
        }

        // Remove any existing recipient assignment logic since we only use ProcessedByAdmin

        // Validate business rules based on status
        if (processPaymentDto.PaymentStatus == PaymentStatus.Success)
        {
            var user = await _userRepository.GetByIdAsync(payment.SenderId);
            var balance = 0;
            if (user != null)
                balance = user.Balance;

            if (!processPaymentDto.ApprovedAmount.HasValue || processPaymentDto.ApprovedAmount <= 0)
            {
                payment.ApprovedAmount = payment.RequestedAmount;
            }
            else
            {
                payment.ApprovedAmount = processPaymentDto.ApprovedAmount;
            }

            if (user != null)
            {
                user.Balance = balance + (int)payment.FinalAmount;
                await _userRepository.UpdateAsync(user, ct);
            }

            payment.PaymentStatus = PaymentStatus.Success;

        }
        else if (processPaymentDto.PaymentStatus == PaymentStatus.Rejected)
        {
            if (string.IsNullOrWhiteSpace(processPaymentDto.RejectReason))
            {
                throw new ArgumentException("Reject reason is required when rejecting payment.");
            }

            payment.RejectReason = processPaymentDto.RejectReason;
            payment.PaymentStatus = PaymentStatus.Rejected;
            payment.ApprovedAmount = null; // Clear approved amount for rejected payments
        }
        else
        {
            throw new ArgumentException("Invalid payment status for processing.");
        }

        // Set processing details
        payment.ProcessedByAdminId = adminId;
        payment.ProcessedAt = DateTime.UtcNow;
        payment.AdminNotes = processPaymentDto.AdminNotes;
        payment.UpdatedAt = DateTime.UtcNow;

        await _paymentRepository.UpdateAsync(payment, ct);

        return new PaymentProcessResult
        {
            Success = true,
            Message = processPaymentDto.PaymentStatus == PaymentStatus.Success ? "Payment approved successfully." : "Payment rejected successfully.",
            ResultCode = PaymentProcessResultCode.Success,
            Payment = _dtoMappingService.MapToPaymentDto(payment)
        };
    }

    public async Task<PaymentDto?> GetPaymentByIdAsync(int paymentId, CancellationToken ct = default)
    {
        var payment = await _paymentRepository.GetWithDetailsAsync(paymentId, ct);
        return payment == null ? null : _dtoMappingService.MapToPaymentDto(payment);
    }

    public async Task<List<PaymentSummaryDto>> GetUserPaymentsAsync(int userId, PaymentStatus? status = null, CancellationToken ct = default)
    {
        var payments = await _paymentRepository.GetBySenderIdAsync(userId, status, ct);
        return payments.Select(_dtoMappingService.MapToPaymentSummaryDto).ToList();
    }

    public async Task<List<PaymentSummaryDto>> GetAdminPaymentsAsync(int adminId, PaymentStatus? status = null, CancellationToken ct = default)
    {
        var payments = await _paymentRepository.GetByProcessedByAdminIdAsync(adminId, status, ct);
        return payments.Select(_dtoMappingService.MapToPaymentSummaryDto).ToList();
    }

    public async Task<List<PaymentSummaryDto>> GetPendingPaymentsAsync(int? adminId = null, CancellationToken ct = default)
    {
        var payments = await _paymentRepository.GetPendingPaymentsAsync(ct);
        return payments.Select(_dtoMappingService.MapToPaymentSummaryDto).ToList();
    }

    public async Task<(List<PaymentSummaryDto> Payments, int TotalCount)> GetPagedPaymentsAsync(int pageNumber, int pageSize, PaymentStatus? status = null, CancellationToken ct = default)
    {
        var payments = await _paymentRepository.GetPagedAsync(pageNumber, pageSize, status, ct);
        var totalCount = await _paymentRepository.GetCountAsync(status, ct);

        return (payments.Select(_dtoMappingService.MapToPaymentSummaryDto).ToList(), totalCount);
    }

    public async Task<bool> CanCreatePaymentAsync(int senderId, CancellationToken ct = default)
    {
        var sender = await _userRepository.GetByIdAsync(senderId, ct);
        return sender != null && sender.UserRole == UserRole.User;
    }

    public async Task<bool> CanProcessPaymentAsync(int paymentId, int adminId, CancellationToken ct = default)
    {
        return await _paymentRepository.CanBeProcessedByAdminAsync(paymentId, adminId, ct);
    }

    public async Task<PaymentStatsDto> GetPaymentStatsAsync(CancellationToken ct = default)
    {
        var allPayments = await _context.Payments.ToListAsync(ct);

        return new PaymentStatsDto
        {
            TotalPayments = allPayments.Count,
            PendingPayments = allPayments.Count(p => p.PaymentStatus == PaymentStatus.Waiting),
            ApprovedPayments = allPayments.Count(p => p.PaymentStatus == PaymentStatus.Success),
            RejectedPayments = allPayments.Count(p => p.PaymentStatus == PaymentStatus.Rejected),
            TotalRequestedAmount = allPayments.Sum(p => p.RequestedAmount),
            TotalApprovedAmount = allPayments.Where(p => p.ApprovedAmount.HasValue).Sum(p => p.ApprovedAmount!.Value)
        };
    }
    private (bool IsValid, string Message, PaymentProcessResultCode ResultCode) ValidateStatusTransition(PaymentStatus currentStatus, PaymentStatus newStatus)
    {
        // Rule 1: If trying to approve an already successful payment
        if (currentStatus == PaymentStatus.Success && newStatus == PaymentStatus.Success)
        {
            return (false, "AlreadySuccess", PaymentProcessResultCode.AlreadySuccess);
        }

        // Rule 2: Cannot change back to Waiting from Success or Rejected
        if ((currentStatus == PaymentStatus.Success || currentStatus == PaymentStatus.Rejected) && newStatus == PaymentStatus.Waiting)
        {
            return (false, "Cannot change processed payments back to waiting status.", PaymentProcessResultCode.InvalidTransition);
        }

        // Rule 3: Valid transitions
        // Waiting → Success ✅
        // Waiting → Rejected ✅  
        // Rejected → Success ✅
        if ((currentStatus == PaymentStatus.Waiting && (newStatus == PaymentStatus.Success || newStatus == PaymentStatus.Rejected)) ||
            (currentStatus == PaymentStatus.Rejected && newStatus == PaymentStatus.Success))
        {
            return (true, "Valid transition", PaymentProcessResultCode.Success);
        }

        // Rule 4: Invalid transitions (like Success → Rejected)
        return (false, $"Invalid status transition from {currentStatus} to {newStatus}.", PaymentProcessResultCode.InvalidTransition);
    }
}
