namespace DevTech.PickupsDeliveries.Domain.Model.Queries;

/// <summary>
///     Query to get all the pickups and deliveries of an order.
/// </summary>
/// <param name="OrderId">The identifier of the order to search</param>
public record GetAllPickupDeliveriesByOrderIdQuery(int OrderId);