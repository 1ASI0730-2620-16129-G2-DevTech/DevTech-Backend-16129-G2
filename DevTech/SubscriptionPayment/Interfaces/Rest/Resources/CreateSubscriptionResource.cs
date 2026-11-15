namespace DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

/// <summary>
///     Resource to subscribe a customer to a subscription plan.
/// </summary>
public record CreateSubscriptionResource(int CustomerId, int PlanId);
