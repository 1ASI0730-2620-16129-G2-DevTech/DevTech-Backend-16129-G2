using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.LaundryOperations.Domain.Model.Events;

/// <summary>
/// Represents the ResourceReleased contract.
/// </summary>
public record ResourceReleased(Guid ResourceId) : IEvent;
