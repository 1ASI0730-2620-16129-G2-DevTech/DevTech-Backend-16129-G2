using DevTech.Shared.Domain.Model.Events;
using DevTech.TrackingNotifications.Domain.Model.ValueObjects;

namespace DevTech.TrackingNotifications.Domain.Model.Events;

/// <summary>
///     The tracking of an order recorded a new stage.
/// </summary>
/// <remarks>
///     Notifications can subscribe to it to send the <c>order-stage-changed</c> notification.
/// </remarks>
public record TrackingStageRecorded(int TrackingId, Guid OrderId, ETrackingStage Stage) : IEvent;
