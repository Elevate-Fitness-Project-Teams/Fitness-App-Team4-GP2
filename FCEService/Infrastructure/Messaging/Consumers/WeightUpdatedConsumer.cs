using BuildingBlocks.Contracts.Progress;
using FCEService.Features.RecalculateMetrics;
using MassTransit;
using MediatR;

namespace FCEService.Infrastructure.Messaging.Consumers
{
    public sealed class WeightUpdatedConsumer(ISender sender, ILogger<WeightUpdatedConsumer> logger)
        : IConsumer<WeightUpdatedEvent>
    {
        public async Task Consume(ConsumeContext<WeightUpdatedEvent> context)
        {
            var message = context.Message;

            logger.LogInformation(
                "Received weight_updated event for UserId {UserId}, WeightKg {WeightKg}",
                message.UserId, message.WeightKg);

            var command = new RecalculateMetricsCommand(
                message.UserId,
                Reason: "weight_update", 
                NewWeight: message.WeightKg,
                TriggeredBy: "weight_updated_event");

            var result = await sender.Send(command, context.CancellationToken);

            if (result.IsFailure)
            {
                logger.LogWarning(
                    "Recalculate failed for UserId {UserId} via weight_updated event: {Errors}",
                    message.UserId, string.Join(", ", result.Errors.Select(e => e.Code)));
            }
            else
            {
                logger.LogInformation(
                    "Recalculate succeeded for UserId {UserId} via weight_updated event. PlanReassignment: {PlanReassignment}",
                    message.UserId, result.Value.PlanReassignment);
            }
        }
    }
}