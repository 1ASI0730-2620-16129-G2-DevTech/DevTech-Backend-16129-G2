using DevTech.PickupsDeliveries.Domain.Model.Aggregates;
using DevTech.PickupsDeliveries.Interfaces.Rest.Resources;

namespace DevTech.PickupsDeliveries.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a PickupDeliveryResource from a PickupDelivery.
/// </summary>
public static class PickupDeliveryResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a PickupDeliveryResource from a PickupDelivery.
    /// </summary>
    /// <param name="entity">The PickupDelivery entity</param>
    /// <returns>
    ///     A PickupDeliveryResource assembled from the PickupDelivery
    /// </returns>
    public static PickupDeliveryResource ToResourceFromEntity(PickupDelivery entity) =>
        new(entity.Id,
            entity.OrderId,
            entity.Address.Street,
            entity.Address.City,
            entity.Type.ToString(),
            entity.Status.ToString());
}
