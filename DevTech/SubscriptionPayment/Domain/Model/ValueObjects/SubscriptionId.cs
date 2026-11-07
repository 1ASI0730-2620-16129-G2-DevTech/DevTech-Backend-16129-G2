using Acme.Center.Platform.Shared.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

/// <summary>
///     Represents the strongly-typed identifier for a subscription in the Subscription &amp; Payment bounded context.
/// </summary>
/// <param name="Value">The underlying identifier value.</param>
public readonly record struct SubscriptionId(int Value) : IEntityId<int>
{
    public static implicit operator int(SubscriptionId id) => id.Value;
    public static implicit operator SubscriptionId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
