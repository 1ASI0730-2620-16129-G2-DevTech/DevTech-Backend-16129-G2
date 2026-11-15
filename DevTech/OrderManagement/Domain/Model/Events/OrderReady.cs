using DevTech.Shared.Domain.Model.Events;

namespace DevTech.OrderManagement.Domain.Model.Events;

/// <summary>
///     Raised when the order moves to READY. Subscription &amp; Payment reacts by charging the customer.
/// </summary>
public sealed record OrderReady(Guid OrderId, Guid CustomerId) : IEvent;
