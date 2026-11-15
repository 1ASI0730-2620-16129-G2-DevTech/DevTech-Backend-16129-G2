using DevTech.Shared.Application.Model;
using DevTech.OrderManagement.Application.Errors;
using DevTech.OrderManagement.Application.Services;
using DevTech.OrderManagement.Domain.Model.Aggregates;
using DevTech.OrderManagement.Domain.Model.Commands;
using DevTech.OrderManagement.Domain.Model.Exceptions;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.OrderManagement.Domain.Repositories;
using DevTech.Shared.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace DevTech.OrderManagement.Application.Internal.CommandServices;

/// <summary>
///     Orchestrates the write use cases of the <see cref="Order" /> aggregate.
/// </summary>
/// <remarks>
///     Orders are loaded tracked, so the change tracker detects every modification (including added or removed
///     garment items). That is why <c>Update</c> is intentionally never called on an existing order: on an aggregate
///     with client-generated ids it would flag newly added children as modified instead of added.
/// </remarks>
public class OrderCommandService(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    ILogger<OrderCommandService> logger) : IOrderCommandService
{
    /// <inheritdoc />
    public async Task<Result<Order>> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = new Order(command);
            await orderRepository.AddAsync(order, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Order>.Success(order);
        }
        catch (OrderDomainException ex)
        {
            return Result<Order>.Failure(ToOrderError(ex.Rule), ex.Message);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return UnexpectedFailure(ex, "creating an order");
        }
    }

    /// <inheritdoc />
    public Task<Result<Order>> Handle(AddGarmentItemCommand command, CancellationToken cancellationToken = default)
    {
        return ExecuteOnOrderAsync(command.OrderId, order => order.AddGarmentItem(command), cancellationToken);
    }

    /// <inheritdoc />
    public Task<Result<Order>> Handle(RemoveGarmentItemCommand command, CancellationToken cancellationToken = default)
    {
        return ExecuteOnOrderAsync(command.OrderId, order => order.RemoveGarmentItem(command.GarmentItemId),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<Result<Order>> Handle(ChangeOrderStatusCommand command, CancellationToken cancellationToken = default)
    {
        return ExecuteOnOrderAsync(command.OrderId, order => order.AdvanceStatus(command.NewStatus),
            cancellationToken);
    }

    /// <summary>
    ///     Loads the order, applies a change to it, persists it and maps every failure to an <see cref="OrderError" />.
    /// </summary>
    private async Task<Result<Order>> ExecuteOnOrderAsync(
        OrderId orderId,
        Action<Order> change,
        CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.FindByIdAsync(orderId, cancellationToken);
            if (order is null)
                return Result<Order>.Failure(OrderError.OrderNotFound, $"The order {orderId.Value} was not found.");

            change(order);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Order>.Success(order);
        }
        catch (OrderDomainException ex)
        {
            return Result<Order>.Failure(ToOrderError(ex.Rule), ex.Message);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return UnexpectedFailure(ex, $"updating the order {orderId.Value}");
        }
    }

    private Result<Order> UnexpectedFailure(Exception ex, string operation)
    {
        logger.LogError(ex, "Unexpected error while {Operation}.", operation);
        return Result<Order>.Failure(OrderError.UnexpectedError, $"An unexpected error occurred while {operation}.");
    }

    private static OrderError ToOrderError(OrderRule rule)
    {
        return rule switch
        {
            OrderRule.InvalidCustomer => OrderError.InvalidCustomer,
            OrderRule.InvalidLaundry => OrderError.InvalidLaundry,
            OrderRule.InvalidEstimatedDeliveryDate => OrderError.InvalidEstimatedDeliveryDate,
            OrderRule.InvalidSpecialCareInstructions => OrderError.InvalidSpecialCareInstructions,
            OrderRule.InvalidGarmentItem => OrderError.InvalidGarmentItem,
            OrderRule.GarmentItemsLocked => OrderError.GarmentItemsLockedAfterCreation,
            OrderRule.InvalidStatusTransition => OrderError.InvalidStatusTransition,
            OrderRule.GarmentItemNotFound => OrderError.GarmentItemNotFound,
            _ => OrderError.UnexpectedError
        };
    }
}
