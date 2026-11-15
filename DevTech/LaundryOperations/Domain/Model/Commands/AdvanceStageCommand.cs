using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.Commands;

/// <summary>
///     Command to advance a laundry order to its next processing stage.
/// </summary>
/// <param name="LaundryOrderId">
///     The ID of the laundry order to advance.
/// </param>
public record AdvanceStageCommand(LaundryOrderId LaundryOrderId)
{
    public AdvanceStageCommand(Guid laundryOrderId)
        : this(new LaundryOrderId(laundryOrderId))
    {
    }
}
