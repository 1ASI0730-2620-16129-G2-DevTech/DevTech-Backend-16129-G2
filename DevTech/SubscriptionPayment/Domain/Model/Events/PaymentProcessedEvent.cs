using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.SubscriptionPayment.Domain.Model.Events;

/// <summary>
///     Domain event raised when a payment is approved by the payment provider.
/// </summary>
public record PaymentProcessedEvent(PaymentId PaymentId, int OrderId, string TransactionId) : DomainEvent;
