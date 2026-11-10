using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.LaundryOperations.Domain.Model.Errors;
using DevTech.LaundryOperations.Domain.Model.Events;
using DevTech.Shared.Domain.Model.Aggregates.Internal;
using DevTech.Shared.Domain.Model.Errors;

namespace DevTech.LaundryOperations.Domain.Model.Aggregates;

public class LaundryOrder : AggregateRoot
{
    private LaundryOrder() { }
    public LaundryOrder(ReceiveLaundryOrderCommand command)
    {
        if (command.OrderId == Guid.Empty || command.LaundryId == Guid.Empty)
            throw new ArgumentException("OrderId and LaundryId must be non-empty.");
        if (!Enum.IsDefined(command.Priority)) throw new ArgumentException("Invalid priority.");
        Id = new(Guid.NewGuid());
        OrderId = command.OrderId;
        LaundryId = new(command.LaundryId);
        Priority = command.Priority;
        ExpectedCompletion = command.ExpectedCompletion?.ToUniversalTime();
        AddDomainEvent(new LaundryOrderReceived(Id.Value, OrderId));
    }

    public LaundryOrderId Id { get; private set; }
    public Guid OrderId { get; private set; }
    public LaundryId LaundryId { get; private set; }
    public EProcessingStage CurrentStage { get; private set; } = EProcessingStage.RECEPTION;
    public EPriority Priority { get; private set; }
    public WashingCycleId? WashingCycleId { get; private set; }
    public LaundryResourceId? ResourceId { get; private set; }
    public DateTimeOffset? ExpectedCompletion { get; private set; }
    public bool IsReady => CurrentStage == EProcessingStage.READY;
    public Guid Version { get; private set; } = Guid.NewGuid();
    public void RefreshVersion() => Version = Guid.NewGuid();
    public bool IsAtRisk(DateTimeOffset now) =>
        Priority == EPriority.VIP && !IsReady && ExpectedCompletion is { } due && due < now;

    public Error Classify()
    {
        if (CurrentStage != EProcessingStage.RECEPTION) return LaundryOperationsErrors.InvalidStageTransition;
        CurrentStage = EProcessingStage.CLASSIFICATION;
        AddDomainEvent(new LaundryOrderClassified(Id.Value, OrderId));
        AddDomainEvent(new StageAdvanced(Id.Value, OrderId, CurrentStage));
        return Error.None;
    }

    public Error Prioritize(EPriority priority)
    {
        if (!Enum.IsDefined(priority)) return Error.Validation("LaundryOperations.InvalidPriority", "Invalid priority.");
        if (IsReady) return LaundryOperationsErrors.OrderAlreadyReady;
        Priority = priority;
        AddDomainEvent(new OrderPrioritized(Id.Value, priority));
        return Error.None;
    }

    public Error AssignWashingCycle(WashingCycleId id)
    {
        if (id.Value == Guid.Empty) return Error.Validation("LaundryOperations.InvalidCycle", "Cycle id is required.");
        if (CurrentStage != EProcessingStage.CLASSIFICATION) return LaundryOperationsErrors.InvalidStageTransition;
        WashingCycleId = id;
        AddDomainEvent(new WashingCycleAssigned(Id.Value, id.Value));
        return Error.None;
    }

    public Error AssignResource(LaundryResourceId id)
    {
        if (id.Value == Guid.Empty) return Error.Validation("LaundryOperations.InvalidResource", "Resource id is required.");
        if (CurrentStage != EProcessingStage.CLASSIFICATION) return LaundryOperationsErrors.InvalidStageTransition;
        if (ResourceId is not null) return Error.Conflict("LaundryOperations.ResourceAlreadyAssigned", "A resource is already assigned.");
        ResourceId = id;
        AddDomainEvent(new ResourceAssigned(Id.Value, id.Value));
        return Error.None;
    }

    public Error AdvanceStage()
    {
        if (CurrentStage == EProcessingStage.RECEPTION) return Classify();
        if (IsReady) return LaundryOperationsErrors.OrderAlreadyReady;
        if (CurrentStage == EProcessingStage.CLASSIFICATION && (WashingCycleId is null || ResourceId is null))
            return LaundryOperationsErrors.WashingCycleAndResourceRequired;
        CurrentStage = CurrentStage switch
        {
            EProcessingStage.CLASSIFICATION => EProcessingStage.WASHING,
            EProcessingStage.WASHING => EProcessingStage.DRYING_IRONING,
            EProcessingStage.DRYING_IRONING => EProcessingStage.PACKAGING,
            EProcessingStage.PACKAGING => EProcessingStage.READY,
            _ => throw new InvalidOperationException("Unknown persisted stage.")
        };
        AddDomainEvent(new StageAdvanced(Id.Value, OrderId, CurrentStage));
        if (IsReady) AddDomainEvent(new OrderReady(Id.Value, OrderId));
        return Error.None;
    }
}

