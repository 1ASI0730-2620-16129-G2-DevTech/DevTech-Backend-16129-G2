using DevTech.PickupsDeliveries.Application.Errors;
using DevTech.PickupsDeliveries.Domain.Model.Aggregates;
using DevTech.PickupsDeliveries.Domain.Model.Commands;
using DevTech.Shared.Application.Patterns;

namespace DevTech.PickupsDeliveries.Application.Services;

/// <summary>
///     Command service interface for pickup and delivery operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the schedule pickup or delivery command.
///     It ensures the business rules are checked before persisting a new pickup or delivery.
/// </remarks>
public interface IPickupDeliveryCommandService
{
    /// <summary>
    ///     Handle the schedule pickup or delivery command.
    /// </summary>
    /// <remarks>
    ///     This method validates the command, checks that the order exists and does not already have a service
    ///     of the same type and, for a delivery, checks that the order is for home delivery and is ready.
    ///     If every rule is fulfilled, it creates a new pickup or delivery and adds it to the database.
    /// </remarks>
    /// <param name="command">SchedulePickupDeliveryCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the scheduled PickupDelivery on success, or the error reason on failure.
    /// </returns>
    Task<Result<PickupDelivery, SchedulePickupDeliveryError>> Handle(SchedulePickupDeliveryCommand command,
        CancellationToken cancellationToken = default);
}
