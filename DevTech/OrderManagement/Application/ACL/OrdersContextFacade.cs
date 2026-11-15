using DevTech.OrderManagement.Application.Services;
using DevTech.OrderManagement.Domain.Model.Queries;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.OrderManagement.Interfaces.ACL;

namespace DevTech.OrderManagement.Application.ACL;

/// <summary>
///     Implementation of the facade that Order Management offers to the other bounded contexts.
/// </summary>
/// <remarks>
///     It reuses the Order Management query service, so the read rules stay in one place.
/// </remarks>
/// <param name="orderQueryService">The Order Management query service</param>
public class OrdersContextFacade(IOrderQueryService orderQueryService) : IOrdersContextFacade
{
    /// <inheritdoc />
    public async Task<OrderSummary?> FetchOrderSummaryByIdAsync(Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await orderQueryService.Handle(new GetOrderByIdQuery(new OrderId(orderId)), cancellationToken);
        if (order is null)
            return null;

        return new OrderSummary(order.Id.Value, order.Status.ToString(), order.DeliveryMethod.ToString());
    }
}
