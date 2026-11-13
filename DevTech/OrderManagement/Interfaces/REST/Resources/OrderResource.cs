namespace DevTech.OrderManagement.Interfaces.REST.Resources;

public record OrderResource(
    Guid Id,
    Guid CustomerId,
    Guid LaundryId,
    string Status,
    string DeliveryMethod,
    string ServiceType,
    DateOnly EstimatedDeliveryDate,
    string? SpecialCareInstructions,
    DateTimeOffset CreatedAt,
    IEnumerable<GarmentItemResource> Items);
