using BuildingBlocks.Shared.Controllers;
using FCEService.Features.SaveFitnessStats;
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
    }

}
