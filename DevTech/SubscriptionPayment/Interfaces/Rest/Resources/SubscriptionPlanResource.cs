namespace DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

/// <summary>
///     Subscription plan resource for REST API.
/// </summary>
public record SubscriptionPlanResource(int Id, string Name, decimal Price, string Currency, int Credits, int DurationDays);
