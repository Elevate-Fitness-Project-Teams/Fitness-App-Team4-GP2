using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var exercise = await exerciseRepository.GetTable()
                .Where(e=>e.ExerciseId== request.ExerciseId)
                .Select(e=>new ExerciseDetailsDto
                {
                    ExerciseId = e.ExerciseId,
                    Name = e.Name,
                    TargetMuscles = e.TargetMuscles,
                    Equipment = e.Equipment,
                    Difficulty = e.Difficulty,
                    Description = e.Description,
                    VideoUrl = e.VideoUrl
                })
                .FirstOrDefaultAsync(cancellationToken);
            
            if (exercise == null)
            {
                return HandlerResponseFactory.Failure<ExerciseDetailsDto>(HandlerErrorCodesEnum.NotFoundExerciesWithSpecificID);
            }


            return HandlerResponseFactory.Success(exercise);
        }
    }
}
