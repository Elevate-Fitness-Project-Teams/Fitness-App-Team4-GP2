using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.GetAllWorkOuts.Dtos;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.GetAllWorkOuts
{
    public class GetAllWorkOutsBySearchQueryHandler : IRequestHandler<GetAllWorkOutsBySearchQuery, HandlerResponse<List<WorkOutDto>>>
    {
        private readonly IGenericRepository<Workout> workOutrepository;

        public GetAllWorkOutsBySearchQueryHandler( IGenericRepository<Workout> WorkOutrepository)
        {
            workOutrepository = WorkOutrepository;
        }
        public async Task<HandlerResponse<List<WorkOutDto>>> Handle(GetAllWorkOutsBySearchQuery request, CancellationToken cancellationToken)
        {
            int page = request.Page;
            if (page < 1 ) page = 1;

            var pageSize = request.PageSize;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;


            var workouts = workOutrepository.GetAllAsync().Select(
                e =>new Workout
            { 
               WorkoutId= e.WorkoutId,
               Name= e.Name,
               Category= e.Category,
               Difficulty= e.Difficulty,
               DurationInMinutes= e.DurationInMinutes,
               WorkoutPlan=new WorkoutPlan
                {
                   PlanId= e.WorkoutPlan.PlanId,
                   Name= e.WorkoutPlan.Name,
                   Difficulty= e.WorkoutPlan.Difficulty,
                   Description= e.WorkoutPlan.Description,
                   Goal= e.WorkoutPlan.Goal,
                   Status= e.WorkoutPlan.Status
                 }

             });
            if (workouts is null || !workouts.Any())
            {
                return HandlerResponseFactory.Failure<List<WorkOutDto>>(HandlerErrorCodesEnum.NotFoundAnyWorkOuts);
            }


            if (!string.IsNullOrEmpty(request.Search))
            {
                workouts=workouts.Where(c=>c.Name.Contains(request.Search));
            }
            if(request.Category.HasValue)
            {
                if (!Enum.IsDefined(typeof(WorkOutCategory), request.Category))
                    return HandlerResponseFactory.Failure<List<WorkOutDto>>(HandlerErrorCodesEnum.InvalidCategoryName);
                workouts =workouts.Where(c=>c.Category.Equals(request.Category.Value));
            }
            if(request.Difficulty.HasValue)
            {
                if (!Enum.IsDefined(typeof(WorkOutDiffeculty), request.Difficulty))
                    return HandlerResponseFactory.Failure<List<WorkOutDto>>(HandlerErrorCodesEnum.InvalidDiffecultyLevel);
                workouts = workouts.Where(c=>c.Difficulty.Equals(request.Difficulty.Value));
            }
            if (request.Duration.HasValue)
            {
                workouts=workouts.Where(c=>c.DurationInMinutes==request.Duration.Value);
            }
          
            var pagedWorkoutsList = await workouts
                 .OrderBy(e => e.DurationInMinutes)
                 .Skip((page - 1) * request.PageSize)
                 .Take(pageSize)
                 .Select(w => new WorkOutDto
                 {
                     WorkoutId = w.WorkoutId,
                     PlanId = w.PlanId,
                     Name = w.Name,
                     Category = w.Category,
                     DurationInMinutes = w.DurationInMinutes,
                     Difficulty = w.Difficulty
                 }).ToListAsync(cancellationToken);


            return HandlerResponseFactory.Success(pagedWorkoutsList);

        }
    }
}
