using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Feature.GetExerciseDetailsById.Dtos__ViewModels;
using WorkoutService.Feature.GetWorkOutPlanDetails.Dtos__ViewModels;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.GetWorkOutPlanDetails
{
    public class GetWorkOutPlanDetailsQueryHandler : IRequestHandler<GetWorkOutPlanDetailsQuery, HandlerResponse<WorkOutPlanDto>>
    {
        private readonly IGenericRepository<WorkoutPlan> workOutPlanRepository;

        public GetWorkOutPlanDetailsQueryHandler(IGenericRepository<WorkoutPlan> workOutPlanRepository)
        {
            this.workOutPlanRepository = workOutPlanRepository;
        }
        public async Task<HandlerResponse<WorkOutPlanDto>> Handle(GetWorkOutPlanDetailsQuery request, CancellationToken cancellationToken)
        {
            var exercise = await workOutPlanRepository.GetTable().Where(e => e.PlanId == request.PlanId)
                .Select(e => new WorkOutPlanDto
                {
                    PlanId = e.PlanId,
                    Name = e.Name,
                    Description = e.Description,
                    Goal = e.Goal,
                    Status = e.Status,
                    Difficulty = e.Difficulty
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (exercise == null)
            {
                return HandlerResponseFactory.Failure<WorkOutPlanDto>(HandlerErrorCodesEnum.NotFoundPlanWithSpecificID);
            }


            return HandlerResponseFactory.Success(exercise);
        }
    }
}
