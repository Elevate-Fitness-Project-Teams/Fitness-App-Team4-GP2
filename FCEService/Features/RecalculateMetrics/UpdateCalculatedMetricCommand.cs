using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using FCEService.Domain.Interfaces;
using FCEService.Domain.Services;
using FCEService.Features.Shared.Dtos;
using MediatR;

namespace FCEService.Features.RecalculateMetrics
{
        public sealed record UpdateCalculatedMetricCommand(
            int Id, Guid UserId, double Weight, double Height, int Age,
            Gender Gender, ActivityLevel ActivityLevel, FitnessGoal Goal, DateTime CalculatedAt)
            : IRequest<Result<CalculatedMetricDto>>;

      
        public sealed class UpdateCalculatedMetricHandler(
            IFceUnitOfWork uow, IMetabolicCalculatorService calculator)
            : IRequestHandler<UpdateCalculatedMetricCommand, Result<CalculatedMetricDto>>
        {
            public async Task<Result<CalculatedMetricDto>> Handle(
                UpdateCalculatedMetricCommand command, CancellationToken ct)
            {
                var bmr = calculator.CalculateBmr(command.Weight, command.Height, command.Age, command.Gender);
                var tdee = calculator.CalculateTdee(bmr, command.ActivityLevel);
                var calorieTarget = calculator.CalculateCalorieTarget(tdee, command.Goal);
                var status = calculator.DetermineStatus(calorieTarget);

                if (!double.IsFinite(bmr) || !double.IsFinite(tdee) ||
                    !double.IsFinite(calorieTarget) || status == PlanStatus.None)
                    return Result<CalculatedMetricDto>.Fail(FceErrors.InvalidCalculation);

                var repository = uow.GetRepository<CalculatedMetric>(); 
                var lastUpdatedAt = DateTime.UtcNow;

                var stub = new CalculatedMetric
                {
                    Id = command.Id,
                    UserId = command.UserId,
                    Bmr = bmr,
                    Tdee = tdee,
                    CalorieTarget = calorieTarget,
                    Status = status,
                    LastUpdatedAt = lastUpdatedAt
                };

                repository.SaveInclude(stub,
                    nameof(CalculatedMetric.Bmr), nameof(CalculatedMetric.Tdee),
                    nameof(CalculatedMetric.CalorieTarget), nameof(CalculatedMetric.Status),
                    nameof(CalculatedMetric.LastUpdatedAt));

                await uow.SaveChangesAsync(ct); 
                return Result<CalculatedMetricDto>.OK(new CalculatedMetricDto(
                    command.Id, command.UserId, bmr, tdee, calorieTarget, status,
                    command.CalculatedAt, lastUpdatedAt));
            }
        }
}

