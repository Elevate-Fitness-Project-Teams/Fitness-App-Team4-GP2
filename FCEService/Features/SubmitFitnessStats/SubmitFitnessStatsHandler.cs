using BuildingBlocks.Shared.Results;
using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using Mapster;
using MediatR;

namespace FCEService.Features.SaveFitnessStats
{
   
        public sealed class SaveFitnessStatsHandler(IFceUnitOfWork uow, IGenericRepository<UserFitnessStat> FitnessStatRepo)
        : IRequestHandler<SaveFitnessStatsCommand, Result<SubmitFitnessStatsResponse>>
        {
            public async Task<Result<SubmitFitnessStatsResponse>> Handle(
                SaveFitnessStatsCommand command,
                CancellationToken ct)
            {

                var stat = new UserFitnessStat
                {
                    UserId = command.UserId,
                    Weight = command.Weight,
                    Height = command.Height,
                    Age = command.Age,
                    Gender =command.Gender,
                    Goal = command.Goal,
                    ActivityLevel = command.ActivityLevel,
                    RecordedAt = DateTime.UtcNow
                };

                await FitnessStatRepo.AddAsync(stat, ct);
                await uow.SaveChangesAsync(ct);

                return Result<SubmitFitnessStatsResponse>.OK(stat.Adapt<SubmitFitnessStatsResponse>());
            }
        }
 }

