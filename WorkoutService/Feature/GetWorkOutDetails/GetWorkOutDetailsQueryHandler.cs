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
                WorkoutName =WorkOut.WorkoutName,
                WorkoutCategory=WorkOut.WorkoutCategory,
                WorkoutDifficulty=WorkOut.WorkoutDifficulty,
                DurationInMinutes=WorkOut.DurationInMinutes,
                Exercises=WorkOut.Exercises
                .OrderBy(e => e.OrderIndex)
                .Select(e=>new WorkoutExerciseDto
                {
                   ExerciseId=e.ExerciseId,
                   ExerciseName=e.ExerciseName,
                   TargetMuscles=e.TargetMuscles,
                   ExerciseEquipment=e.ExerciseEquipment,
                   ExerciseDifficulty=e.ExerciseDifficulty,
                   ExerciseDescription=e.ExerciseDescription,
                   ExerciseVideoUrl=e.ExerciseVideoUrl,
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
