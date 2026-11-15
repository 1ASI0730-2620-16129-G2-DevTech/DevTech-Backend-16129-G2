namespace DevTech.OrderManagement.Application.Errors;

/// <summary>
///     Errors that the Order Management application layer can report through <c>Result&lt;Order&gt;</c>.
/// </summary>
/// <remarks>
///     Every member has a matching localized title under the key <c>OrderError.&lt;Member&gt;</c>
///     in <c>ErrorMessages.resx</c> / <c>ErrorMessages.es.resx</c>.
/// </remarks>
public enum OrderError
{
    InvalidCustomer,
    InvalidLaundry,
    InvalidEstimatedDeliveryDate,
    InvalidSpecialCareInstructions,
    InvalidGarmentItem,
    GarmentItemsLockedAfterCreation,
    InvalidStatusTransition,
    OrderNotFound,
    GarmentItemNotFound,
    UnexpectedError
}
