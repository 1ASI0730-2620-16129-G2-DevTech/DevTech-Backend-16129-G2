namespace DevTech.PickupsDeliveries.Application.Outbound;

/// <summary>
///     The information about an order that the Pickups and Deliveries bounded context needs.
/// </summary>
/// <remarks>
///     It is written in the language of this bounded context, not in the language of Order Management:
///     it only answers the two questions the scheduling rules ask.
/// </remarks>
/// <param name="IsReady">True when the order is ready to be delivered.</param>
/// <param name="IsForHomeDelivery">True when the customer asked to receive the order at home.</param>
public record OrderStatusSnapshot(bool IsReady, bool IsForHomeDelivery);
