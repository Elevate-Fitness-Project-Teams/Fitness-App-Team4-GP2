using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.GetAllWorkOuts.Dtos;

namespace WorkoutService.Feature.GetAllWorkOuts
{
    public record GetAllWorkOutsQuery(int Page,
        int PageSize,string? Search, 
        WorkOutCategory? Category, 
        WorkOutDiffeculty? Difficulty,
        int? Duration, 
        CancellationToken CancellationToken) :IRequest<HandlerResponse<List<WorkOutDto>>>;

}