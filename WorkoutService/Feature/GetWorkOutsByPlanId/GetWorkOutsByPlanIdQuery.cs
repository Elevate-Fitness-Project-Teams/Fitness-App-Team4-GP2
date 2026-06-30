using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Feature.GetWorkOutsByPlanId.Dtos__ViewModels;

namespace WorkoutService.Feature.GetWorkOutsByPlanId
{
    public record GetWorkOutsByPlanIdQuery(string PlanId) : IRequest<HandlerResponse<List<WorkoutByPlanDto>>>;
   
}
