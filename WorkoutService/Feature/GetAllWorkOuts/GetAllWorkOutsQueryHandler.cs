using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.GetAllWorkOuts.Dtos;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.GetAllWorkOuts
{
    public class GetAllWorkOutsQueryHandler : IRequestHandler<GetAllWorkOutsQuery, HandlerResponse<List<WorkOutDto>>>
    {
        private readonly IGenericRepository<Workout> _workOutrepository;

        public GetAllWorkOutsQueryHandler(IGenericRepository<Workout> WorkOutrepository)
        {
            this._workOutrepository = WorkOutrepository;
        }
        public async Task<HandlerResponse<List<WorkOutDto>>> Handle(GetAllWorkOutsQuery request, CancellationToken cancellationToken)
        {
            int page = request.Page;
            if (page < 1) page = 1;
            var workouts = _workOutrepository.GetAllAsync(include: e => e.Include(e => e.WorkoutPlan));
            
            if(workouts is null || !workouts.Any())
            {
                return HandlerResponseFactory.Failure<List<WorkOutDto>>(HandlerErrorCodesEnum.NotFoundAnyWorkOuts);
            }
            if(!string.IsNullOrEmpty(request.Search))
            {
                workouts=workouts.Where(c=>c.Name.Contains(request.Search));
            }
            if(request.Category.HasValue)
            {
                workouts=workouts.Where(c=>c.Category.Equals(request.Category.Value));
            }
            if(request.Difficulty.HasValue)
            {
                workouts=workouts.Where(c=>c.Difficulty.Equals(request.Difficulty.Value));
            }
            if (request.Duration.HasValue)
            {
                workouts=workouts.Where(c=>c.DurationInMinutes==request.Duration.Value);
            }
          
            var pagedWorkoutsList = await workouts
                 .OrderBy(e => e.DurationInMinutes)
                 .Skip((page - 1) * request.PageSize)
                 .Take(request.PageSize)
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
