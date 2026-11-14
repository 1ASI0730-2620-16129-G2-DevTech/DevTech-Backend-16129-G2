using System.ComponentModel.DataAnnotations;

namespace DevTech.OrderManagement.Interfaces.REST.Resources;

public record AddGarmentItemResource
{
    [Required] public string Type { get; init; } = string.Empty;

    public int Quantity { get; init; }

    public string? CareInstructions { get; init; }
}
