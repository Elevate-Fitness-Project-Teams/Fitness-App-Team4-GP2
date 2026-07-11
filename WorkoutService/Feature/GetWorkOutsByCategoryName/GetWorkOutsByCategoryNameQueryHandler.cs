using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.GetWorkOutsByCategoryName.Dtos__ViewModels;
using WorkoutService.Feature.GetWorkOutsByPlanId.Dtos__ViewModels;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.GetWorkOutsByCategoryName
{
    public class GetWorkOutsByCategoryNameQueryHandler : IRequestHandler<GetWorkOutsByCategoryNameQuery, HandlerResponse<List<WorkoutByCategoryDto>>>
    {
        private readonly IGenericRepository<Workout> workOutRepository;

        public GetWorkOutsByCategoryNameQueryHandler(IGenericRepository<Workout> workOutRepository )
        {
            this.workOutRepository = workOutRepository;
        }
        public async Task<HandlerResponse<List<WorkoutByCategoryDto>>> Handle(GetWorkOutsByCategoryNameQuery request, CancellationToken cancellationToken)
        {
            if(!Enum.IsDefined(typeof(WorkOutCategory), request.CategoryName))
            {
                return HandlerResponseFactory.Failure<List<WorkoutByCategoryDto>>(HandlerErrorCodesEnum.InvalidCategoryName);
            }

            var workouts = await workOutRepository.GetTable()
                .Where(w => w.Category == request.CategoryName)
                .Select(w => new WorkoutByCategoryDto
                {
                    WorkoutId = w.WorkoutId,
                    PlanId = w.PlanId,
                    WorkoutName = w.Name,
                    WorkoutCategory = w.Category,
                    WorkoutDifficulty = w.Difficulty,
                    DurationInMinutes = w.DurationInMinutes,
                    PlanName = w.WorkoutPlan.Name,
                    PlanDescription = w.WorkoutPlan.Description,
                    PlanGoal = w.WorkoutPlan.Goal,
                    PlanStatus = w.WorkoutPlan.Status
                })
                .ToListAsync(cancellationToken);

            if (!workouts.Any())
            {
              return  HandlerResponseFactory.Failure<List<WorkoutByCategoryDto>>(HandlerErrorCodesEnum.NotFoundAnyWorkOutsForThisCategoryName);
            }

            return HandlerResponseFactory.Success(workouts);
        }
    }
}
