using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Feature.GetWorkOutsByPlanId.Dtos__ViewModels;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.GetWorkOutsByPlanId
{
    public class GetWorkOutsByPlanIdQueryHandler : IRequestHandler<GetWorkOutsByPlanIdQuery, HandlerResponse<List<WorkoutByPlanDto>>>
    {
        private readonly IGenericRepository<Workout> workOutRepository;

        public GetWorkOutsByPlanIdQueryHandler(IGenericRepository<Workout> workOutRepository)
        {
            this.workOutRepository = workOutRepository;
        }
        public async Task<HandlerResponse<List<WorkoutByPlanDto>>> Handle(GetWorkOutsByPlanIdQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.PlanId))
            {
                return HandlerResponseFactory.Failure<List<WorkoutByPlanDto>>(HandlerErrorCodesEnum.PlanIdISNull);
            }

            var workouts = await workOutRepository.GetAllAsync(expression: w => w.PlanId == request.PlanId, include: e => e.Include(e => e.WorkoutPlan))
                .Select(w => new WorkoutByPlanDto
                {
                    WorkoutId = w.WorkoutId,
                    PlanId = w.PlanId,
                    WorkoutName = w.Name,
                    WorkoutCategory = w.Category,
                    WorkoutDifficulty = w.Difficulty,
                    DurationInMinutes = w.DurationInMinutes,
                    PlanName=w.WorkoutPlan.Name,
                    PlanDescription=w.WorkoutPlan.Description,
                    PlanGoal=w.WorkoutPlan.Goal,
                    PlanStatus=w.WorkoutPlan.Status
                })
                .ToListAsync(cancellationToken);

            if (!workouts.Any())
            {
                HandlerResponseFactory.Failure<List<WorkoutByPlanDto>>(HandlerErrorCodesEnum.NotFoundAnyWorkOutsForThisPlan);
            }

            return HandlerResponseFactory.Success(workouts);
        }
    }
}
