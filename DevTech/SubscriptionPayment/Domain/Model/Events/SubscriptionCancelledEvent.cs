using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.SubscriptionPayment.Domain.Model.Events;

/// <summary>
///     Domain event raised when a subscription is cancelled.
/// </summary>
public record SubscriptionCancelledEvent(SubscriptionId SubscriptionId) : DomainEvent;
