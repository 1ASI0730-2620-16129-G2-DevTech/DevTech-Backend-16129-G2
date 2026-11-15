using DevTech.OrderManagement.Domain.Model.ValueObjects;

namespace DevTech.OrderManagement.Domain.Model.Commands;

/// <summary>
///     Creates a new order with its initial garment items.
/// </summary>
/// <param name="CustomerId">Reference (by id) to the customer in Customer &amp; Business Management.</param>
/// <param name="LaundryId">Reference (by id) to the laundry in Customer &amp; Business Management.</param>
/// <param name="DeliveryMethod">Pickup or delivery.</param>
/// <param name="ServiceType">Requested laundry service.</param>
/// <param name="EstimatedDeliveryDate">Date promised to the customer.</param>
/// <param name="SpecialCareInstructions">Optional instructions that apply to the whole order.</param>
/// <param name="Items">Initial garments. At least one is required.</param>
public record CreateOrderCommand(
    Guid CustomerId,
    Guid LaundryId,
    DeliveryMethod DeliveryMethod,
    ServiceType ServiceType,
    DateOnly EstimatedDeliveryDate,
    string? SpecialCareInstructions,
    IEnumerable<AddGarmentItemCommand> Items);
