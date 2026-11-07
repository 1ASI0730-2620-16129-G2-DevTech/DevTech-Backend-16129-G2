using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.Queries;

/// <summary>
///     Query to get a subscription plan by its id.
/// </summary>
/// <param name="SubscriptionPlanId">The identifier of the subscription plan.</param>
public record GetSubscriptionPlanByIdQuery(SubscriptionPlanId SubscriptionPlanId)
{
    public GetSubscriptionPlanByIdQuery(int subscriptionPlanId) : this(new SubscriptionPlanId(subscriptionPlanId))
    {
    }
}
