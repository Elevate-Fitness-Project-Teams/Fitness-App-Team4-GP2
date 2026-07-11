using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Interfaces;
using Mapster;
using MediatR;

namespace FCEService.Features.GetSpecificPlanConfiguration
{
   
    public sealed record GetPlanConfigDetailQuery(string PlanId) : IRequest<Result<PlanConfigDetailResponse>>;

        public sealed class GetPlanConfigDetailHandler(IFceUnitOfWork uow)
            : IRequestHandler<GetPlanConfigDetailQuery, Result<PlanConfigDetailResponse>>
        {
            public async Task<Result<PlanConfigDetailResponse>> Handle(
                GetPlanConfigDetailQuery query, CancellationToken ct)
            {
                var config = await uow.FitnessPlanConfigs.GetByPlanIdAsync(query.PlanId, ct);

                return config is null
                    ? Result<PlanConfigDetailResponse>.Fail(FceErrors.PlanNotFound(query.PlanId))
                    : Result<PlanConfigDetailResponse>.OK(config.Adapt<PlanConfigDetailResponse>());
            }
        }
}

