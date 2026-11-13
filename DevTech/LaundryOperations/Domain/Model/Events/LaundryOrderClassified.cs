using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.LaundryOperations.Domain.Model.Events;

/// <summary>
/// Represents the LaundryOrderClassified contract.
/// </summary>
public record LaundryOrderClassified(Guid LaundryOrderId, Guid OrderId) : IEvent;
