namespace DevTech.SubscriptionPayment.Infrastructure.PaymentGateway.External;

/// <summary>
///     DTO returned by the external payment provider when charging a card.
/// </summary>
/// <remarks>
///     This structure belongs to the external provider. It must never leak into the domain:
///     the anti-corruption layer translates it into a <c>PaymentResult</c>.
/// </remarks>
/// <param name="ExternalTransactionId">The transaction identifier assigned by the provider.</param>
/// <param name="StatusCode">The provider status code (e.g. "succeeded", "declined").</param>
/// <param name="StatusMessage">A human-readable message from the provider.</param>
/// <param name="RawPayload">The raw response payload, kept for auditing purposes.</param>
public record ExternalChargeResponse(
    string ExternalTransactionId,
    string StatusCode,
    string StatusMessage,
    string RawPayload);
