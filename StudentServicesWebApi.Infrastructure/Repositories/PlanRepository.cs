using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Repositories;

public class PlanRepository(AppDbContext context) : IPlanRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Plan> CreateAsync(Plan plan, CancellationToken cancellationToken = default)
    {
        _context.Plans.Add(plan);
        await _context.SaveChangesAsync(cancellationToken);
        return plan;
    }

    public async Task<Plan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Plans
            .Include(p => p.PlanText)
            .Include(p => p.Plans)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<Plan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Plans
            .Include(p => p.PlanText)
            .Include(p => p.Plans)
            .OrderBy(p => p.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<Plan> Plans, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Plans
            .Include(p => p.PlanText)
            .Include(p => p.Plans);

        var totalCount = await query.CountAsync(cancellationToken);
        var plans = await query
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (plans, totalCount);
    }

    public async Task<Plan?> UpdateAsync(Plan plan, CancellationToken cancellationToken = default)
    {
        var existingPlan = await GetByIdAsync(plan.Id, cancellationToken);
        if (existingPlan == null) return null;

        _context.Entry(existingPlan).CurrentValues.SetValues(plan);
        existingPlan.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);
        return existingPlan;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await GetByIdAsync(id, cancellationToken);
        if (plan == null) return false;

        _context.Plans.Remove(plan);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Plans.AnyAsync(p => p.Id == id, cancellationToken);
    }
}