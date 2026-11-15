using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.Commands;

/// <summary>
///     Command to synchronize the status of a laundry resource with the monitored status.
/// </summary>
/// <param name="ResourceId">
///     The ID of the laundry resource to synchronize.
/// </param>
/// <param name="Status">
///     The monitored status to apply to the laundry resource.
/// </param>
public record SyncResourceStatusCommand(LaundryResourceId ResourceId, EResourceStatus Status)
{
    public SyncResourceStatusCommand(Guid resourceId, EResourceStatus status)
        : this(new LaundryResourceId(resourceId), status)
    {
    }
}
