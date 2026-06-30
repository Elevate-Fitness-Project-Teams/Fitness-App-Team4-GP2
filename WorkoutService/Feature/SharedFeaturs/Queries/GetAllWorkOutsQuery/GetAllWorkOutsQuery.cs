using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Feature.SharedFeaturs.Queries.GetAllWorkOutsQuery
{
    public record GetAllWorkOutsQuery(CancellationToken CancellationToken): IRequest<HandlerResponse<IQueryable<Workout>>>;

   
}
