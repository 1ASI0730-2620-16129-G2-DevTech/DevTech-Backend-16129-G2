using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.LaundryOperations.Domain.Model.Events;

/// <summary>
/// Represents the ResourceStatusChanged contract.
/// </summary>
public record ResourceStatusChanged(Guid ResourceId, EResourceStatus Status) : IEvent;
