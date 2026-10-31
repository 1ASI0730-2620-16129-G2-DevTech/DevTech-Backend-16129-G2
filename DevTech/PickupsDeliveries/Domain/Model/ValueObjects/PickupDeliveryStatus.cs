namespace DevTech.PickupsDeliveries.Domain.Model.ValueObjects;

/// <summary>
///     Represents the lifecycle status of a pickup or a delivery.
/// </summary>
/// <remarks>
///     A pickup or delivery always starts as <see cref="Scheduled" />.
///     <see cref="Completed" /> and <see cref="Failed" /> are final statuses:
///     once a pickup or delivery reaches one of them, its status can no longer change.
/// </remarks>
public enum PickupDeliveryStatus
{
    /// <summary>
    ///     The pickup or delivery was requested and is waiting to start.
    /// </summary>
    Scheduled,

    /// <summary>
    ///     The courier is on the way.
    /// </summary>
    InTransit,

    /// <summary>
    ///     The clothes were picked up or handed over to the customer. Final status.
    /// </summary>
    Completed,

    /// <summary>
    ///     The pickup or delivery could not be completed. Final status.
    /// </summary>
    Failed
}