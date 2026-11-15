namespace DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

/// <summary>
///     Resource to create a subscription plan.
/// </summary>
public record CreateSubscriptionPlanResource(string Name, decimal Price, string Currency, int Credits, int DurationDays);
