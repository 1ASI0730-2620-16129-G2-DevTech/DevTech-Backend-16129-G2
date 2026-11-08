using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.SubscriptionPayment.Domain.Model.Events;

/// <summary>
///     Domain event raised when a payment is rejected by the payment provider.
/// </summary>
public record PaymentFailedEvent(PaymentId PaymentId, int OrderId) : DomainEvent;
