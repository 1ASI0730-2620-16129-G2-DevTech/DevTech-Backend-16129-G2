using DevTech.OrderManagement.Domain.Model.Aggregates;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.OrderManagement.Domain.Repositories;

/// <summary>
///     Repository of the <see cref="Order" /> aggregate.
/// </summary>
/// <remarks>
///     <see cref="IBaseRepository{TEntity}.FindByIdAsync(int, CancellationToken)" /> looks entities up by an
///     <see cref="int" /> and cannot work with <see cref="OrderId" />, so this contract adds a strongly-typed overload.
///     Always use <see cref="FindByIdAsync(OrderId, CancellationToken)" /> for orders.
/// </remarks>
public interface IOrderRepository : IBaseRepository<Order>
{
    /// <summary>
    ///     Finds an order (tracked, with its garment items) by its id.
    /// </summary>
    Task<Order?> FindByIdAsync(OrderId id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Lists the orders of a customer, newest first.
    /// </summary>
    Task<IEnumerable<Order>> FindByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Lists the orders handled by a laundry, newest first.
    /// </summary>
    Task<IEnumerable<Order>> FindByLaundryIdAsync(Guid laundryId, CancellationToken cancellationToken = default);
}
