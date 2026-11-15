using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Interfaces.Rest.Resources;

/// <summary>
/// Represents the WashingCycleResource contract.
/// </summary>
public record WashingCycleResource(Guid Id, string Name, int DurationMinutes, string CompatibleType);
