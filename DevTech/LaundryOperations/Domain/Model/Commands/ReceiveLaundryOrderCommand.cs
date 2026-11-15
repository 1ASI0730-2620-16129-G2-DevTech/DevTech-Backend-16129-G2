using DevTech.LaundryOperations.Domain.Model.ValueObjects;
namespace DevTech.LaundryOperations.Domain.Model.Commands;
public record ReceiveLaundryOrderCommand(Guid OrderId, Guid LaundryId, EPriority Priority,
    DateTimeOffset? ExpectedCompletion = null);

