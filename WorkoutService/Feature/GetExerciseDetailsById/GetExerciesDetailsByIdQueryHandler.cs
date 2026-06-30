using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Feature.GetExerciseDetailsById.Dtos__ViewModels;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.GetExerciseDetailsById
{
    public class GetExerciesDetailsByIdQueryHandler : IRequestHandler<GetExerciesDetailsByIdQuery, HandlerResponse<ExerciseDetailsDto>>
    {
        private readonly IGenericRepository<Exercise> exerciseRepository;

        public GetExerciesDetailsByIdQueryHandler(IGenericRepository<Exercise> exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }
        public async Task<HandlerResponse<ExerciseDetailsDto>> Handle(GetExerciesDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var exercise = await exerciseRepository.GetByIdAsync(request.ExerciseId);
            
            if (exercise == null)
            {
                return HandlerResponseFactory.Failure<ExerciseDetailsDto>(HandlerErrorCodesEnum.NotFoundExerciesWithSpecificID);
            }

            var exerciseDetailsDto = new ExerciseDetailsDto
            {
                ExerciseId = exercise.ExerciseId,
                Name = exercise.Name,
                TargetMuscles = exercise.TargetMuscles,
                Equipment = exercise.Equipment,
                Difficulty = exercise.Difficulty,
                Description = exercise.Description,
                VideoUrl = exercise.VideoUrl
            };

            return HandlerResponseFactory.Success(exerciseDetailsDto);
        }
    }
}
