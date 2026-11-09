using System.Text.Json;

namespace DevTech.SubscriptionPayment.Infrastructure.PaymentGateway.External;

/// <summary>
///     Simulated implementation of the external payment provider API, used until a real provider is integrated.
/// </summary>
/// <remarks>
///     Any card token containing "declined" is rejected; every other token is approved.
///     To integrate a real provider, implement <see cref="IExternalPaymentProviderApi" /> with an HTTP client
///     and register it in Program.cs instead of this class.
/// </remarks>
public class SimulatedExternalPaymentProviderApi : IExternalPaymentProviderApi
{
    /// <inheritdoc />
    public Task<ExternalChargeResponse> ChargeAsync(string cardToken, decimal amount, string currency,
        CancellationToken cancellationToken)
    {
        var declined = cardToken.Contains("declined", StringComparison.OrdinalIgnoreCase);
        var transactionId = $"sim_{Guid.NewGuid():N}";
        var statusCode = declined ? "declined" : "succeeded";
        var statusMessage = declined ? "The card was declined." : "The charge was successful.";
        var rawPayload = JsonSerializer.Serialize(new
        {
            id = transactionId,
            status = statusCode,
            message = statusMessage,
            amount,
            currency
        });

        return Task.FromResult(new ExternalChargeResponse(transactionId, statusCode, statusMessage, rawPayload));
    }
}
