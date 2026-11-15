using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.Commands;

/// <summary>
///     Command to cancel a subscription.
/// </summary>
/// <param name="SubscriptionId">The identifier of the subscription.</param>
public record CancelSubscriptionCommand(SubscriptionId SubscriptionId)
{
    public CancelSubscriptionCommand(int subscriptionId) : this(new SubscriptionId(subscriptionId))
    {
    }
}
