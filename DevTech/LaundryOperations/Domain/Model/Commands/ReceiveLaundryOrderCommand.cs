using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.Commands;

/// <summary>
///     Command to receive a laundry order coming from the Order Management context.
/// </summary>
/// <param name="OrderId">
///     The ID of the related order in the Order Management context.
/// </param>
/// <param name="Priority">
///     The priority assigned to the laundry order.
/// </param>
public record ReceiveLaundryOrderCommand(int OrderId, EPriority Priority);
