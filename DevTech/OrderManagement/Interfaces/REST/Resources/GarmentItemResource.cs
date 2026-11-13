namespace DevTech.OrderManagement.Interfaces.REST.Resources;

public record GarmentItemResource(Guid Id, string Type, int Quantity, string? CareInstructions);
