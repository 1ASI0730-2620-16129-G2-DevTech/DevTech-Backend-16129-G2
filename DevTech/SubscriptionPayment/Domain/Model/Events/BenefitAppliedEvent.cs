using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.SubscriptionPayment.Domain.Model.Events;

/// <summary>
///     Domain event raised when a subscription benefit (credit) is applied.
/// </summary>
public record BenefitAppliedEvent(SubscriptionId SubscriptionId, int RemainingCredits) : DomainEvent;
