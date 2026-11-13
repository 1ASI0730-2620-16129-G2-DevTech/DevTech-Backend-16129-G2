using DevTech.PickupsDeliveries.Application.Errors;
using DevTech.PickupsDeliveries.Application.Outbound;
using DevTech.PickupsDeliveries.Application.Services;
using DevTech.PickupsDeliveries.Domain.Model.Aggregates;
using DevTech.PickupsDeliveries.Domain.Model.Commands;
using DevTech.PickupsDeliveries.Domain.Model.ValueObjects;
using DevTech.PickupsDeliveries.Domain.Repositories;
using DevTech.Shared.Application.Patterns;
using DevTech.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevTech.PickupsDeliveries.Application.Internal.CommandServices;

/// <summary>
///     Command service for pickup and delivery operations.
/// </summary>
/// <remarks>
///     It checks the business rules in this order:
///     <list type="number">
///         <item><description>The order identifier is not empty.</description></item>
///         <item><description>The address is present.</description></item>
///         <item><description>The service type is Pickup or Delivery.</description></item>
///         <item><description>The order does not already have a service of the same type.</description></item>
///         <item><description>The order exists in Order Management.</description></item>
///         <item><description>For a delivery, the order was created with the home delivery method.</description></item>
///         <item><description>For a delivery, the order is ready.</description></item>
///     </list>
/// </remarks>
/// <param name="pickupDeliveryRepository">The pickup and delivery repository</param>
/// <param name="orderStatusFacade">The outbound port to ask Order Management about the order</param>
/// <param name="unitOfWork">The unit of work to save the changes</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
public class PickupDeliveryCommandService(
    IPickupDeliveryRepository pickupDeliveryRepository,
    IOrderStatusFacade orderStatusFacade,
    IUnitOfWork unitOfWork,
    ILogger<PickupDeliveryCommandService> logger
    ) : IPickupDeliveryCommandService
{
    /// <inheritdoc />
    public async Task<Result<PickupDelivery, SchedulePickupDeliveryError>> Handle(
        SchedulePickupDeliveryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.OrderId == Guid.Empty)
        {
            logger.LogWarning("Empty order identifier when scheduling a pickup or delivery");
            return new Result<PickupDelivery, SchedulePickupDeliveryError>.Failure(
                SchedulePickupDeliveryError.InvalidOrderId);
        }

        if (command.Address is null)
        {
            logger.LogWarning("Missing address when scheduling a pickup or delivery for OrderId: {OrderId}",
                command.OrderId);
            return new Result<PickupDelivery, SchedulePickupDeliveryError>.Failure(
                SchedulePickupDeliveryError.InvalidAddress);
        }

        if (!Enum.IsDefined(command.Type))
        {
            logger.LogWarning("Invalid service type {Type} when scheduling for OrderId: {OrderId}",
                command.Type, command.OrderId);
            return new Result<PickupDelivery, SchedulePickupDeliveryError>.Failure(
                SchedulePickupDeliveryError.InvalidServiceType);
        }

        var existingPickupDelivery = await pickupDeliveryRepository.FindByOrderIdAndTypeAsync(
            command.OrderId, command.Type, cancellationToken);
        if (existingPickupDelivery is not null)
        {
            logger.LogWarning("Duplicate {Type} scheduling attempt for OrderId: {OrderId}",
                command.Type, command.OrderId);
            return new Result<PickupDelivery, SchedulePickupDeliveryError>.Failure(
                SchedulePickupDeliveryError.DuplicatePickupDelivery);
        }

        try
        {
            var orderStatus = await orderStatusFacade.FetchOrderStatusAsync(command.OrderId, cancellationToken);
            if (orderStatus is null)
            {
                logger.LogWarning("Scheduling attempt for an order that does not exist. OrderId: {OrderId}",
                    command.OrderId);
                return new Result<PickupDelivery, SchedulePickupDeliveryError>.Failure(
                    SchedulePickupDeliveryError.OrderNotFound);
            }

            if (command.Type == ServiceType.Delivery)
            {
                if (!orderStatus.IsForHomeDelivery)
                {
                    logger.LogWarning(
                        "Delivery scheduling attempt for an order without home delivery. OrderId: {OrderId}",
                        command.OrderId);
                    return new Result<PickupDelivery, SchedulePickupDeliveryError>.Failure(
                        SchedulePickupDeliveryError.OrderDeliveryMethodMismatch);
                }

                if (!orderStatus.IsReady)
                {
                    logger.LogWarning("Delivery scheduling attempt for an order that is not ready. OrderId: {OrderId}",
                        command.OrderId);
                    return new Result<PickupDelivery, SchedulePickupDeliveryError>.Failure(
                        SchedulePickupDeliveryError.OrderNotReady);
                }
            }

            var pickupDelivery = new PickupDelivery(command);
            await pickupDeliveryRepository.AddAsync(pickupDelivery, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<PickupDelivery, SchedulePickupDeliveryError>.Success(pickupDelivery);
        }
        catch (ArgumentException exception)
        {
            logger.LogWarning(exception,
                "Invalid argument when scheduling {Type} for OrderId: {OrderId}", command.Type, command.OrderId);
            return new Result<PickupDelivery, SchedulePickupDeliveryError>.Failure(
                SchedulePickupDeliveryError.UnexpectedError);
        }
        catch (DbUpdateException exception)
        {
            logger.LogWarning(exception,
                "Database update failed scheduling {Type} for OrderId: {OrderId}", command.Type, command.OrderId);
            return new Result<PickupDelivery, SchedulePickupDeliveryError>.Failure(
                SchedulePickupDeliveryError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                "Unexpected error scheduling {Type} for OrderId: {OrderId}", command.Type, command.OrderId);
            return new Result<PickupDelivery, SchedulePickupDeliveryError>.Failure(
                SchedulePickupDeliveryError.UnexpectedError);
        }
    }
}
