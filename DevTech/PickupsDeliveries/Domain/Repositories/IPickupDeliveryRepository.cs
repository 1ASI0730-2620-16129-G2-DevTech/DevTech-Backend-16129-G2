using DevTech.PickupsDeliveries.Domain.Model.Aggregates;
using DevTech.PickupsDeliveries.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.PickupsDeliveries.Domain.Repositories;

/// <summary>
///     The PickupDelivery repository contract.
/// </summary>
public interface IPickupDeliveryRepository : IBaseRepository<PickupDelivery>
{
    /// <summary>
    ///     Find all the pickups and deliveries of an order.
    /// </summary>
    /// <param name="orderId">The identifier of the order to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the pickup and delivery objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<PickupDelivery>> FindByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find the pickup or the delivery of an order.
    /// </summary>
    /// <param name="orderId">The identifier of the order to search.</param>
    /// <param name="type">The type of service to search: Pickup or Delivery.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     The pickup or delivery object if found, or null otherwise.
    /// </returns>
    Task<PickupDelivery?> FindByOrderIdAndTypeAsync(Guid orderId, ServiceType type,
        CancellationToken cancellationToken = default);
}