using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.Commands;

/// <summary>
///     Command to assign a laundry resource to a laundry order.
/// </summary>
/// <param name="LaundryOrderId">
///     The ID of the laundry order that receives the resource.
/// </param>
/// <param name="ResourceId">
///     The ID of the laundry resource to assign.
/// </param>
public record AssignResourceCommand(LaundryOrderId LaundryOrderId, LaundryResourceId ResourceId)
{
    public AssignResourceCommand(int laundryOrderId, int resourceId)
        : this(new LaundryOrderId(laundryOrderId), new LaundryResourceId(resourceId))
    {
    }
}
