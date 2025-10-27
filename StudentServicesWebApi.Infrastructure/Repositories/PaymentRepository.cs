using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
using StudentServicesWebApi.Infrastructure.Interfaces;

namespace StudentServicesWebApi.Infrastructure.Repositories;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Payment>> GetBySenderIdAsync(int senderId, PaymentStatus? status = null, CancellationToken ct = default)
    {
        var query = _context.Payments
            .Where(p => !p.IsDeleted)
            .Include(p => p.Sender)
            .Include(p => p.ProcessedByAdmin)
            .Where(p => p.SenderId == senderId);

        if (status.HasValue)
        {
            query = query.Where(p => p.PaymentStatus == status.Value);
        }

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<Payment>> GetByProcessedByAdminIdAsync(int adminId, PaymentStatus? status = null, CancellationToken ct = default)
    {
        var query = _context.Payments
            .Where(p => !p.IsDeleted)
            .Include(p => p.Sender)
            .Include(p => p.ProcessedByAdmin)
            .Where(p => p.ProcessedByAdminId == adminId);

        if (status.HasValue)
        {
            query = query.Where(p => p.PaymentStatus == status.Value);
        }

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<Payment>> GetPendingPaymentsAsync(CancellationToken ct = default)
    {
        var query = _context.Payments
            .Where(p => !p.IsDeleted)
            .Include(p => p.Sender)
            .Include(p => p.ProcessedByAdmin)
            .Where(p => p.PaymentStatus == PaymentStatus.Waiting);

        return await query
            .OrderBy(p => p.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<Payment>> GetProcessedByAdminAsync(int adminId, CancellationToken ct = default)
    {
        return await _context.Payments
            .Where(p => !p.IsDeleted)
            .Include(p => p.Sender)
            .Include(p => p.ProcessedByAdmin)
            .Where(p => p.ProcessedByAdminId == adminId && p.PaymentStatus != PaymentStatus.Waiting)
            .OrderByDescending(p => p.ProcessedAt)
            .ToListAsync(ct);
    }

    public async Task<Payment?> GetWithDetailsAsync(int paymentId, CancellationToken ct = default)
    {
        return await _context.Payments
            .Where(p => !p.IsDeleted)
            .Include(p => p.Sender)
            .Include(p => p.ProcessedByAdmin)
            .FirstOrDefaultAsync(p => p.Id == paymentId, ct);
    }

    public async Task<List<Payment>> GetPagedAsync(int pageNumber, int pageSize, PaymentStatus? status = null, CancellationToken ct = default)
    {
        var query = _context.Payments
            .Where(p => !p.IsDeleted)
            .Include(p => p.Sender)
            .Include(p => p.ProcessedByAdmin)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(p => p.PaymentStatus == status.Value);
        }

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> GetCountAsync(PaymentStatus? status = null, CancellationToken ct = default)
    {
        var query = _context.Payments
            .Where(p => !p.IsDeleted)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(p => p.PaymentStatus == status.Value);
        }

        return await query.CountAsync(ct);
    }

    public async Task<bool> CanBeProcessedByAdminAsync(int paymentId, int adminId, CancellationToken ct = default)
    {
        var payment = await _context.Payments
            .Where(p => !p.IsDeleted)
            .FirstOrDefaultAsync(p => p.Id == paymentId, ct);

        var admins = await _context.Users
            .Where(p => !p.IsDeleted && (p.UserRole == UserRole.Admin || p.UserRole == UserRole.SuperAdmin))
            .Select(p => p.Id)
            .ToListAsync();

        if (payment == null)
            return false;

        // Payment must be in Waiting status and admin must have proper role
        return payment.PaymentStatus == PaymentStatus.Waiting && 
               admins.Contains(adminId);
    }
}