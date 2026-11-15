using DevTech.OrderManagement.Domain.Model.Commands;
using DevTech.OrderManagement.Domain.Model.Exceptions;
using DevTech.OrderManagement.Domain.Model.ValueObjects;

namespace DevTech.OrderManagement.Domain.Model.Entities;

/// <summary>
///     A garment (or group of identical garments) that belongs to an <c>Order</c>.
/// </summary>
/// <remarks>
///     Child entity of the <c>Order</c> aggregate: it is only created and removed through the aggregate root.
/// </remarks>
public class GarmentItem
{
    public const int MaxTypeLength = 100;
    public const int MaxCareInstructionsLength = 500;
    public const int MaxQuantity = 1000;

    /// <summary>
    ///     Parameterless constructor required by EF Core.
    /// </summary>
    private GarmentItem()
    {
    }

    /// <summary>
    ///     Creates a garment from an <see cref="AddGarmentItemCommand" />.
    /// </summary>
    /// <exception cref="OrderDomainException">When the type or the quantity are invalid.</exception>
    public GarmentItem(AddGarmentItemCommand command)
    {
        var type = command.Type?.Trim();

        if (string.IsNullOrWhiteSpace(type))
            throw new OrderDomainException(OrderRule.InvalidGarmentItem, "The garment type is required.");
        if (type.Length > MaxTypeLength)
            throw new OrderDomainException(OrderRule.InvalidGarmentItem,
                $"The garment type cannot exceed {MaxTypeLength} characters.");
        if (command.Quantity is < 1 or > MaxQuantity)
            throw new OrderDomainException(OrderRule.InvalidGarmentItem,
                $"The garment quantity must be between 1 and {MaxQuantity}.");

        var care = string.IsNullOrWhiteSpace(command.CareInstructions) ? null : command.CareInstructions.Trim();
        if (care is { Length: > MaxCareInstructionsLength })
            throw new OrderDomainException(OrderRule.InvalidGarmentItem,
                $"The care instructions cannot exceed {MaxCareInstructionsLength} characters.");

        Id = GarmentItemId.New();
        OrderId = command.OrderId;
        Type = type;
        Quantity = command.Quantity;
        CareInstructions = care;
    }

    public GarmentItemId Id { get; private set; }
    public OrderId OrderId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public string? CareInstructions { get; private set; }
}
