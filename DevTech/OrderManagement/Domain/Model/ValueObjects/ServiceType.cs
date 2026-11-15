namespace DevTech.OrderManagement.Domain.Model.ValueObjects;

/// <summary>
///     Laundry service requested for the whole order.
/// </summary>
public enum ServiceType
{
    WASHING,
    WASHING_DRYING,
    WASHING_IRONING,
    IRONING,
    DRYING,
    DRY_CLEANING
}
