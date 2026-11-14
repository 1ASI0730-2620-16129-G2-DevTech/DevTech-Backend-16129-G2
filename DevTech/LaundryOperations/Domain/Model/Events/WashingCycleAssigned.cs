using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.LaundryOperations.Domain.Model.Events;

/// <summary>
/// Represents the WashingCycleAssigned contract.
/// </summary>
public record WashingCycleAssigned(Guid LaundryOrderId, Guid WashingCycleId) : IEvent;
