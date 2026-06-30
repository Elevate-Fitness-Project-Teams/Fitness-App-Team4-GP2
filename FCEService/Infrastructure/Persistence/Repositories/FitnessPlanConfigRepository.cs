using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using FCEService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Shared.Results.Pagination;
namespace FCEService.Infrastructure.Persistence.Repositories
{
    public sealed class FitnessPlanConfigRepository(FCEDbContext db)
        : GenericRepository<FitnessPlanConfig>(db), IFitnessPlanConfigRepository
    {
        public Task<FitnessPlanConfig?> FindMatchingPlanAsync(
            FitnessGoal goal,
            PlanStatus status,
            CancellationToken ct = default)
            => dbSet
                 .AsNoTracking()
                 .FirstOrDefaultAsync(p => p.Goal == goal && p.Status == status, ct);

        public async Task<PaginatedResult<FitnessPlanConfig>> GetPagedAsync(
               FitnessGoal? goalFilter,
               PlanStatus? statusFilter,
               PaginationRequest pagination,
               CancellationToken ct = default)
        {
            var query = dbSet.AsNoTracking().AsQueryable();

            if (goalFilter.HasValue && goalFilter != FitnessGoal.None)
                query = query.Where(p => p.Goal == goalFilter);

            if (statusFilter.HasValue && statusFilter != PlanStatus.None)
                query = query.Where(p => p.Status == statusFilter);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(p => p.Goal).ThenBy(p => p.Status)
                .Skip(pagination.Skip)
                .Take(pagination.PageSize)
                .ToListAsync(ct);

            return PaginatedResult<FitnessPlanConfig>.Create(
                items, totalCount, pagination.Page, pagination.PageSize);
        }
    }
}

