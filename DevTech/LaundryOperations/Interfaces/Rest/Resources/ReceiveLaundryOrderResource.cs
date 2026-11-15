using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Interfaces.Rest.Resources;

/// <summary>
/// Represents the ReceiveLaundryOrderResource contract.
/// </summary>
public record ReceiveLaundryOrderResource(Guid OrderId, Guid LaundryId, EPriority Priority, DateTimeOffset? ExpectedCompletion);
