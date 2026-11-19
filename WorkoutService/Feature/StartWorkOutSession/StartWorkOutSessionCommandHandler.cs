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

            var workout = workoutRequest.Data;

            // 1. Idempotency: check if an active session already exists for this exact workout
            var existingSessionSameWorkout = await workOutSessionRepository
                .GetAllAsync(x => x.UserId == request.UserId && x.WorkoutId == request.WorkoutId && x.Status == WorkOutSessionStatus.active)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingSessionSameWorkout is not null)
            {
                var exercises = workout.Exercises
                    .OrderBy(e => e.OrderIndex)
                    .Select(e => new WorkoutExerciseInSessionDto
                    {
                        ExerciseId = e.ExerciseId,
                        Name = e.ExerciseName,
                        Description = e.ExerciseDescription,
                        Difficulty = e.ExerciseDifficulty,
                        Equipment = e.ExerciseEquipment,
                        OrderIndex = e.OrderIndex,
                        RepsDefault = e.RepsDefault,
                        RestTimeInSeconds = e.RestTimeInSeconds,
                        SetsDefault = e.SetsDefault,
                        TargetMuscles = e.TargetMuscles,
                        VideoUrl = e.ExerciseVideoUrl
                    }).ToList();

                return HandlerResponseFactory.Success(new StartWorkOutSessionResultDto
                {
                    SessionId = existingSessionSameWorkout.SessionId,
                    Status = existingSessionSameWorkout.Status,
                    ExercisesInSession = exercises
                });
            }

            // 2. If an active session exists for a DIFFERENT workout, mark it abandoned
            var otherActiveSessions = await workOutSessionRepository
                .GetAllAsync(x => x.UserId == request.UserId && x.WorkoutId != request.WorkoutId && x.Status == WorkOutSessionStatus.active)
                .ToListAsync(cancellationToken);

            foreach (var oldSession in otherActiveSessions)
            {
                oldSession.Status = WorkOutSessionStatus.abandoned;
            }

            // 3. Create new session
            var session = new WorkoutSession
            {
                UserId = request.UserId,
                WorkoutId = request.WorkoutId,
                StartedAt = DateTime.UtcNow,
                Status = WorkOutSessionStatus.active
            };

            await workOutSessionRepository.AddAsync(session);
            await workOutSessionRepository.SaveChangesAsync();

            var exercisesInSession = workout.Exercises
                .OrderBy(e => e.OrderIndex)
                .Select(e => new WorkoutExerciseInSessionDto
                {
                    ExerciseId = e.ExerciseId,
                    Name = e.ExerciseName,
                    Description = e.ExerciseDescription,
                    Difficulty = e.ExerciseDifficulty,
                    Equipment = e.ExerciseEquipment,
                    OrderIndex = e.OrderIndex,
                    RepsDefault = e.RepsDefault,
                    RestTimeInSeconds = e.RestTimeInSeconds,
                    SetsDefault = e.SetsDefault,
                    TargetMuscles = e.TargetMuscles,
                    VideoUrl = e.ExerciseVideoUrl
                }).ToList();

            await publishEndpoint.Publish(new SessionStartedEvent
            {
                SessionId = session.SessionId,
                UserId = request.UserId,
            }, cancellationToken);

            return HandlerResponseFactory.Success(new StartWorkOutSessionResultDto
            {
                SessionId = session.SessionId,
                Status = session.Status,
                ExercisesInSession = exercisesInSession
            });
        }
    }
}
