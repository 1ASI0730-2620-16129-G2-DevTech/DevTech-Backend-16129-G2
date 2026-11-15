namespace DevTech.TrackingNotifications.Domain.Model.Queries;

/// <summary>
///     Gets the trackings, optionally only the one of an order.
/// </summary>
/// <param name="OrderId">The order identifier; null gets the trackings of every order.</param>
public record GetAllTrackingsQuery(Guid? OrderId);
