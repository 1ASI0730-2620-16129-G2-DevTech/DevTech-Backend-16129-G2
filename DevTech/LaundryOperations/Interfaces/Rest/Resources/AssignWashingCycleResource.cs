using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Interfaces.Rest.Resources;

/// <summary>
/// Represents the AssignWashingCycleResource contract.
/// </summary>
public record AssignWashingCycleResource(Guid WashingCycleId);
