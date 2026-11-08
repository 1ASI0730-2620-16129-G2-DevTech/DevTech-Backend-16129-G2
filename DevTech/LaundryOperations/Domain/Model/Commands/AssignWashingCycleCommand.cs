using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.Commands;

/// <summary>
///     Command to assign a washing cycle to a laundry order.
/// </summary>
/// <param name="LaundryOrderId">
///     The ID of the laundry order that receives the washing cycle.
/// </param>
/// <param name="WashingCycleId">
///     The ID of the washing cycle to assign.
/// </param>
public record AssignWashingCycleCommand(LaundryOrderId LaundryOrderId, WashingCycleId WashingCycleId)
{
    public AssignWashingCycleCommand(int laundryOrderId, int washingCycleId)
        : this(new LaundryOrderId(laundryOrderId), new WashingCycleId(washingCycleId))
    {
    }
}
