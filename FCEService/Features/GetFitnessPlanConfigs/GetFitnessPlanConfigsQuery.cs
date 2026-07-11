using BuildingBlocks.Shared.Results;
using BuildingBlocks.Shared.Results.Pagination;
using FCEService.Domain.Enums;
using FCEService.Domain.Interfaces;
using FCEService.Features.GetFitnessPlanConfigs.FCEService.Features.GetFitnessPlanConfigs;
using Mapster;
using MediatR;

namespace FCEService.Features.GetFitnessPlanConfigs
{

        public sealed record GetFitnessPlanConfigsQuery(
            FitnessGoal? Goal, PlanStatus? Status, int Page = 1, int PageSize = 20)
            : IRequest<Result<PaginatedResult<FitnessPlanConfigResponse>>>;

       
        public sealed class GetFitnessPlanConfigsHandler(IFceUnitOfWork uow)
            : IRequestHandler<GetFitnessPlanConfigsQuery, Result<PaginatedResult<FitnessPlanConfigResponse>>>
        {
            public async Task<Result<PaginatedResult<FitnessPlanConfigResponse>>> Handle(
                GetFitnessPlanConfigsQuery query, CancellationToken ct)
            {
                var pagination = new PaginationRequest(query.Page, query.PageSize);

                var paged = await uow.FitnessPlanConfigs.GetPagedAsync(query.Goal, query.Status, pagination, ct);

                var items = paged.Items.Select(p => p.Adapt<FitnessPlanConfigResponse>()).ToList();

                var response = PaginatedResult<FitnessPlanConfigResponse>.Create(
                    items, paged.TotalCount, paged.Page, paged.PageSize);

                return Result<PaginatedResult<FitnessPlanConfigResponse>>.OK(response);
            }
        }
}

