using BuildingBlocks.Shared.Results;
using MediatR;
using ProfileService.Features.UpdateSettings.Dtos;

namespace ProfileService.Features.UpdateSettings
{
    public record UpdateSettingsCommand(UpdateSettingsRequest Request) : IRequest<Result<UpdateSettingsResponse>>;
}
