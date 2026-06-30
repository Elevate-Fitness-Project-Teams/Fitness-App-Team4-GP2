using FCEService.Domain.Entities;
using FCEService.Domain.Enums;

namespace FCEService.Domain.Interfaces
{
    public interface IFitnessPlanConfigRepository
    {
        Task<FitnessPlanConfig?> FindMatchingPlanAsync(
            FitnessGoal goal,
            PlanStatus status,
            CancellationToken ct = default);
        Task<IReadOnlyList<FitnessPlanConfig>> GetPagedAsync(
            FitnessGoal? goalFilter,
            PlanStatus? statusFilter,
            int page,
            int pageSize,
            CancellationToken ct = default);
    }
}
