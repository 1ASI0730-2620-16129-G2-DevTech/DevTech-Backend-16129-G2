using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.Commands;

/// <summary>
///     Command to change the priority of a laundry order.
/// </summary>
/// <param name="LaundryOrderId">
///     The ID of the laundry order to prioritize.
/// </param>
/// <param name="Priority">
///     The new priority of the laundry order.
/// </param>
public record PrioritizeLaundryOrderCommand(LaundryOrderId LaundryOrderId, EPriority Priority)
{
    public PrioritizeLaundryOrderCommand(int laundryOrderId, EPriority priority)
        : this(new LaundryOrderId(laundryOrderId), priority)
    {
    }
}
