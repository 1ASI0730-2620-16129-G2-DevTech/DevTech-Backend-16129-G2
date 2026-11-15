using DevTech.OrderManagement.Domain.Model.ValueObjects;

namespace DevTech.OrderManagement.Domain.Model.Commands;

/// <summary>
///     Removes a garment from an order that is still in <c>CREATED</c> status.
/// </summary>
public record RemoveGarmentItemCommand(OrderId OrderId, GarmentItemId GarmentItemId);
