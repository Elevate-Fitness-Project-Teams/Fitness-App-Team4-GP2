using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Feature.GetWorkOutDetails.Dtos__ViewModels;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.SharedFeaturs.Queries.GetWorkOutById
{
    public class GetWorkOutByIdQueryHandler : IRequestHandler<GetWorkOutByIdQuery, HandlerResponse<WorkoutQueryDto>>
    {
        private readonly IGenericRepository<Workout> workOutRepository;

        public GetWorkOutByIdQueryHandler(IGenericRepository<Workout> workOutRepository)
        {
            this.workOutRepository = workOutRepository;
        }
        public  async Task<HandlerResponse<WorkoutQueryDto>> Handle(GetWorkOutByIdQuery request, CancellationToken cancellationToken)
        {
            var workout = await workOutRepository.GetTable()
                .Where(e => e.WorkoutId == request.Id)
                .Select(e => new WorkoutQueryDto
                {
                    WorkoutId = e.WorkoutId,
                    PlanId = e.PlanId,
                    WorkoutName = e.Name,
                    WorkoutCategory = e.Category,
                    WorkoutDifficulty = e.Difficulty,
                    DurationInMinutes = e.DurationInMinutes,
                    Exercises = e.WorkoutExercises
                        .OrderBy(e => e.OrderIndex)
                        .Select(e => new WorkoutQueryExerciseDto
                        {
                            ExerciseId = e.ExerciseId,
                            ExerciseName = e.Exercise.Name,
                            TargetMuscles = e.Exercise.TargetMuscles,
                            ExerciseEquipment = e.Exercise.Equipment,
                            ExerciseDifficulty = e.Exercise.Difficulty,
                            ExerciseDescription = e.Exercise.Description,
                            ExerciseVideoUrl = e.Exercise.VideoUrl,
                            SetsDefault = e.SetsDefault,
                            RepsDefault = e.RepsDefault,
                            RestTimeInSeconds = e.RestTimeInSeconds,
                            OrderIndex = e.OrderIndex
                        }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);



            //Include(w => w.WorkoutExercises)
            //.ThenInclude(we => we.Exercise)
            //.FirstOrDefaultAsync(e=>e.WorkoutId == request.Id, cancellationToken);
            if (workout is null)
            {
                return  HandlerResponseFactory.Failure<WorkoutQueryDto>(HandlerErrorCodesEnum.NotFoundAnyWorkOuts);
            }
            return HandlerResponseFactory.Success(workout);
        }
    }
}
