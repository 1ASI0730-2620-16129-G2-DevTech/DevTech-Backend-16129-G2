using DevTech.OrderManagement.Domain.Model.Aggregates;
using DevTech.OrderManagement.Domain.Model.Queries;

namespace DevTech.OrderManagement.Application.Services;

/// <summary>
///     Read side of Order Management.
/// </summary>
public interface IOrderQueryService
{
    Task<Order?> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> Handle(GetAllOrdersByCustomerIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> Handle(GetAllOrdersByLaundryIdQuery query, CancellationToken cancellationToken = default);
}
