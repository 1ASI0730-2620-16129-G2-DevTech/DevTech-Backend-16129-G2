using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Interfaces.Rest.Resources;

/// <summary>
/// Represents the SynchronizeResourceResource contract.
/// </summary>
public record SynchronizeResourceResource(EResourceStatus Status);
