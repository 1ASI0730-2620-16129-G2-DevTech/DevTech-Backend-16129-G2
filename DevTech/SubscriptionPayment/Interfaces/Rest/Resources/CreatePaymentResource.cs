namespace DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

/// <summary>
///     Resource to create and process a payment for an order.
/// </summary>
/// <param name="PaymentMethod">YAPE, CARD or CASH (case-insensitive); CARD when it is not sent.</param>
public record CreatePaymentResource(
    int CustomerId,
    Guid OrderId,
    decimal Amount,
    string Currency,
    string CardToken,
    string? PaymentMethod = null);
