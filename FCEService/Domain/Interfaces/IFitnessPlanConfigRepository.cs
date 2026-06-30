using BuildingBlocks.Shared.Results.Pagination;
using FCEService.Domain.Entities;
using FCEService.Domain.Enums;

namespace FCEService.Domain.Interfaces
{
    public interface IFitnessPlanConfigRepository : IGenericRepository<FitnessPlanConfig>
    {
        Task<FitnessPlanConfig?> FindMatchingPlanAsync(
            FitnessGoal goal,
            PlanStatus status,
            CancellationToken ct = default);
      

        Task<PaginatedResult<FitnessPlanConfig>> GetPagedAsync(
        FitnessGoal? goalFilter,
        PlanStatus? statusFilter,
        PaginationRequest pagination,
        CancellationToken ct = default);
    }
}
