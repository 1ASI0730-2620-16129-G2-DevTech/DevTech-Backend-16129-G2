using DevTech.OrderManagement.Application.Services;
using DevTech.OrderManagement.Domain.Model.Aggregates;
using DevTech.OrderManagement.Domain.Model.Queries;
using DevTech.OrderManagement.Domain.Repositories;

namespace DevTech.OrderManagement.Application.Internal.QueryServices;

/// <summary>
///     Handles the read use cases of the <see cref="Order" /> aggregate.
/// </summary>
public class OrderQueryService(IOrderRepository orderRepository) : IOrderQueryService
{
    /// <inheritdoc />
    public Task<Order?> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken = default)
    {
        return orderRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<IEnumerable<Order>> Handle(GetAllOrdersByCustomerIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return orderRepository.FindByCustomerIdAsync(query.CustomerId, cancellationToken);
    }

    /// <inheritdoc />
    public Task<IEnumerable<Order>> Handle(GetAllOrdersByLaundryIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return orderRepository.FindByLaundryIdAsync(query.LaundryId, cancellationToken);
    }
}
