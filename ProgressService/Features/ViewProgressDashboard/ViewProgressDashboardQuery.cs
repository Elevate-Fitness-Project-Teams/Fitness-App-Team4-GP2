using BuildingBlocks.Shared.Results;
using MediatR;

namespace ProgressService.Features.ViewProgressDashboard
{
    public record ViewProgressDashboardQuery(Guid UserId,Period? Period , DateTime? StartDate , DateTime? EndDate) : IRequest<Result<ViewProgressDashboardResponse>>;
    
    
}
