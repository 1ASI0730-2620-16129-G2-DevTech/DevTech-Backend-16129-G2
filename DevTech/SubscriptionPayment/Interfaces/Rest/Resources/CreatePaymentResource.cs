namespace DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

/// <summary>
///     Resource to create and process a payment for an order.
/// </summary>
public record CreatePaymentResource(int CustomerId, int OrderId, decimal Amount, string Currency, string CardToken);
