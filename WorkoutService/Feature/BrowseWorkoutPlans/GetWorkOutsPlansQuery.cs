using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Feature.BrowseWorkoutPlans.Dtos__ViewModels;

namespace WorkoutService.Feature.BrowseWorkoutPlans
{
    public record GetWorkOutsPlansQuery(PaginationRequest PaginationRequest) :IRequest<HandlerResponse<PaginatedResult<WorkOutPlansDto>>>;
    
}
