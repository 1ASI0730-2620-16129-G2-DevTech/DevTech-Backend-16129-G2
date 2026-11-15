using DevTech.LaundryOperations.Domain.Model.Events;
using DevTech.Shared.Application.Internal.EventHandlers;
using DevTech.TrackingNotifications.Application.CommandServices;
using DevTech.TrackingNotifications.Domain.Model.Commands;

namespace DevTech.TrackingNotifications.Application.Internal.EventHandlers;

/// <summary>
///     When Laundry Operations receives an order, its tracking starts at RECEIVED (if it was not started yet).
/// </summary>
/// <remarks>
///     Saved in its own scope, apart from the save of Laundry Operations that raised the event.
/// </remarks>
public class LaundryOrderReceivedEventHandler(
    IServiceScopeFactory scopeFactory,
    ILogger<LaundryOrderReceivedEventHandler> logger) : IEventHandler<LaundryOrderReceived>
{
    /// <inheritdoc />
    public async Task Handle(LaundryOrderReceived domainEvent, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var commandService = scope.ServiceProvider.GetRequiredService<IOrderTrackingCommandService>();
        await commandService.Handle(new StartTrackingCommand(domainEvent.OrderId, null), cancellationToken);

        logger.LogInformation("Tracking of order {OrderId} started", domainEvent.OrderId);
    }
}
