namespace DevTech.PickupsDeliveries.Domain.Model.ValueObjects;

/// <summary>
///     Represents the type of logistic service requested for a laundry order.
/// </summary>
public enum ServiceType
{
    /// <summary>
    ///     The laundry picks up the dirty clothes at the customer's address.
    /// </summary>
    Pickup,

    /// <summary>
    ///     The laundry delivers the clean clothes to the customer's address.
    /// </summary>
    Delivery
}