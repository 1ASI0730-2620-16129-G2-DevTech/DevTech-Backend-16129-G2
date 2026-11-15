using DevTech.LaundryOperations.Domain.Model.Events;
using DevTech.Shared.Application.Internal.EventHandlers;
using DevTech.TrackingNotifications.Application.CommandServices;
using DevTech.TrackingNotifications.Domain.Model.Commands;

namespace DevTech.TrackingNotifications.Application.Internal.EventHandlers;

/// <summary>
///     Policy "When <c>StageAdvanced</c> occurs in Laundry Operations, then Tracking records the stage change".
/// </summary>
/// <remarks>
///     The event is published right after Laundry Operations saved its changes (DispatchDomainEventsInterceptor),
///     so the tracking is saved in its own scope (its own unit of work), apart from the save that raised the event.
/// </remarks>
public class StageAdvancedEventHandler(
    IServiceScopeFactory scopeFactory,
    TimeProvider timeProvider,
    ILogger<StageAdvancedEventHandler> logger) : IEventHandler<StageAdvanced>
{
    /// <inheritdoc />
    public async Task Handle(StageAdvanced domainEvent, CancellationToken cancellationToken)
    {
        var stage = ProcessingStageTranslator.ToTrackingStage(domainEvent.Stage);

        using var scope = scopeFactory.CreateScope();
        var commandService = scope.ServiceProvider.GetRequiredService<IOrderTrackingCommandService>();
        await commandService.Handle(
            new RecordTrackingStageCommand(domainEvent.OrderId, stage, timeProvider.GetUtcNow()), cancellationToken);

        logger.LogInformation("Tracking of order {OrderId} moved to {Stage}", domainEvent.OrderId, stage);
    }
}
