using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.GetWorkOutsByCategoryName.Dtos__ViewModels;

namespace WorkoutService.Feature.GetWorkOutsByCategoryName
{
    public record GetWorkOutsByCategoryNameQuery(WorkOutCategory CategoryName) : IRequest<HandlerResponse<List<WorkoutByCategoryDto>>>;

}
