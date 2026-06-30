using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Feature.Browse_Exercise_Library.Dtos__ViewModels;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.Browse_Exercise_Library
{
    public class GetAllExricesQueryHandler : IRequestHandler<GetAllExricesQuery, HandlerResponse<PaginatedResult<ExerciseDto>>>
    {
        private readonly IGenericRepository<Exercise> exerciseRepository;

        public GetAllExricesQueryHandler(IGenericRepository<Exercise> exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }
        public async Task<HandlerResponse<PaginatedResult<ExerciseDto>>> Handle(GetAllExricesQuery request, CancellationToken cancellationToken)
        {

            var exercises = await exerciseRepository.GetAllAsync()
                .Skip(request.PaginationRequest.Skip)
                .Take(request.PaginationRequest.PageSize)
                .Select(e => new ExerciseDto
                {
                    ExerciseId = e.ExerciseId,
                    Name = e.Name,  
                    Difficulty = e.Difficulty,
                    Description = e.Description
                }).ToListAsync(cancellationToken);

            var count = exercises.Count();
            
            if (exercises == null || !exercises.Any())
            {
                return HandlerResponseFactory.Failure<PaginatedResult<ExerciseDto>>(HandlerErrorCodesEnum.NotFoundAnyExercise);
            }

            var PaginatedExercies = PaginatedResult<ExerciseDto>.Create
                (exercises, count, request.PaginationRequest.Page, request.PaginationRequest.PageSize);

            return  HandlerResponseFactory.Success(PaginatedExercies);
        }
    }
}
