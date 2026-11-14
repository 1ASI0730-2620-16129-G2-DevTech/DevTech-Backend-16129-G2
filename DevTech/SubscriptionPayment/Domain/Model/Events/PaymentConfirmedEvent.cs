using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.SubscriptionPayment.Domain.Model.Events;

/// <summary>
///     Domain event raised when a payment is confirmed.
/// </summary>
public record PaymentConfirmedEvent(PaymentId PaymentId, int OrderId, int CustomerId) : DomainEvent;
