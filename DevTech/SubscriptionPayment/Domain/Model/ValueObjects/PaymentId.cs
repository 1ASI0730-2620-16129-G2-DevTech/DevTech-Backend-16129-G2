using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

/// <summary>
///     Represents the strongly-typed identifier for a payment in the Subscription &amp; Payment bounded context.
/// </summary>
/// <param name="Value">The underlying identifier value.</param>
public readonly record struct PaymentId(int Value) : IEntityId<int>
{
    public static implicit operator int(PaymentId id) => id.Value;
    public static implicit operator PaymentId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
