using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Feature.Browse_Exercise_Library.Dtos__ViewModels;
using WorkoutService.Feature.BrowseWorkoutPlans.Dtos__ViewModels;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.BrowseWorkoutPlans
{
    public class GetWorkOutsPlansQueryHAndler : IRequestHandler<GetWorkOutsPlansQuery, HandlerResponse<PaginatedResult<WorkOutPlansDto>>>
    {
        private readonly IGenericRepository<WorkoutPlan> workOutRepository;

        public GetWorkOutsPlansQueryHAndler(IGenericRepository<WorkoutPlan> workOutRepository)
        {
            this.workOutRepository = workOutRepository;
        }
        public async Task<HandlerResponse<PaginatedResult<WorkOutPlansDto>>> Handle(GetWorkOutsPlansQuery request, CancellationToken cancellationToken)
        {
            var WorkOutPlans = await workOutRepository.GetAllAsync()
                  .Skip(request.PaginationRequest.Skip)
                  .Take(request.PaginationRequest.PageSize)
                  .Select(e => new WorkOutPlansDto
                  {
                     PlanId= e.PlanId,
                        Name= e.Name,
                        Goal= e.Goal
                  }).ToListAsync(cancellationToken);

            var count = WorkOutPlans.Count();

            if (WorkOutPlans == null || !WorkOutPlans.Any())
            {
                return HandlerResponseFactory.Failure<PaginatedResult<WorkOutPlansDto>>(HandlerErrorCodesEnum.NotFoundAnyExercise);
            }

            var PaginatedWorkOutPlans = PaginatedResult<WorkOutPlansDto>.Create
                (WorkOutPlans, count, request.PaginationRequest.Page, request.PaginationRequest.PageSize);

            return HandlerResponseFactory.Success(PaginatedWorkOutPlans);
        }
    }
    
}
