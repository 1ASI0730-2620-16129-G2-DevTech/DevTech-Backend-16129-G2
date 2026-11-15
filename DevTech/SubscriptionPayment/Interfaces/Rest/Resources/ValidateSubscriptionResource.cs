namespace DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

/// <summary>
///     Resource to validate the active subscription of a customer.
/// </summary>
public record ValidateSubscriptionResource(int CustomerId);
