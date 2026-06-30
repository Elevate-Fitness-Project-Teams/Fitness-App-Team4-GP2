using BuildingBlocks.Shared.Results;
using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using FCEService.Domain.Interfaces;
using MediatR;

namespace FCEService.Features.SaveFitnessStats
{
 
       public sealed record SaveFitnessStatsCommand(
       Guid UserId,
       double Weight,
       double Height,
       int Age,
       Gender Gender,
       FitnessGoal Goal,
       ActivityLevel ActivityLevel) : IRequest<Result<SubmitFitnessStatsResponse>>;

  
}
