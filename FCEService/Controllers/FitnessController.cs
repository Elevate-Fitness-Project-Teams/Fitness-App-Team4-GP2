using BuildingBlocks.Shared.Controllers;
using FCEService.Common;
using FCEService.Domain.Enums;
using FCEService.Features.CalculateMetrics;
using FCEService.Features.GetFitnessMetrics;
using FCEService.Features.GetFitnessPlanConfigs;
using FCEService.Features.GetFitnessStats;
using FCEService.Features.GetSpecificPlanConfiguration;
using FCEService.Features.SaveFitnessStats;
using FCEService.Features.Shared;
using FCEService.Features.UserAssignedPlans;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
        public async Task<IActionResult> GetMetrics(Guid userId, CancellationToken ct)
        {
            var result = await _sender.Send(new GetFitnessMetricsQuery(userId), ct);
            return FromResult(result, "Fitness metrics retrieved successfully.", 200);
        }

        [HttpGet("stats/{userId:guid}")]
        public async Task<IActionResult> GetStats(
            Guid userId,
            CancellationToken ct)
        {
            var result = await _sender.Send(new GetFitnessStatsQuery(userId), ct);
            return FromResult(result, "Fitness Stats retrieved successfully.", 200);
        }

        [HttpPost("assign-plan")]
        public async Task<IActionResult> AssignPlan(
        [FromBody] AssignFitnessPlanCommand command, CancellationToken ct)
        {
            var result = await _sender.Send(command, ct);
            return FromResult(result, "Fitness plan assigned successfully.", StatusCodes.Status200OK);
        }

        [HttpGet("plan-configs")]
        public async Task<IActionResult> GetPlanConfigs(
        [FromQuery] FitnessGoal? goal,
        [FromQuery] PlanStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        {
            var result = await _sender.Send(new GetFitnessPlanConfigsQuery(goal, status, page, pageSize), ct);
            return FromResult(result, "Fitness plan configurations retrieved successfully.", StatusCodes.Status200OK);
        }

        [HttpGet("plans/{planId}")]
        public async Task<IActionResult> GetPlanConfigDetail(string planId, CancellationToken ct)
        {
            var result = await _sender.Send(new GetPlanConfigDetailQuery(planId), ct);
            return FromResult(result, "Plan configuration retrieved successfully.", StatusCodes.Status200OK);
        }


    }

}


