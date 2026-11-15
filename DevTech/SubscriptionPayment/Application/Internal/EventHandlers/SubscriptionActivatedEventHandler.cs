using DevTech.SubscriptionPayment.Domain.Model.Events;
using DevTech.Shared.Application.Internal.EventHandlers;

namespace DevTech.SubscriptionPayment.Application.Internal.EventHandlers;

/// <summary>
///     Event handler for <see cref="SubscriptionActivatedEvent" />.
/// </summary>
public class SubscriptionActivatedEventHandler(ILogger<SubscriptionActivatedEventHandler> logger)
    : IEventHandler<SubscriptionActivatedEvent>
{
    /// <inheritdoc />
    public Task Handle(SubscriptionActivatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscription {SubscriptionId} activated for customer {CustomerId} with plan {PlanId}",
            domainEvent.SubscriptionId.Value, domainEvent.CustomerId, domainEvent.PlanId.Value);
        return Task.CompletedTask;
    }
}
