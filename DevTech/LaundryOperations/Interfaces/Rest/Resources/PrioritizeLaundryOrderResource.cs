using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Interfaces.Rest.Resources;

/// <summary>
/// Represents the PrioritizeLaundryOrderResource contract.
/// </summary>
public record PrioritizeLaundryOrderResource(EPriority Priority);
