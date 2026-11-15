using DevTech.PickupsDeliveries.Domain.Model.Commands;
using DevTech.PickupsDeliveries.Domain.Model.ValueObjects;
using DevTech.PickupsDeliveries.Interfaces.Rest.Resources;

namespace DevTech.PickupsDeliveries.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a SchedulePickupDeliveryCommand from a SchedulePickupDeliveryResource.
/// </summary>
public static class SchedulePickupDeliveryCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a SchedulePickupDeliveryCommand from a SchedulePickupDeliveryResource.
    /// </summary>
    /// <remarks>
    ///     Creating the DeliveryAddress value object validates the street and the city.
    /// </remarks>
    /// <param name="resource">The SchedulePickupDeliveryResource resource</param>
    /// <returns>
    ///     A SchedulePickupDeliveryCommand assembled from the SchedulePickupDeliveryResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the address or the service type is not valid.</exception>
    public static SchedulePickupDeliveryCommand ToCommandFromResource(SchedulePickupDeliveryResource resource) =>
        new(resource.OrderId,
            new DeliveryAddress(resource.Street, resource.City),
            ServiceTypeFromStringAssembler.ToServiceTypeFromString(resource.Type));
}
