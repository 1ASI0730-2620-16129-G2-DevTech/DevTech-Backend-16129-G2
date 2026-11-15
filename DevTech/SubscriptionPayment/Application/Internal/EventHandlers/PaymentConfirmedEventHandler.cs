using DevTech.SubscriptionPayment.Domain.Model.Events;
using DevTech.Shared.Application.Internal.EventHandlers;

namespace DevTech.SubscriptionPayment.Application.Internal.EventHandlers;

/// <summary>
///     Event handler for <see cref="PaymentConfirmedEvent" />.
/// </summary>
/// <remarks>
///     Order Management can subscribe to this event to continue the order flow once the payment is confirmed.
/// </remarks>
public class PaymentConfirmedEventHandler(ILogger<PaymentConfirmedEventHandler> logger)
    : IEventHandler<PaymentConfirmedEvent>
{
    /// <inheritdoc />
    public Task Handle(PaymentConfirmedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Payment {PaymentId} confirmed for order {OrderId} (customer {CustomerId})",
            domainEvent.PaymentId.Value, domainEvent.OrderId, domainEvent.CustomerId);
        return Task.CompletedTask;
    }
}
