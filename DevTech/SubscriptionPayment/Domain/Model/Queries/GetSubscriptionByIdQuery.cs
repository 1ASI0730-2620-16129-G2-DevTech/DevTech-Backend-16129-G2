using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.Queries;

/// <summary>
///     Query to get a subscription by its id.
/// </summary>
/// <param name="SubscriptionId">The identifier of the subscription.</param>
public record GetSubscriptionByIdQuery(SubscriptionId SubscriptionId)
{
    public GetSubscriptionByIdQuery(int subscriptionId) : this(new SubscriptionId(subscriptionId))
    {
    }
}
