using DevTech.TrackingNotifications.Domain.Model.Aggregates;
using DevTech.TrackingNotifications.Domain.Model.Commands;

namespace DevTech.TrackingNotifications.Application.CommandServices;

/// <summary>
///     Use cases that change the tracking of an order.
/// </summary>
public interface IOrderTrackingCommandService
{
    /// <summary>
    ///     Starts the tracking of an order. If the order already has one, it is returned
    ///     (keeping the estimated delivery when the command brings it), so starting it twice is harmless.
    /// </summary>
    Task<OrderTracking> Handle(StartTrackingCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Records a new stage of an order, starting its tracking first if it did not have one.
    /// </summary>
    Task<OrderTracking> Handle(RecordTrackingStageCommand command, CancellationToken cancellationToken = default);
}
