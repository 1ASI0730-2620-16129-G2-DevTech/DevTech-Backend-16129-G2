using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.Commands;

/// <summary>
///     Command to create a payment for an order and charge it through the payment gateway.
/// </summary>
/// <param name="CustomerId">The customer identifier (Customer &amp; Business Management BC, reference by id only).</param>
/// <param name="OrderId">The order identifier (Order Management BC, reference by id only).</param>
/// <param name="Amount">The amount to charge.</param>
/// <param name="Currency">The ISO 4217 currency code.</param>
/// <param name="CardToken">The tokenized card provided by the client to the payment provider.</param>
/// <param name="PaymentMethod">How the customer pays (Yape, card or cash).</param>
public record ProcessPaymentCommand(
    int CustomerId,
    Guid OrderId,
    decimal Amount,
    string Currency,
    string CardToken,
    EPaymentMethod PaymentMethod = EPaymentMethod.CARD);
