using BuildingBlocks.Shared.Controllers;
using BuildingBlocks.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutService.BuildingBlock.HandlersResponse;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.GetAllWorkOuts.Dtos__ViewModels;

namespace WorkoutService.Controllers
{
    
    public class WorkOutController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public WorkOutController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetWorkOuts")]
        public async Task<IActionResult> GetWorkOuts([FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string? search,
            [FromQuery] WorkOutCategory? category, 
            [FromQuery] WorkOutDiffeculty? difficulty, 
            [FromQuery] int? duration,
            CancellationToken cancellationToken)
        {
            var query = new Feature.GetAllWorkOuts.GetAllWorkOutsQuery(page, pageSize, search, category, difficulty, duration, cancellationToken);
            var result = await mediator.Send(query);
            if(!result.IsSuccess)
            {
                var _errorCode = result.ErrorCode;
               if(_errorCode == HandlerErrorCodesEnum.NotFoundAnyWorkOuts)
                    return NotFound(ResponseFactory.Failure("No WorkOuts Is Found ",404));

               return BadRequest();
            }
            var WorkoutsViewModel = result.Data.Select(w=>new WorkOutViewModel
            {
                WorkoutId = w.WorkoutId,
                Name = w.Name,
                PlanId = w.PlanId,
                Category = w.Category.ToString(),
                DurationInMinutes = w.DurationInMinutes,
                Difficulty = w.Difficulty.ToString()

            }).ToList();

            return StatusCode(200, ResponseFactory.Success(WorkoutsViewModel, "WorkOuts Fetched Successfully", 200));
        }

    }
}
