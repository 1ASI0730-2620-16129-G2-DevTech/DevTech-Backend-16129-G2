namespace DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

/// <summary>
///     Payment resource for REST API.
/// </summary>
public record PaymentResource(
    int Id,
    int CustomerId,
    Guid OrderId,
    decimal Amount,
    string Currency,
    string PaymentMethod,
    string Status,
    string? TransactionId,
    DateTimeOffset? ProcessedAt);
