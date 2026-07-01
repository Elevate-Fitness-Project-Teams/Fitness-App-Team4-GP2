using BuildingBlocks.Shared.Results;
using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using FCEService.Features.CalculateMetrics;
using MediatR;

namespace FCEService.Features.CalculateMetrics
{
    public sealed class UpsertCalculatedMetricHandler(IFceUnitOfWork uow)
            : IRequestHandler<UpsertCalculatedMetricCommand, Result<CalculatedMetric>>
    {
        public async Task<Result<CalculatedMetric>> Handle(
            UpsertCalculatedMetricCommand command,
            CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var existing = await uow.GetRepository<CalculatedMetric>().FirstOrDefaultAsync(
                m => m.UserId == command.UserId, ct);

            CalculatedMetric metric;

            if (existing is null)
            {
                metric = new CalculatedMetric
                {
                    UserId = command.UserId,
                    Bmr = command.Bmr,
                    Tdee = command.Tdee,
                    CalorieTarget = command.CalorieTarget,
                    Status = command.Status,
                    CalculatedAt = now,
                    LastUpdatedAt = null
                };
                await uow.GetRepository<CalculatedMetric>().AddAsync(metric, ct);
            }
            else
            {
                existing.Bmr = command.Bmr;
                existing.Tdee = command.Tdee;
                existing.CalorieTarget = command.CalorieTarget;
                existing.Status = command.Status;
                existing.LastUpdatedAt = now;
                uow.GetRepository<CalculatedMetric>().Update(existing);
                metric = existing;
            }

            await uow.SaveChangesAsync(ct);
            return Result<CalculatedMetric>.OK(metric);
        }
    }
}