 using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using FCEService.Domain.Interfaces;
using FCEService.Domain.Services;
using FCEService.Features.CalculateMetrics.Queries;
using MediatR;

namespace FCEService.Features.CalculateMetrics.Commands
    {
        public sealed record CreateCalculatedMetricCommand(
            Guid UserId,double Weight, double Height,int Age,
            Gender Gender,ActivityLevel ActivityLevel,FitnessGoal Goal)
              : IRequest<Result<CalculatedMetricDto>>;

        public sealed class CreateCalculatedMetricCommandHandler(
            IFceUnitOfWork uow,
            IMetabolicCalculatorService calculator) 
            : IRequestHandler<CreateCalculatedMetricCommand, Result<CalculatedMetricDto>>
        {
            public async Task<Result<CalculatedMetricDto>> Handle(
                CreateCalculatedMetricCommand command, CancellationToken ct)
            {
                var bmr = calculator.CalculateBmr(command.Weight, command.Height, command.Age, command.Gender);
                var tdee = calculator.CalculateTdee(bmr, command.ActivityLevel);
                var calorieTarget = calculator.CalculateCalorieTarget(tdee, command.Goal);
                var status = calculator.DetermineStatus(calorieTarget);

                if (!double.IsFinite(bmr) || !double.IsFinite(tdee) ||
                    !double.IsFinite(calorieTarget) || status == PlanStatus.None)
                    return Result<CalculatedMetricDto>.Fail(FceErrors.InvalidCalculation);

                var repository = uow.GetRepository<CalculatedMetric>(); 

                var metric = new CalculatedMetric
                {
                    UserId = command.UserId,
                    Bmr = bmr,
                    Tdee = tdee,
                    CalorieTarget = calorieTarget,
                    Status = status,
                    CalculatedAt = DateTime.UtcNow,
                    LastUpdatedAt = null
                };

                await repository.AddAsync(metric, ct);
                await uow.SaveChangesAsync(ct); 

                return Result<CalculatedMetricDto>.OK(new CalculatedMetricDto(
                    metric.UserId, metric.Bmr, metric.Tdee, metric.CalorieTarget,
                    metric.Status, metric.CalculatedAt, metric.LastUpdatedAt));
            }
        }
    }

