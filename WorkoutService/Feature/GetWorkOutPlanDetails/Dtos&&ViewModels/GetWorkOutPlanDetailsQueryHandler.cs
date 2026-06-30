using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Feature.GetExerciseDetailsById.Dtos__ViewModels;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.GetWorkOutPlanDetails.Dtos__ViewModels
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
            var exercise = await workOutPlanRepository.GetAllAsync().FirstOrDefaultAsync(e=>e.PlanId== request.PlanId);

            if (exercise == null)
            {
                return HandlerResponseFactory.Failure<WorkOutPlanDto>(HandlerErrorCodesEnum.NotFoundPlanWithSpecificID);
            }

            var WorkOutPlanDetailsDto = new WorkOutPlanDto
            {
                PlanId = exercise.PlanId,
                Name = exercise.Name,
                Description = exercise.Description,
                Goal = exercise.Goal,
                Status = exercise.Status,
                Difficulty = exercise.Difficulty

            };

            return HandlerResponseFactory.Success(WorkOutPlanDetailsDto);
        }
    }
}
