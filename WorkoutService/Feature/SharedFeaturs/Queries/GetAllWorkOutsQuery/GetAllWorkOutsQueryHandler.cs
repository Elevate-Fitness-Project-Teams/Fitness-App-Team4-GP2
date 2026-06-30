using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Feature.GetAllWorkOuts.Dtos;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.SharedFeaturs.Queries.GetAllWorkOutsQuery
{
    public class GetAllWorkOutsQueryHandler : IRequestHandler<GetAllWorkOutsQuery, HandlerResponse<IQueryable<Workout>>>
    {
        private readonly IGenericRepository<Workout> _workOutrepository;

        public GetAllWorkOutsQueryHandler(IGenericRepository<Workout> WorkOutrepository)
        {
            this._workOutrepository = WorkOutrepository;
        }
        public async  Task<HandlerResponse<IQueryable<Workout>>> Handle(GetAllWorkOutsQuery request, CancellationToken cancellationToken)
        {
            var workouts =   _workOutrepository.GetAllAsync(include: e => e.Include(e => e.WorkoutPlan));
            if (workouts is null || !workouts.Any())
            {
                return  HandlerResponseFactory.Failure<IQueryable<Workout>>(HandlerErrorCodesEnum.NotFoundAnyWorkOuts);
            }

            return HandlerResponseFactory.Success(workouts);
        }
    }
}
