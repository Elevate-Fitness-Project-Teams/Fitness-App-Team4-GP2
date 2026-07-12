using BuildingBlocks.Shared.Controllers;
using FCEService.Domain.Enums;
using FCEService.Features.AssignPlan;
using FCEService.Features.CalculateMetrics;
using FCEService.Features.GetFitnessMetrics;
using FCEService.Features.GetFitnessPlanConfigs;
using FCEService.Features.GetFitnessStats;
using FCEService.Features.GetSpecificPlanConfiguration;
using FCEService.Features.RecalculateMetrics;
using FCEService.Features.SaveFitnessStats;
using FCEService.Features.UserAssignedPlans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCEService.Controllers
{
    [Authorize]
    [Route("api/v1/fitness")]
    public sealed class FitnessController(ISender sender) : ApiControllerBase
    {
        [HttpPost("stats")]
        public async Task<IActionResult> SubmitFitnessStats(
            [FromBody] SubmitFitnessStatsCommand command, CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return FromResult(result, "Fitness stats submitted successfully.", StatusCodes.Status201Created);
        }

        [HttpGet("stats/{userId:guid}")]
        public async Task<IActionResult> GetStats(Guid userId, CancellationToken ct)
        {
            var result = await sender.Send(new GetFitnessStatsQuery(userId), ct);
            return FromResult(result, "Fitness stats retrieved successfully.", StatusCodes.Status200OK);
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate(
            [FromBody] CalculateMetricsCommand command, CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return FromResult(result, "Fitness metrics calculated successfully.", StatusCodes.Status200OK);
        }

        [HttpGet("metrics/{userId:guid}")]
        public async Task<IActionResult> GetMetrics(Guid userId, CancellationToken ct)
        {
            var result = await sender.Send(new GetFitnessMetricsQuery(userId), ct);
            return FromResult(result, "Fitness metrics retrieved successfully.", StatusCodes.Status200OK);
        }

        [HttpPost("assign-plan")]
        public async Task<IActionResult> AssignPlan(
            [FromBody] AssignFitnessPlanCommand command, CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
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
            var result = await sender.Send(new GetFitnessPlanConfigsQuery(goal, status, page, pageSize), ct);
            return FromResult(result, "Fitness plan configurations retrieved successfully.", StatusCodes.Status200OK);
        }

        [HttpGet("plans/{planId}")]
        public async Task<IActionResult> GetPlanConfigDetail(string planId, CancellationToken ct)
        {
            var result = await sender.Send(new GetPlanConfigDetailQuery(planId), ct);
            return FromResult(result, "Plan configuration retrieved successfully.", StatusCodes.Status200OK);
        }

        [HttpPut("recalculate/{userId:guid}")]
        public async Task<IActionResult> Recalculate(
            Guid userId, [FromBody] RecalculateMetricsRequestBody? body, CancellationToken ct)
        {
            var command = new RecalculateMetricsCommand(userId, body?.Reason, body?.NewWeight, body?.TriggeredBy);
            var result = await sender.Send(command, ct);
            return FromResult(result, "Metrics recalculated successfully.", StatusCodes.Status200OK);
        }
    }
}