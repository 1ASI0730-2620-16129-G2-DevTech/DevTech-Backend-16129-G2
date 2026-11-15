namespace DevTech.TrackingNotifications.Interfaces.Rest.Resources;

/// <summary>
///     Resource to start the tracking of an order.
/// </summary>
/// <param name="OrderId">The order identifier in Order Management (required).</param>
/// <param name="EstimatedDelivery">Date and time promised to the customer (optional).</param>
public record StartTrackingResource(Guid OrderId, DateTimeOffset? EstimatedDelivery);
