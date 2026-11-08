using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.Commands;

/// <summary>
///     Command to classify a laundry order.
/// </summary>
/// <param name="LaundryOrderId">
///     The ID of the laundry order to classify.
/// </param>
public record ClassifyLaundryOrderCommand(LaundryOrderId LaundryOrderId)
{
    public ClassifyLaundryOrderCommand(int laundryOrderId)
        : this(new LaundryOrderId(laundryOrderId))
    {
    }
}
