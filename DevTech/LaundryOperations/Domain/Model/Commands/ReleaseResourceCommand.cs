using DevTech.LaundryOperations.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.Commands;

/// <summary>
///     Command to release a laundry resource so it becomes available again.
/// </summary>
/// <param name="ResourceId">
///     The ID of the laundry resource to release.
/// </param>
public record ReleaseResourceCommand(LaundryResourceId ResourceId)
{
    public ReleaseResourceCommand(Guid resourceId)
        : this(new LaundryResourceId(resourceId))
    {
    }
}
