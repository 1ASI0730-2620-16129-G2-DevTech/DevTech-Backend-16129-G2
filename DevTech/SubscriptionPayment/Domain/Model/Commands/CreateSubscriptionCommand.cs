using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.Commands;

/// <summary>
///     Command to subscribe a customer to a subscription plan.
/// </summary>
/// <param name="CustomerId">The customer identifier (Customer &amp; Business Management BC, reference by id only).</param>
/// <param name="PlanId">The identifier of the subscription plan.</param>
public record CreateSubscriptionCommand(int CustomerId, SubscriptionPlanId PlanId)
{
    public CreateSubscriptionCommand(int customerId, int planId) : this(customerId, new SubscriptionPlanId(planId))
    {
    }
}
