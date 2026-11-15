using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Interfaces.Rest.Resources;

/// <summary>
/// Represents the LaundryOrderResource contract.
/// </summary>
public record LaundryOrderResource(Guid Id, Guid OrderId, Guid LaundryId, EProcessingStage CurrentStage,
    EPriority Priority, Guid? WashingCycleId, Guid? ResourceId, DateTimeOffset? ExpectedCompletion, bool IsReady, bool IsAtRisk);
