namespace DevTech.PickupsDeliveries.Application.Outbound;

/// <summary>
///     Outbound port to ask the Order Management bounded context about the status of an order.
/// </summary>
/// <remarks>
///     The application layer depends only on this contract.
///     The infrastructure layer implements it with an anti-corruption layer,
///     so the Order Management model never leaks into the Pickups and Deliveries model.
/// </remarks>
public interface IOrderStatusFacade
{
    /// <summary>
    ///     Checks whether an order is ready to be delivered.
    /// </summary>
    /// <param name="orderId">The identifier of the order.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True when the order exists and its status is Ready; otherwise, false.</returns>
    Task<bool> IsOrderReadyAsync(int orderId, CancellationToken cancellationToken = default);
}