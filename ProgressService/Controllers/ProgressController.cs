using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgressService.Features.WorkoutCompletion;
using ProgressService.Shared;
using System.Security.Claims;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProgressService.Controllers
{
    public class ProgressController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public ProgressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpPost("workouts")]
        public async Task<ActionResult<WorkOutCompletionResponse>> LogWorkoutCompletionAsync(WorkoutCompletionCommand command)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //if (string.IsNullOrWhiteSpace(userId))
            //{
            //    return Unauthorized();
            //}
            command.UserId = "user-1";

            var result = await _mediator.Send(command);

            return HandleResult(result);
        }

    }
}

