namespace DevTech.OrderManagement.Domain.Model.Exceptions;

/// <summary>
///     Business rules that the <c>Order</c> aggregate can reject.
///     The application layer translates each rule into its own <c>OrderError</c>.
/// </summary>
public enum OrderRule
{
    InvalidCustomer,
    InvalidLaundry,
    InvalidEstimatedDeliveryDate,
    InvalidSpecialCareInstructions,
    InvalidGarmentItem,
    GarmentItemsLocked,
    InvalidStatusTransition,
    GarmentItemNotFound
}

/// <summary>
///     Thrown by the <c>Order</c> aggregate (and its entities) when an invariant would be violated.
///     The aggregate always checks before mutating, so it is never left half-modified.
/// </summary>
/// <param name="rule">The violated rule.</param>
/// <param name="message">Human-readable explanation.</param>
public sealed class OrderDomainException(OrderRule rule, string message) : Exception(message)
{
    /// <summary>
    ///     Gets the violated rule.
    /// </summary>
    public OrderRule Rule { get; } = rule;
}
