using Acme.Center.Platform.Shared.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.ValueObjects;

/// <summary>
///     Represents the strongly-typed identifier for a Laundry Order.
/// </summary>
/// <param name="Value">The underlying identifier value</param>
public readonly record struct LaundryOrderId(int Value) : IEntityId<int>
{
    public static implicit operator int(LaundryOrderId id) => id.Value;
    
    public static implicit operator LaundryOrderId(int value) => new(value);
    
    public override string ToString() => Value.ToString();
}