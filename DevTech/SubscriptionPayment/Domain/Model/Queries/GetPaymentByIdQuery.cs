using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.Queries;

/// <summary>
///     Query to get a payment by its id.
/// </summary>
/// <param name="PaymentId">The identifier of the payment.</param>
public record GetPaymentByIdQuery(PaymentId PaymentId)
{
    public GetPaymentByIdQuery(int paymentId) : this(new PaymentId(paymentId))
    {
    }
}
