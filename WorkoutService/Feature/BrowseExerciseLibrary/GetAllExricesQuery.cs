using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Feature.Browse_Exercise_Library.Dtos__ViewModels;

namespace WorkoutService.Feature.Browse_Exercise_Library
{
    public record GetAllExricesQuery(PaginationRequest PaginationRequest) : IRequest<HandlerResponse<PaginatedResult<ExerciseDto>>>;
    

}
