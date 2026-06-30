using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.StartWorkOutSession.Dtos__ViewModels;

namespace WorkoutService.Feature.StartWorkOutSession
{
    public record StartWorkOutSessionCommand(
    int WorkoutId,
    string UserId,
    WorkOutDiffeculty Difficulty,
    int PlannedDuration) :IRequest<HandlerResponse<StartWorkOutSessionResultDto>>;
   
}

