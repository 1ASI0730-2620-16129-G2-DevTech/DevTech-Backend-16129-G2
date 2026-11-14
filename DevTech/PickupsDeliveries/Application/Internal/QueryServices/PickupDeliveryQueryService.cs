using DevTech.PickupsDeliveries.Application.Services;
using DevTech.PickupsDeliveries.Domain.Model.Aggregates;
using DevTech.PickupsDeliveries.Domain.Model.Queries;
using DevTech.PickupsDeliveries.Domain.Repositories;

namespace DevTech.PickupsDeliveries.Application.Internal.QueryServices;

/// <summary>
///     Query service for pickup and delivery operations.
/// </summary>
/// <remarks>
///     Queries only read data, so they never change the state of the system.
/// </remarks>
/// <param name="pickupDeliveryRepository">The pickup and delivery repository</param>
public class PickupDeliveryQueryService(IPickupDeliveryRepository pickupDeliveryRepository)
    : IPickupDeliveryQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<PickupDelivery>> Handle(GetAllPickupDeliveriesByOrderIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await pickupDeliveryRepository.FindByOrderIdAsync(query.OrderId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PickupDelivery?> Handle(GetPickupDeliveryByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await pickupDeliveryRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PickupDelivery?> Handle(GetPickupDeliveryByOrderIdAndTypeQuery query,
        CancellationToken cancellationToken = default)
    {
        return await pickupDeliveryRepository.FindByOrderIdAndTypeAsync(query.OrderId, query.Type,
            cancellationToken);
    }
}