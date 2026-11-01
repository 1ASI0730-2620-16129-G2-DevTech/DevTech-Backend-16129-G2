namespace DevTech.PickupsDeliveries.Application.Errors;

/// <summary>
///     Represents errors that can occur when scheduling a pickup or a delivery.
/// </summary>
public enum SchedulePickupDeliveryError
{
    /// <summary>
    ///     The provided order identifier is zero or negative.
    /// </summary>
    InvalidOrderId,

    /// <summary>
    ///     The provided address is missing or invalid.
    /// </summary>
    InvalidAddress,

    /// <summary>
    ///     The provided service type is not Pickup or Delivery.
    /// </summary>
    InvalidServiceType,

    /// <summary>
    ///     A pickup or delivery of the same type already exists for the order.
    /// </summary>
    DuplicatePickupDelivery,

    /// <summary>
    ///     A delivery was requested but the order is not ready yet.
    /// </summary>
    OrderNotReady,

    /// <summary>
    ///     An unexpected error occurred during the operation.
    /// </summary>
    UnexpectedError
}