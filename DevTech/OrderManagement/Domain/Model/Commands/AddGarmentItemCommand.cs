using DevTech.OrderManagement.Domain.Model.ValueObjects;

namespace DevTech.OrderManagement.Domain.Model.Commands;

/// <summary>
///     Adds a garment to an order that is still in <c>CREATED</c> status.
/// </summary>
/// <remarks>
///     When this command is nested inside <see cref="CreateOrderCommand" /> the order does not exist yet,
///     so <paramref name="OrderId" /> is ignored: the <c>Order</c> aggregate stamps its own id on every item.
/// </remarks>
/// <param name="OrderId">The order that receives the garment.</param>
/// <param name="Type">Kind of garment (shirt, jeans, coat...).</param>
/// <param name="Quantity">Number of units of this garment.</param>
/// <param name="CareInstructions">Optional care instructions specific to this garment.</param>
public record AddGarmentItemCommand(OrderId OrderId, string Type, int Quantity, string? CareInstructions);
