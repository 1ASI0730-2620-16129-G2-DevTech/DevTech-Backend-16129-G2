using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.LaundryOperations.Domain.Model.Events;

/// <summary>
/// Represents the LaundryOrderReceived contract.
/// </summary>
public record LaundryOrderReceived(Guid LaundryOrderId, Guid OrderId) : IEvent;
