using BuildingBlocks.Shared.Results;
using MediatR;

namespace SmartCoachService.Features.SendChatMessage
{
    public record SendChatMessageCommand(
        string UserId,
        string Message
        ) : IRequest<Result<string>>;
    

}
