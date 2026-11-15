using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.SubscriptionPayment.Domain.Model.Events;

/// <summary>
///     Domain event raised when a payment is approved by the payment provider.
/// </summary>
public record PaymentProcessedEvent(PaymentId PaymentId, Guid OrderId, string TransactionId) : DomainEvent;
