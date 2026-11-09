namespace DevTech.SubscriptionPayment.Infrastructure.PaymentGateway.External;

/// <summary>
///     Client contract of the external payment provider API.
/// </summary>
public interface IExternalPaymentProviderApi
{
    /// <summary>
    ///     Charges a tokenized card.
    /// </summary>
    /// <param name="cardToken">The tokenized card.</param>
    /// <param name="amount">The amount to charge.</param>
    /// <param name="currency">The ISO 4217 currency code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="ExternalChargeResponse" /> returned by the provider.</returns>
    /// <exception cref="HttpRequestException">Thrown when the provider cannot be reached.</exception>
    Task<ExternalChargeResponse> ChargeAsync(string cardToken, decimal amount, string currency,
        CancellationToken cancellationToken);
}
