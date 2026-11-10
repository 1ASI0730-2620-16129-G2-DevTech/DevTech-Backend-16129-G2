using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.LaundryOperations.Domain.Model.Events;
using DevTech.Shared.Domain.Model.Aggregates.Internal;

namespace DevTech.LaundryOperations.Domain.Model.Aggregates;

public class LaundryResource : AggregateRoot
{
    private LaundryResource() { }
    public LaundryResource(LaundryId laundryId, string name, int capacity)
    {
        if (laundryId.Value == Guid.Empty) throw new ArgumentException("Laundry id is required.");
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);
        Id = new(Guid.NewGuid());
        LaundryId = laundryId;
        Name = name.Trim();
        Capacity = capacity;
    }
    public LaundryResourceId Id { get; private set; }
    public LaundryId LaundryId { get; private set; }
    public string Name { get; private set; } = "";
    public EResourceStatus Status { get; private set; } = EResourceStatus.AVAILABLE;
    public int Capacity { get; private set; }
    public Guid? AssignedOrderId { get; private set; }
    public Guid Version { get; private set; } = Guid.NewGuid();
    public void RefreshVersion() => Version = Guid.NewGuid();
    public bool IsAvailable => Status == EResourceStatus.AVAILABLE && AssignedOrderId is null;

    public bool Assign(LaundryOrderId orderId)
    {
        if (!IsAvailable || orderId.Value == Guid.Empty) return false;
        AssignedOrderId = orderId.Value;
        Status = EResourceStatus.BUSY;
        return true;
    }
    // Manual release must not detach a resource from an unfinished order.
    public bool Release()
    {
        if (Status != EResourceStatus.BUSY || AssignedOrderId is not null) return false;
        Status = EResourceStatus.AVAILABLE;
        AddDomainEvent(new ResourceReleased(Id.Value));
        return true;
    }
    public bool CompleteAssignment(LaundryOrderId orderId)
    {
        if (AssignedOrderId != orderId.Value) return false;
        AssignedOrderId = null;
        // Finishing an order never clears a maintenance condition.
        if (Status == EResourceStatus.BUSY) Status = EResourceStatus.AVAILABLE;
        AddDomainEvent(new ResourceReleased(Id.Value));
        return true;
    }
    public bool SynchronizeStatus(EResourceStatus status)
    {
        if (!Enum.IsDefined(status)) return false;
        if (status == EResourceStatus.AVAILABLE && (Status == EResourceStatus.BUSY || AssignedOrderId is not null))
            return false;
        Status = status;
        AddDomainEvent(new ResourceStatusChanged(Id.Value, status));
        return true;
    }
}

