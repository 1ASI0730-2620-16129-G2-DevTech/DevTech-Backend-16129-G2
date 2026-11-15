using DevTech.Shared.Domain.Model.Events;

namespace DevTech.OrderManagement.Domain.Model.Events;

/// <summary>
///     Raised when the order moves to RECEIVED. Laundry Operations reacts by creating its LaundryOrder.
/// </summary>
public sealed record OrderReceived(Guid OrderId) : IEvent;
