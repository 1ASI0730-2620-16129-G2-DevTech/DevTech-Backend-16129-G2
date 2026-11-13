namespace DevTech.PickupsDeliveries.Application.Errors;

/// <summary>
///     Represents errors that can occur when scheduling a pickup or a delivery.
/// </summary>
public enum SchedulePickupDeliveryError
{
    /// <summary>
    ///     The provided order identifier is empty.
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
    ///     The order does not exist in the Order Management bounded context.
    /// </summary>
    OrderNotFound,

    /// <summary>
    ///     A delivery was requested but the order was created to be picked up by the customer at the laundry.
    /// </summary>
    OrderDeliveryMethodMismatch,

    /// <summary>
    ///     A delivery was requested but the order is not ready yet.
    /// </summary>
    OrderNotReady,

    /// <summary>
    ///     An unexpected error occurred during the operation.
    /// </summary>
    UnexpectedError
}