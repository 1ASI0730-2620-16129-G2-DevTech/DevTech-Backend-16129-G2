namespace DevTech.PickupsDeliveries.Application.Outbound;

/// <summary>
///     Outbound port to ask the Order Management bounded context about an order.
/// </summary>
/// <remarks>
///     The command service depends only on this contract.
///     It is implemented by an anti-corruption layer that calls the facade offered by Order Management,
///     so the Order Management model never leaks into the Pickups and Deliveries model.
/// </remarks>
public interface IOrderStatusFacade
{
    /// <summary>
    ///     Gets the information about an order that the scheduling rules need.
    /// </summary>
    /// <param name="orderId">The identifier of the order.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The order status snapshot, or null when the order does not exist.</returns>
    Task<OrderStatusSnapshot?> FetchOrderStatusAsync(Guid orderId, CancellationToken cancellationToken = default);
}
