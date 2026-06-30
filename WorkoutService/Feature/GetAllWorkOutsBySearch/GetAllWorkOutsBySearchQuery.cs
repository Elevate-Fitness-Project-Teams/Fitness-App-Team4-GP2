using MediatR;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.GetAllWorkOuts.Dtos;

namespace WorkoutService.Feature.GetAllWorkOuts
{
    public record GetAllWorkOutsBySearchQuery(int Page,
        int PageSize,string? Search, 
        WorkOutCategory? Category, 
        WorkOutDiffeculty? Difficulty,
        int? Duration
        ) :IRequest<HandlerResponse<List<WorkOutDto>>>;

}