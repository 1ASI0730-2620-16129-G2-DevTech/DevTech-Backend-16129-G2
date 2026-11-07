using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.Commands;

/// <summary>
///     Command to confirm an approved payment.
/// </summary>
/// <param name="PaymentId">The identifier of the payment.</param>
public record ConfirmPaymentCommand(PaymentId PaymentId)
{
    public ConfirmPaymentCommand(int paymentId) : this(new PaymentId(paymentId))
    {
    }
}
