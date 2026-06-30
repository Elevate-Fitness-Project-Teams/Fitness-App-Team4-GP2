using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Feature.GetWorkOutDetails.Dtos__ViewModels;

namespace WorkoutService.Feature.GetWorkOutDetails
{
    public record GetWorkOutDetailsQuery(int Id):IRequest<HandlerResponse<WorkoutDetailsDto>>
    {
    }
}
