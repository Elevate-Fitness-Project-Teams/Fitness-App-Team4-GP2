using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.SharedFeaturs.Queries.GetWorkOutById
{
    public class GetWorkOutByIdQueryHandler : IRequestHandler<GetWorkOutByIdQuery, HandlerResponse<Workout>>
    {
        private readonly IGenericRepository<Workout> workOutRepository;

        public GetWorkOutByIdQueryHandler(IGenericRepository<Workout> workOutRepository)
        {
            this.workOutRepository = workOutRepository;
        }
        public  async Task<HandlerResponse<Workout>> Handle(GetWorkOutByIdQuery request, CancellationToken cancellationToken)
        {
            var workout = await workOutRepository.GetTable()
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(e=>e.WorkoutId == request.Id, cancellationToken);
            if (workout is null)
            {
                return  HandlerResponseFactory.Failure<Workout>(HandlerErrorCodesEnum.NotFoundAnyWorkOuts);
            }
            return HandlerResponseFactory.Success(workout);
        }
    }
}
