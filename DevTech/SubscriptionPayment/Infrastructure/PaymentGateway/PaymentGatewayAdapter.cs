using DevTech.SubscriptionPayment.Application.Internal.OutboundServices;
using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Infrastructure.PaymentGateway;

/// <summary>
///     Adapter that implements the <see cref="IPaymentGateway" /> port using the <see cref="PaymentGatewayAcl" />.
/// </summary>
/// <param name="paymentGatewayAcl">The anti-corruption layer used to reach the external provider.</param>
public class PaymentGatewayAdapter(PaymentGatewayAcl paymentGatewayAcl) : IPaymentGateway
{
    /// <inheritdoc />
    public Task<PaymentResult> ProcessPaymentAsync(Money amount, string cardToken,
        CancellationToken cancellationToken)
    {
        return paymentGatewayAcl.ChargeAsync(amount, cardToken, cancellationToken);
    }
}
