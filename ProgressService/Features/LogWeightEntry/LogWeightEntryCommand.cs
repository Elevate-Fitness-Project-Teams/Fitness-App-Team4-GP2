using BuildingBlocks.Shared.Results;
using MediatR;

namespace ProgressService.Features.LogWeightEntry
{
    public record LogWeightEntryCommand(double Weight , DateTime Date , string? Notes , Guid UserId) : IRequest<Result<LogWeightEntryResponse>>;
    
}
