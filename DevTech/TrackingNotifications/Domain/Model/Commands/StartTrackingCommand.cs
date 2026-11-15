namespace DevTech.TrackingNotifications.Domain.Model.Commands;

/// <summary>
///     Starts the tracking of an order at the RECEIVED stage.
/// </summary>
/// <param name="OrderId">Reference (by id) to the order in Order Management.</param>
/// <param name="EstimatedDelivery">Date and time promised to the customer, when known.</param>
public record StartTrackingCommand(Guid OrderId, DateTimeOffset? EstimatedDelivery);
