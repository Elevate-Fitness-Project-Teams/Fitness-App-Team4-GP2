using BuildingBlocks.Shared.Controllers;
using FCEService.Features.CalculateMetrics;
using FCEService.Features.SaveFitnessStats;
using FCEService.Features.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FCEService.Controllers
{
    public class FitnessController : ApiControllerBase
    {
        private readonly ISender _sender;

        public FitnessController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("SubmitFitnessStats")]
        public async Task<IActionResult> SubmitFitnessStats( [FromBody] SubmitFitnessStatsCommand command, CancellationToken ct)
        {
           
            var result = await _sender.Send(command, ct);
            return FromResult(result, "Fitness stats submitted successfully.", 201);
        }
       
        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate(
            [FromBody] UserIdRequest dto,
            CancellationToken ct)
        {
            var result = await _sender.Send(new CalculateMetricsCommand(dto.UserId), ct);
            return FromResult(result, "Fitness metrics calculated successfully.", 200);
        }
    }

}
