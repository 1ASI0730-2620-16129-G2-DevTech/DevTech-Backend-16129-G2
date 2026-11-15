using DevTech.PickupsDeliveries.Domain.Model.ValueObjects;

namespace DevTech.PickupsDeliveries.Domain.Model.Queries;

/// <summary>
///     Query to get the pickup or the delivery of an order.
/// </summary>
/// <param name="OrderId">The identifier of the order to search</param>
/// <param name="Type">The type of service to search: Pickup or Delivery</param>
public record GetPickupDeliveryByOrderIdAndTypeQuery(Guid OrderId, ServiceType Type);