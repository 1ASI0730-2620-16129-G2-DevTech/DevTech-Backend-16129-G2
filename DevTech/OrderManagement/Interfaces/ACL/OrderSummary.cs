namespace DevTech.OrderManagement.Interfaces.ACL;

/// <summary>
///     Summary of an order that Order Management shares with other bounded contexts.
/// </summary>
/// <remarks>
///     It only uses simple types (Guid and text), so the other bounded contexts never depend on
///     the Order aggregate or on the Order Management enums.
/// </remarks>
/// <param name="OrderId">The identifier of the order</param>
/// <param name="Status">The order status name, for example READY</param>
/// <param name="DeliveryMethod">The delivery method name: PICKUP or DELIVERY</param>
public record OrderSummary(Guid OrderId, string Status, string DeliveryMethod);
