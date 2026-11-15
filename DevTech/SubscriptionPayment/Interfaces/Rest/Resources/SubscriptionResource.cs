namespace DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

/// <summary>
///     Subscription resource for REST API.
/// </summary>
public record SubscriptionResource(int Id, int CustomerId, int PlanId, DateOnly StartDate, DateOnly ExpirationDate, string Status, int Credits);
