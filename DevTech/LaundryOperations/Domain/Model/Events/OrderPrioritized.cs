using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.LaundryOperations.Domain.Model.Events;

/// <summary>
/// Represents the OrderPrioritized contract.
/// </summary>
public record OrderPrioritized(Guid LaundryOrderId, EPriority Priority) : IEvent;
