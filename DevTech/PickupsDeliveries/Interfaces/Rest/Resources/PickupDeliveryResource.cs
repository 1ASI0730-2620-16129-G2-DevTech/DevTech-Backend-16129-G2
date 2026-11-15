namespace DevTech.PickupsDeliveries.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a pickup or a delivery.
/// </summary>
/// <param name="Id">The server-generated identifier of the pickup or delivery</param>
/// <param name="OrderId">The identifier of the related order in Order Management</param>
/// <param name="Street">The street of the address</param>
/// <param name="City">The city of the address</param>
/// <param name="Type">The type of service: Pickup or Delivery</param>
/// <param name="Status">The current status: Scheduled, InTransit, Completed or Failed</param>
public record PickupDeliveryResource(
    int Id,
    Guid OrderId,
    string Street,
    string City,
    string Type,
    string Status);
