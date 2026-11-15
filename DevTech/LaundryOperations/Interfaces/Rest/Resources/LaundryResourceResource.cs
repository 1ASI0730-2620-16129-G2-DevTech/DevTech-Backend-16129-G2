using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Interfaces.Rest.Resources;

/// <summary>
/// Represents the LaundryResourceResource contract.
/// </summary>
public record LaundryResourceResource(Guid Id, Guid LaundryId, string Name, EResourceStatus Status, int Capacity, Guid? AssignedOrderId);
