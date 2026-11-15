using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.LaundryOperations.Domain.Model.Events;

/// <summary>
/// Represents the OrderReady contract.
/// </summary>
public record OrderReady(Guid LaundryOrderId, Guid OrderId) : IEvent;
