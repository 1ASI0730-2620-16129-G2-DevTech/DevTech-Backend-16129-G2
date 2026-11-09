using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.SubscriptionPayment.Infrastructure.PaymentGateway.External;
using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Infrastructure.PaymentGateway;

/// <summary>
///     Anti-corruption layer between the Subscription &amp; Payment bounded context and the external payment provider.
/// </summary>
/// <remarks>
///     It calls the external provider API and translates its <see cref="ExternalChargeResponse" />
///     into the domain value object <see cref="PaymentResult" />.
/// </remarks>
/// <param name="externalApi">The <see cref="IExternalPaymentProviderApi" /> client.</param>
public class PaymentGatewayAcl(IExternalPaymentProviderApi externalApi)
{
    /// <summary>
    ///     Charges the given amount through the external provider.
    /// </summary>
    /// <param name="amount">The amount to charge.</param>
    /// <param name="cardToken">The tokenized card.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The translated <see cref="PaymentResult" />.</returns>
    public async Task<PaymentResult> ChargeAsync(Money amount, string cardToken, CancellationToken cancellationToken)
    {
        var response = await externalApi.ChargeAsync(cardToken, amount.Amount, amount.Currency, cancellationToken);
        return Translate(response);
    }

    /// <summary>
    ///     Translates the external provider response into a <see cref="PaymentResult" />.
    /// </summary>
    /// <param name="response">The <see cref="ExternalChargeResponse" /> to translate.</param>
    /// <returns>The equivalent <see cref="PaymentResult" />.</returns>
    public PaymentResult Translate(ExternalChargeResponse response)
    {
        var status = response.StatusCode.Equals("succeeded", StringComparison.OrdinalIgnoreCase)
            ? PaymentResult.Approved
            : PaymentResult.Rejected;
        return new PaymentResult(response.ExternalTransactionId, status);
    }
}
