using BuildingBlocks.Shared.Controllers;
using FCEService.Features.CalculateMetrics;
using FCEService.Features.GetFitnessMetrics;
using FCEService.Features.GetFitnessStats;
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
            [FromBody] CalculateMetricsCommand command,
            CancellationToken ct)
        {
            var result = await _sender.Send(command, ct);
            return FromResult(result, "Fitness metrics calculated successfully.", 200);
        }

        [HttpGet("metrics/{userId:guid}")]
        public async Task<IActionResult> GetFitnessMetrics(Guid userId,CancellationToken ct)
        {
            var result = await _sender.Send(new GetFitnessMetricsQuery(userId),ct);
            return FromResult(result, "Fitness metrics retrieved successfully", 200);
        }

        
        [HttpGet("stats/{userId:guid}")]
        public async Task<IActionResult> GetStats(
            Guid userId,
            CancellationToken ct)
        {
            var result = await _sender.Send(new GetFitnessStatsQuery(userId), ct);
            return FromResult(result, "Fitness Stats retrieved successfully.", 200);
        }
    }

}


