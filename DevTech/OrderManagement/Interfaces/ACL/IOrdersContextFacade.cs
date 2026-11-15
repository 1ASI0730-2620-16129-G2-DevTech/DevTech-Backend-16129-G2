namespace DevTech.OrderManagement.Interfaces.ACL;

/// <summary>
///     Facade that Order Management offers to the other bounded contexts of the platform.
/// </summary>
/// <remarks>
///     The other bounded contexts call this contract in memory, inside the same backend.
///     They never use the Order Management repositories or aggregates directly.
/// </remarks>
public interface IOrdersContextFacade
{
    /// <summary>
    ///     Gets the summary of an order.
    /// </summary>
    /// <param name="orderId">The identifier of the order.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The order summary, or null when the order does not exist.</returns>
    Task<OrderSummary?> FetchOrderSummaryByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}
