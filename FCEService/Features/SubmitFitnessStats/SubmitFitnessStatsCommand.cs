using BuildingBlocks.Contracts.Fce;
using BuildingBlocks.Shared.Results;
using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using FCEService.Domain.Interfaces;
using Mapster;
using MassTransit;
using MediatR;

namespace FCEService.Features.SaveFitnessStats
{
   public sealed record SubmitFitnessStatsCommand(
   Guid UserId,
   double Weight,
   double Height,
   int Age,
   Gender Gender,
   FitnessGoal Goal,
   ActivityLevel ActivityLevel) : IRequest<Result<SubmitFitnessStatsResponse>>;


    public sealed class SubmitFitnessStatsCommandHandler(IFceUnitOfWork uow, IPublishEndpoint publishEndpoint)
        : IRequestHandler<SubmitFitnessStatsCommand, Result<SubmitFitnessStatsResponse>>
        {
            public async Task<Result<SubmitFitnessStatsResponse>> Handle(
                SubmitFitnessStatsCommand command,
                CancellationToken ct)
            {

            var FitnessStatRepo = uow.GetRepository<UserFitnessStat>();


            var stat = new UserFitnessStat
            {
                UserId = command.UserId,
                Weight = command.Weight,
                Height = command.Height,
                Age = command.Age,
                Gender = command.Gender,
                Goal = command.Goal,
                ActivityLevel = command.ActivityLevel,
                RecordedAt = DateTime.UtcNow
            };

                await FitnessStatRepo.AddAsync(stat, ct);
                await uow.SaveChangesAsync(ct);

                // Refresh the fitness portion of ProfileService's cached snapshot.
                await publishEndpoint.Publish(new UserFitnessUpdatedEvent
                {
                    UserId = stat.UserId,
                    Weight = stat.Weight,
                    Height = stat.Height,
                    UpdatedAt = stat.RecordedAt
                }, ct);

                return Result<SubmitFitnessStatsResponse>.OK(stat.Adapt<SubmitFitnessStatsResponse>());
            }
        }
 }

