using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.Commands;

/// <summary>
///     Command to apply a subscription benefit, consuming one credit.
/// </summary>
/// <param name="SubscriptionId">The identifier of the subscription.</param>
public record ApplyBenefitCommand(SubscriptionId SubscriptionId)
{
    public ApplyBenefitCommand(int subscriptionId) : this(new SubscriptionId(subscriptionId))
    {
    }
}
