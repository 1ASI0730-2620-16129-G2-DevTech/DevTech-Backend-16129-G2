namespace DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

/// <summary>
///     Payment resource for REST API.
/// </summary>
public record PaymentResource(int Id, int CustomerId, int OrderId, decimal Amount, string Currency, string Status, string? TransactionId, DateTimeOffset? ProcessedAt);
