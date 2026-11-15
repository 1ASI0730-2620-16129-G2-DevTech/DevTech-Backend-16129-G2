using System.ComponentModel.DataAnnotations;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.OrderManagement.Interfaces.REST.Validation;

namespace DevTech.OrderManagement.Interfaces.REST.Resources;

public record ChangeOrderStatusResource
{
    /// <summary>CREATED, RECEIVED, CLASSIFIED, IN_PROCESS, READY or DELIVERED.</summary>
    [Required]
    [EnumString(typeof(OrderStatus))]
    public string Status { get; init; } = string.Empty;
}
