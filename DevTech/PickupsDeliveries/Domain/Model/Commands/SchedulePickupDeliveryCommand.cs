using DevTech.PickupsDeliveries.Domain.Model.ValueObjects;

namespace DevTech.PickupsDeliveries.Domain.Model.Commands;

/// <summary>
///     Command to schedule the pickup or the delivery of a laundry order.
/// </summary>
/// <param name="OrderId">The identifier of the order</param>
/// <param name="Address">The validated address where the service takes place</param>
/// <param name="Type">The type of service: Pickup or Delivery</param>
public record SchedulePickupDeliveryCommand(int OrderId, DeliveryAddress Address, ServiceType Type);