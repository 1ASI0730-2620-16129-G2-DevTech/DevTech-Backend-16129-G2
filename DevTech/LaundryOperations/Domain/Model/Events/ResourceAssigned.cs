using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.LaundryOperations.Domain.Model.Events;

/// <summary>
/// Represents the ResourceAssigned contract.
/// </summary>
public record ResourceAssigned(Guid LaundryOrderId, Guid ResourceId) : IEvent;
