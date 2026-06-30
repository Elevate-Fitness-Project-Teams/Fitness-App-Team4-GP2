using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Feature.GetWorkOutPlanDetails.Dtos__ViewModels;
using WorkoutService.Feature.GetWorkOutsByPlanId.Dtos__ViewModels;

namespace WorkoutService.Feature.GetWorkOutPlanDetails
{
    public record GetWorkOutPlanDetailsQuery(string PlanId):IRequest<HandlerResponse<WorkOutPlanDto>>;

}
