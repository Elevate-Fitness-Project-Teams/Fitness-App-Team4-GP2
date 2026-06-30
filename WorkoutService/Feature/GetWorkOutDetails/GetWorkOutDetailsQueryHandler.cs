using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Feature.GetWorkOutDetails.Dtos__ViewModels;
using WorkoutService.Feature.SharedFeaturs.Queries.GetWorkOutById;

namespace WorkoutService.Feature.GetWorkOutDetails
{
    public class GetWorkOutDetailsQueryHandler : IRequestHandler<GetWorkOutDetailsQuery, HandlerResponse<WorkoutDetailsDto>>
    {
        private readonly IMediator mediator;

        public GetWorkOutDetailsQueryHandler(IMediator mediator )
        {
            this.mediator = mediator;
        }
        public async Task<HandlerResponse< WorkoutDetailsDto>> Handle(GetWorkOutDetailsQuery request, CancellationToken cancellationToken)
        {
            var WorkoutQuery = await mediator.Send(new GetWorkOutByIdQuery(request.Id),cancellationToken);
            if (!WorkoutQuery.IsSuccess)
            {
                return HandlerResponseFactory.Failure<WorkoutDetailsDto>(WorkoutQuery.ErrorCode);
            }
            var WorkOut = WorkoutQuery.Data;
            return HandlerResponseFactory.Success(new WorkoutDetailsDto
            {
                WorkoutId = WorkOut.WorkoutId,
                PlanId=WorkOut.PlanId,
                WorkoutName =WorkOut.Name,
                WorkoutCategory=WorkOut.Category,
                WorkoutDifficulty=WorkOut.Difficulty,
                DurationInMinutes=WorkOut.DurationInMinutes,
                Exercises=WorkOut.WorkoutExercises
                .OrderBy(e => e.OrderIndex)
                .Select(e=>new WorkoutExerciseDto
                {
                   ExerciseId=e.ExerciseId,
                   ExerciseName=e.Exercise.Name,
                   TargetMuscles=e.Exercise.TargetMuscles,
                   ExerciseEquipment=e.Exercise.Equipment,
                   ExerciseDifficulty=e.Exercise.Difficulty,
                   ExerciseDescription=e.Exercise.Description,
                   ExerciseVideoUrl=e.Exercise.VideoUrl,
                   SetsDefault=e.SetsDefault,
                   RepsDefault=e.RepsDefault,
                   RestTimeInSeconds=e.RestTimeInSeconds,
                   OrderIndex=e.OrderIndex

                })
                .ToList()
            });

        }
    }
}
