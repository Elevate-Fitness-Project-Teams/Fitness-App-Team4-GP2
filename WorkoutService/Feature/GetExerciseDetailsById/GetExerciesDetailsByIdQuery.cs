using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Feature.GetExerciseDetailsById.Dtos__ViewModels;

namespace WorkoutService.Feature.GetExerciseDetailsById
{
    public record GetExerciesDetailsByIdQuery(int ExerciseId):IRequest<HandlerResponse<ExerciseDetailsDto>>;
   
}
