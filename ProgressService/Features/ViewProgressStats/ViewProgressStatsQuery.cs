using BuildingBlocks.Shared.Results;
using MediatR;

namespace ProgressService.Features.ViewProgressStats
{
    public record ViewProgressStatsQuery(Guid UserId):IRequest<Result<ViewProgressStatsResponse>>;
    
}
