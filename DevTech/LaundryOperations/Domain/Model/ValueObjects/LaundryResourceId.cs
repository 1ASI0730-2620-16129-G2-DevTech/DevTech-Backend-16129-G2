using Acme.Center.Platform.Shared.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.ValueObjects;

/// <summary>
///     Represents the strongly-typed identifier for a Laundry Resource.
/// </summary>
/// <param name="Value">The underlying identifier value</param>
public readonly record struct LaundryResourceId(Guid Value) : IEntityId<Guid>
{
    public static implicit operator Guid(LaundryResourceId id) => id.Value;
    
    public static implicit operator LaundryResourceId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}