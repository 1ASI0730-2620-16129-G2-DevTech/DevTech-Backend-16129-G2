using DevTech.PickupsDeliveries.Domain.Model.Aggregates;
using DevTech.PickupsDeliveries.Domain.Model.Queries;

namespace DevTech.PickupsDeliveries.Application.Services;

/// <summary>
///     Query service interface for pickup and delivery operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying pickups and deliveries by various filters.
///     It supports queries by order, by identifier, and by order and service type.
/// </remarks>
public interface IPickupDeliveryQueryService
{
    /// <summary>
    ///     Handle the GetAllPickupDeliveriesByOrderIdQuery.
    /// </summary>
    /// <remarks>
    ///     This method returns all the pickups and deliveries of the given order.
    /// </remarks>
    /// <param name="query">The GetAllPickupDeliveriesByOrderIdQuery query</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>An IEnumerable containing the PickupDelivery objects</returns>
    Task<IEnumerable<PickupDelivery>> Handle(GetAllPickupDeliveriesByOrderIdQuery query,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the GetPickupDeliveryByIdQuery.
    /// </summary>
    /// <remarks>
    ///     This method returns the pickup or delivery with the given identifier.
    /// </remarks>
    /// <param name="query">The GetPickupDeliveryByIdQuery query</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The PickupDelivery object if found, or null otherwise</returns>
    Task<PickupDelivery?> Handle(GetPickupDeliveryByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the GetPickupDeliveryByOrderIdAndTypeQuery.
    /// </summary>
    /// <remarks>
    ///     This method returns the pickup or the delivery of the given order.
    /// </remarks>
    /// <param name="query">The GetPickupDeliveryByOrderIdAndTypeQuery query</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The PickupDelivery object if found, or null otherwise</returns>
    Task<PickupDelivery?> Handle(GetPickupDeliveryByOrderIdAndTypeQuery query,
        CancellationToken cancellationToken = default);
}