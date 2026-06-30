using BuildingBlocks.Shared.Results;
using MediatR;

namespace ProgressService.Features.ViewWeightHistory
{
    public record ViewWeightHistoryQuery(Guid UserId) : IRequest<Result<WeightHistoryResponse>>;
    
}
