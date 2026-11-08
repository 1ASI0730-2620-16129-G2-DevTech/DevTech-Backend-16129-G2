using DevTech.Shared.Domain.Model.Errors;

namespace DevTech.LaundryOperations.Domain.Model.Errors;

public static class LaundryOperationsErrors
{
    public static readonly Error LaundryOrderCreationFailed =
        Error.Failure("LaundryOperations.LaundryOrderCreationFailed",
            "An error occurred while creating the laundry order.");

    public static readonly Error LaundryOrderNotFound =
        Error.NotFound("LaundryOperations.LaundryOrderNotFound", "The specified laundry order was not found.");

    public static readonly Error LaundryResourceNotFound =
        Error.NotFound("LaundryOperations.LaundryResourceNotFound", "The specified laundry resource was not found.");

    public static readonly Error WashingCycleNotFound =
        Error.NotFound("LaundryOperations.WashingCycleNotFound", "The specified washing cycle was not found.");

    public static readonly Error OrderAlreadyReceived =
        Error.Conflict("LaundryOperations.OrderAlreadyReceived",
            "A laundry order already exists for the specified order.");

    public static readonly Error OrderAlreadyReady =
        Error.Conflict("LaundryOperations.OrderAlreadyReady",
            "The laundry order is already ready and cannot be modified.");

    public static readonly Error WashingCycleAndResourceRequired =
        Error.Validation("LaundryOperations.WashingCycleAndResourceRequired",
            "A washing cycle and a laundry resource must be assigned before washing can start.");

    public static readonly Error ResourceNotAvailable =
        Error.Conflict("LaundryOperations.ResourceNotAvailable", "The laundry resource is not available.");

    public static readonly Error ResourceNotBusy =
        Error.Conflict("LaundryOperations.ResourceNotBusy",
            "The laundry resource cannot be released because it is not busy.");

    public static readonly Error InvalidStageTransition =
        Error.Conflict("LaundryOperations.InvalidStageTransition",
            "The laundry order cannot move to the requested processing stage.");

    public static readonly Error InvalidResourceStatus =
        Error.Validation("LaundryOperations.InvalidResourceStatus",
            "The monitored status received for the laundry resource is not valid.");

    public static Error OrderAlreadyReceivedWithOrderId(int orderId) =>
        Error.Conflict("LaundryOperations.OrderAlreadyReceived",
            $"A laundry order already exists for the order with id '{orderId}'.");
}