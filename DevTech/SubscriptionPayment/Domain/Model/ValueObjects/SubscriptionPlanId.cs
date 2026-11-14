using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

/// <summary>
///     Represents the strongly-typed identifier for a subscription plan in the Subscription &amp; Payment bounded context.
/// </summary>
/// <param name="Value">The underlying identifier value.</param>
public readonly record struct SubscriptionPlanId(int Value) : IEntityId<int>
{
    public static implicit operator int(SubscriptionPlanId id) => id.Value;
    public static implicit operator SubscriptionPlanId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
