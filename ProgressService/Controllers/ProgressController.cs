using BuildingBlocks.Shared.Controllers;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgressService.Features.LogWeightEntry;
using ProgressService.Features.ViewProgressDashboard;
using ProgressService.Features.ViewProgressStats;
using ProgressService.Features.ViewUserAchievements;
using ProgressService.Features.ViewWeightHistory;
using ProgressService.Features.WorkoutCompletion;

namespace ProgressService.Controllers
{
    [Authorize]
    public class ProgressController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ProgressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("workouts")]
        public async Task<IActionResult> LogWorkoutCompletionAsync(WorkoutCompletionRequest request)
        {
            var userId = GetCurrentUserId();

            var command = new WorkoutCompletionCommand
            {
                UserId = userId,
                WorkoutId = request.WorkoutId,
                SessionId = request.SessionId,
                DurationInMinutes = request.DurationInMinutes,
                CaloriesBurned = request.CaloriesBurned,
                Rating = request.Rating,
                Notes = request.Notes,
                ExercisesCompleted = request.ExercisesCompleted
            };

            var result = await _mediator.Send(command);

            return FromResult(result, "Workout Is Logged Successfully", StatusCodes.Status201Created);
        }

        [HttpPost("weight")]
        public async Task<IActionResult> LogWeightAsync(LogWeightEntryRequest request)
        {
            var userId = GetCurrentUserId();
            var command = new LogWeightEntryCommand
            (
                request.Weight,
                request.Date,
                request.Notes,
                userId
            );

            var result = await _mediator.Send(command);

            return FromResult(result, "Weight Is Logged Successfully", StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> ViewProgressDashboardAsync([FromQuery] ViewProgressDashboardRequest request)
        {
            var userId = GetCurrentUserId();
            var query = new ViewProgressDashboardQuery(userId, request.Period, request.StartDate, request.EndDate);
            var result = await _mediator.Send(query);
            return FromResult(result, "Progress dashboard retrieved successfully", StatusCodes.Status200OK);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProgress([FromRoute] Guid userId, [FromQuery] ViewProgressDashboardRequest request)
        {
            if (!IsOwnerOrAdmin(userId))
            {
                return FromResult(
                    Result<ViewProgressDashboardResponse>.Fail(Error.Forbidden("AUTH_FORBIDDEN", "You do not have permission to view progress for another user.")),
                    "",
                    StatusCodes.Status403Forbidden);
            }

            var query = new ViewProgressDashboardQuery(userId, request.Period, request.StartDate, request.EndDate);
            var result = await _mediator.Send(query);
            return FromResult(result, $"Progress of UserID {userId} retrieved successfully", StatusCodes.Status200OK);
        }

        [HttpGet("weight-history/{userId}")]
        public async Task<IActionResult> GetWeightHistory([FromRoute] Guid userId)
        {
            if (!IsOwnerOrAdmin(userId))
            {
                return FromResult(
                    Result<WeightHistoryResponse>.Fail(Error.Forbidden("AUTH_FORBIDDEN", "You do not have permission to view weight history for another user.")),
                    "",
                    StatusCodes.Status403Forbidden);
            }

            var query = new ViewWeightHistoryQuery(userId);
            var result = await _mediator.Send(query);
            return FromResult(result, $"Weight history of UserID {userId} retrieved successfully", StatusCodes.Status200OK);
        }

        [HttpGet("achievements")]
        public async Task<IActionResult> GetAchievements([FromQuery] Guid? userId)
        {
            // Use authenticated caller's identity; if a specific userId is requested, verify ownership/admin
            var targetUserId = userId.HasValue && userId.Value != Guid.Empty ? userId.Value : GetCurrentUserId();
            if (!IsOwnerOrAdmin(targetUserId))
            {
                return FromResult(
                    Result<UserAchievementsResponse>.Fail(Error.Forbidden("AUTH_FORBIDDEN", "You do not have permission to view achievements for another user.")),
                    "",
                    StatusCodes.Status403Forbidden);
            }

            var query = new ViewUserAchievementsQuery(targetUserId);
            var result = await _mediator.Send(query);
            return FromResult(result, $"Achievements of UserID {targetUserId} retrieved successfully", StatusCodes.Status200OK);
        }

        [HttpGet("stats/{userId}")]
        public async Task<IActionResult> GetProgressStats([FromRoute] Guid userId)
        {
            if (!IsOwnerOrAdmin(userId))
            {
                return FromResult(
                    Result<ViewProgressStatsResponse>.Fail(Error.Forbidden("AUTH_FORBIDDEN", "You do not have permission to view progress stats for another user.")),
                    "",
                    StatusCodes.Status403Forbidden);
            }

            var query = new ViewProgressStatsQuery(userId);
            var result = await _mediator.Send(query);
            return FromResult(result, $"Progress stats of UserID {userId} retrieved successfully", StatusCodes.Status200OK);
        }
    }
}

