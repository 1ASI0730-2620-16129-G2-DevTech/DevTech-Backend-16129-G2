using System.ComponentModel.DataAnnotations;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.OrderManagement.Interfaces.REST.Validation;

namespace DevTech.OrderManagement.Interfaces.REST.Resources;

public record CreateOrderResource
{
    public Guid CustomerId { get; init; }

    public Guid LaundryId { get; init; }

    /// <summary>PICKUP or DELIVERY.</summary>
    [Required]
    [EnumString(typeof(DeliveryMethod))]
    public string DeliveryMethod { get; init; } = string.Empty;

    /// <summary>WASHING, WASHING_DRYING, WASHING_IRONING, IRONING, DRYING or DRY_CLEANING.</summary>
    [Required]
    [EnumString(typeof(ServiceType))]
    public string ServiceType { get; init; } = string.Empty;

    public DateOnly EstimatedDeliveryDate { get; init; }

    public string? SpecialCareInstructions { get; init; }

    public IEnumerable<AddGarmentItemResource> Items { get; init; } = [];
}
