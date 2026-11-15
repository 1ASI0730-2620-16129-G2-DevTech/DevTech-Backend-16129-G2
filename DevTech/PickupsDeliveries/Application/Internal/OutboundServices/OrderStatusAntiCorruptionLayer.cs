using DevTech.OrderManagement.Interfaces.ACL;
using DevTech.PickupsDeliveries.Application.Outbound;

namespace DevTech.PickupsDeliveries.Application.Internal.OutboundServices;

/// <summary>
///     Anti-corruption layer between Pickups and Deliveries and Order Management.
/// </summary>
/// <remarks>
///     It implements the <see cref="IOrderStatusFacade" /> outbound port by calling the
///     <see cref="IOrdersContextFacade" /> offered by Order Management, in memory and without HTTP.
///     It translates the order status and delivery method written by Order Management
///     into the only answers this bounded context needs.
///     If Order Management changes its model, only this class has to change.
/// </remarks>
/// <param name="ordersContextFacade">The facade offered by the Order Management bounded context</param>
public class OrderStatusAntiCorruptionLayer(IOrdersContextFacade ordersContextFacade) : IOrderStatusFacade
{
    private const string ReadyStatus = "READY";
    private const string HomeDeliveryMethod = "DELIVERY";

    /// <inheritdoc />
    public async Task<OrderStatusSnapshot?> FetchOrderStatusAsync(Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var orderSummary = await ordersContextFacade.FetchOrderSummaryByIdAsync(orderId, cancellationToken);
        return Translate(orderSummary);
    }

    /// <summary>
    ///     Translates the Order Management summary into a snapshot written in this bounded context language.
    /// </summary>
    /// <param name="orderSummary">The summary received from Order Management, or null if the order does not exist.</param>
    /// <returns>The order status snapshot, or null when the order does not exist.</returns>
    private static OrderStatusSnapshot? Translate(OrderSummary? orderSummary)
    {
        if (orderSummary is null)
            return null;

        var isReady = string.Equals(orderSummary.Status, ReadyStatus, StringComparison.OrdinalIgnoreCase);
        var isForHomeDelivery = string.Equals(orderSummary.DeliveryMethod, HomeDeliveryMethod,
            StringComparison.OrdinalIgnoreCase);
        return new OrderStatusSnapshot(isReady, isForHomeDelivery);
    }
}
