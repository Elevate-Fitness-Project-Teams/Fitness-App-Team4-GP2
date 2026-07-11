using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Feature.SharedFeaturs.Queries.GetWorkOutById
{
    public record GetWorkOutByIdQuery(int Id) : IRequest<HandlerResponse<WorkoutQueryDto>>;

}
