using DevTech.LaundryOperations.Application.CommandServices;
using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.Errors;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.LaundryOperations.Domain.Repositories;
using DevTech.Shared.Application.Model;
using DevTech.Shared.Domain.Model.Errors;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.LaundryOperations.Application.Internal.CommandServices;

public class LaundryOrderCommandService(ILaundryOrderRepository orders,
    ILaundryResourceRepository resources, IWashingCycleRepository cycles, IUnitOfWork unitOfWork)
    : ILaundryOrderCommandService
{
    public async Task<Result<LaundryOrder>> Handle(ReceiveLaundryOrderCommand command, CancellationToken ct)
    {
        if (command.OrderId == Guid.Empty || command.LaundryId == Guid.Empty || !Enum.IsDefined(command.Priority))
            return Result<LaundryOrder>.Failure(Error.Validation("LaundryOperations.InvalidOrder", "Valid order, laundry and priority are required."));
        if (await orders.FindByOrderIdAsync(command.OrderId, ct) is not null)
            return Result<LaundryOrder>.Failure(LaundryOperationsErrors.OrderAlreadyReceived);
        var order = new LaundryOrder(command);
        await orders.AddAsync(order, ct);
        await unitOfWork.CompleteAsync(ct);
        return Result<LaundryOrder>.Success(order);
    }

    public Task<Result<LaundryOrder>> Handle(PrioritizeLaundryOrderCommand command, CancellationToken ct) =>
        Modify(command.LaundryOrderId, o => o.Prioritize(command.Priority), ct);

    public Task<Result<LaundryOrder>> Handle(ClassifyLaundryOrderCommand command, CancellationToken ct) =>
        Modify(command.LaundryOrderId, o => o.Classify(), ct);

    public async Task<Result<LaundryOrder>> Handle(AssignWashingCycleCommand command, CancellationToken ct)
    {
        if (await cycles.FindByIdAsNoTrackingAsync(command.WashingCycleId, ct) is null)
            return Result<LaundryOrder>.Failure(LaundryOperationsErrors.WashingCycleNotFound);
        return await Modify(command.LaundryOrderId, o => o.AssignWashingCycle(command.WashingCycleId), ct);
    }

    public async Task<Result<LaundryOrder>> Handle(AssignResourceCommand command, CancellationToken ct)
    {
        var order = await orders.FindByIdAsync(command.LaundryOrderId, ct);
        if (order is null) return Result<LaundryOrder>.Failure(LaundryOperationsErrors.LaundryOrderNotFound);
        var resource = await resources.FindByIdAsync(command.ResourceId, ct);
        if (resource is null) return Result<LaundryOrder>.Failure(LaundryOperationsErrors.LaundryResourceNotFound);
        if (resource.LaundryId != order.LaundryId)
            return Result<LaundryOrder>.Failure(Error.Conflict("LaundryOperations.OtherLaundry", "Resource belongs to another laundry."));
        if (!resource.IsAvailable) return Result<LaundryOrder>.Failure(LaundryOperationsErrors.ResourceNotAvailable);
        var error = order.AssignResource(resource.Id);
        if (error != Error.None) return Result<LaundryOrder>.Failure(error);
        resource.Assign(order.Id);
        // Both aggregates are persisted in the same SaveChanges transaction.
        await unitOfWork.CompleteAsync(ct);
        return Result<LaundryOrder>.Success(order);
    }

    public async Task<Result<LaundryOrder>> Handle(AdvanceStageCommand command, CancellationToken ct)
    {
        var order = await orders.FindByIdAsync(command.LaundryOrderId, ct);
        if (order is null) return Result<LaundryOrder>.Failure(LaundryOperationsErrors.LaundryOrderNotFound);
        LaundryResource? resource = null;
        if (order.ResourceId is { } resourceId)
        {
            resource = await resources.FindByIdAsync(resourceId, ct);
            if (resource is null || resource.AssignedOrderId != order.Id.Value)
                return Result<LaundryOrder>.Failure(Error.Conflict("LaundryOperations.InvalidReservation", "The resource reservation is inconsistent."));
            if (order.CurrentStage == EProcessingStage.CLASSIFICATION && resource.Status != EResourceStatus.BUSY)
                return Result<LaundryOrder>.Failure(LaundryOperationsErrors.ResourceNotAvailable);
        }
        var error = order.AdvanceStage();
        if (error != Error.None) return Result<LaundryOrder>.Failure(error);
        if (order.IsReady && resource is not null) resource.CompleteAssignment(order.Id);
        await unitOfWork.CompleteAsync(ct);
        return Result<LaundryOrder>.Success(order);
    }

    private async Task<Result<LaundryOrder>> Modify(LaundryOrderId id, Func<LaundryOrder, Error> action, CancellationToken ct)
    {
        var order = await orders.FindByIdAsync(id, ct);
        if (order is null) return Result<LaundryOrder>.Failure(LaundryOperationsErrors.LaundryOrderNotFound);
        var error = action(order);
        if (error != Error.None) return Result<LaundryOrder>.Failure(error);
        await unitOfWork.CompleteAsync(ct);
        return Result<LaundryOrder>.Success(order);
    }
}

