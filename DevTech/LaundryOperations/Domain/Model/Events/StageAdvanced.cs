using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.LaundryOperations.Domain.Model.Events;

/// <summary>
/// Represents the StageAdvanced contract.
/// </summary>
public record StageAdvanced(Guid LaundryOrderId, Guid OrderId, EProcessingStage Stage) : IEvent;
