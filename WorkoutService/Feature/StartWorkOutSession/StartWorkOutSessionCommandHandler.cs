using BuildingBlocks.Contracts.Progress;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.SharedFeaturs.Queries.GetWorkOutById;
using WorkoutService.Feature.StartWorkOutSession.Dtos__ViewModels;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.Feature.StartWorkOutSession
{
    public class StartWorkOutSessionCommandHandler : IRequestHandler<StartWorkOutSessionCommand, HandlerResponse<StartWorkOutSessionResultDto>>
    {
        private readonly IGenericRepository<WorkoutSession> workOutSessionRepository;
        private readonly IGenericRepository<Workout> workOutRepository;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMediator mediator;

        public StartWorkOutSessionCommandHandler(
            IGenericRepository<WorkoutSession> workOutSessionRepository,
            IGenericRepository<Workout> workOutRepository,
            IMediator mediator,
            IPublishEndpoint publishEndpoint

            )
        {
            this.workOutSessionRepository = workOutSessionRepository;
            this.workOutRepository = workOutRepository;
            this.publishEndpoint = publishEndpoint;
            this.mediator = mediator;
        }
        public async Task<HandlerResponse<StartWorkOutSessionResultDto>> Handle(StartWorkOutSessionCommand request, CancellationToken cancellationToken)
        {
            var workoutRequest = await mediator.Send(new GetWorkOutByIdQuery(request.WorkoutId));

            if (!workoutRequest.IsSuccess)
            {
                return HandlerResponseFactory.Failure<StartWorkOutSessionResultDto>(workoutRequest.ErrorCode);            
            }
            var SessionIsExists = await workOutSessionRepository
                .GetAllAsync(x => x.UserId.Equals(request.UserId) && x.Status == Domain.Enums.WorkOutSessionStatus.active).FirstOrDefaultAsync(cancellationToken);

            if (SessionIsExists is not null)
            {
                return HandlerResponseFactory.Success(new StartWorkOutSessionResultDto { SessionId = SessionIsExists.SessionId });

            }
            var workout = workoutRequest.Data;
            var Session = new WorkoutSession
            {
                UserId = request.UserId,

                WorkoutId = request.WorkoutId,

                StartedAt = DateTime.UtcNow,

                Status = WorkOutSessionStatus.active
            };

            await workOutSessionRepository.AddAsync(Session);
            await workOutSessionRepository.SaveChangesAsync();

            var exercisesInSession = workout.WorkoutExercises
                .Select(e => new WorkoutExerciseInSessionDto
                {
                    ExerciseId=e.ExerciseId,
                    Name=e.Exercise.Name,
                    Description=e.Exercise.Description,
                    Difficulty=e.Exercise.Difficulty,
                    Equipment=e.Exercise.Equipment,
                    OrderIndex=e.OrderIndex,
                    RepsDefault=e.RepsDefault,
                    RestTimeInSeconds=e.RestTimeInSeconds,
                    SetsDefault=e.SetsDefault,
                    TargetMuscles=e.Exercise.TargetMuscles,
                    VideoUrl=e.Exercise.VideoUrl

                }).ToList();

            await publishEndpoint.Publish( new SessionStartedEvent
                    {
                        SessionId = Session.SessionId,
                        UserId = request.UserId.ToString(),
                    });



            return HandlerResponseFactory.Success(new StartWorkOutSessionResultDto
            {
                SessionId = Session.SessionId,
                Status = Session.Status,
                ExercisesInSession = exercisesInSession

            });
        }
    }
}
