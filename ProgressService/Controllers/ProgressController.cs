using BuildingBlocks.Shared.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProgressService.Features.WorkoutCompletion;
using System.Security.Claims;


namespace ProgressService.Controllers
{
    public class ProgressController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ProgressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpPost("workouts")]
        public async Task<IActionResult> LogWorkoutCompletionAsync(WorkoutCompletionRequest request)
        {
            //var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //if (!Guid.TryParse(userIdClaim, out var userId))
            //{
            //    return Unauthorized();
            //}
            var userId = new Guid("00000000-0000-0000-0000-000000000001"); 

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

            return FromResult(result , "Workout Is Logged Successfully" , StatusCodes.Status201Created);
        }

    }
}

