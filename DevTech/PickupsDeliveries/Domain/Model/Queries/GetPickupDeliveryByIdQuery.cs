namespace DevTech.PickupsDeliveries.Domain.Model.Queries;

/// <summary>
///     Query to get a pickup or delivery by its identifier.
/// </summary>
/// <param name="Id">The identifier of the pickup or delivery to search</param>
public record GetPickupDeliveryByIdQuery(int Id);