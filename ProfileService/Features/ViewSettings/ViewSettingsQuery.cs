using BuildingBlocks.Shared.Results;
using MediatR;
using ProfileService.Features.ViewSettings.Dtos;

namespace ProfileService.Features.ViewSettings
{
    public record ViewSettingsQuery() : IRequest<Result<ViewSettingsResponse>>;
}
