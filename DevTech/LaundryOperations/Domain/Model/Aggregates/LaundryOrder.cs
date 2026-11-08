using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Aggregates.Internal;

namespace DevTech.LaundryOperations.Domain.Model.Aggregates;

/// <summary>
///     Represents the LaundryOrder aggregate root within the Laundry Operations Bounded Context.
/// </summary>
/// <remarks>
///     This class encapsulates the core data and behavior of a Laundry Order,
///     acting as an aggregate root to ensure consistency of its contained entities.
/// </remarks>
public partial class LaundryOrder : AggregateRoot
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LaundryOrder" /> aggregate with its full state.
    /// </summary>
    /// <param name="id">The unique identifier of the laundry order.</param>
    /// <param name="orderId">The identifier of the related order in Order Management.</param>
    /// <param name="currentStage">The current processing stage.</param>
    /// <param name="priority">The priority of the laundry order.</param>
    /// <param name="washingCycleId">The identifier of the assigned washing cycle, if any.</param>
    /// <param name="resourceId">The identifier of the assigned laundry resource, if any.</param>
    /// <param name="expectedCompletion">The expected completion date and time, if any.</param>
    /// <param name="isReady">Whether the laundry order is ready.</param>
    public LaundryOrder(LaundryOrderId id, int orderId, EProcessingStage currentStage, EPriority priority,
        int? washingCycleId, int? resourceId, DateTimeOffset? expectedCompletion, bool isReady) : this()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderId);
        if (washingCycleId is <= 0) throw new ArgumentOutOfRangeException(nameof(washingCycleId));
        if (resourceId is <= 0) throw new ArgumentOutOfRangeException(nameof(resourceId));

        Id = id;
        OrderId = orderId;
        CurrentStage = currentStage;
        Priority = priority;
        WashingCycleId = washingCycleId;
        ResourceId = resourceId;
        ExpectedCompletion = expectedCompletion;
        IsReady = isReady;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LaundryOrder" /> aggregate when an order is received.
    /// </summary>
    /// <param name="orderId">The identifier of the related order in Order Management.</param>
    /// <param name="priority">The priority of the laundry order.</param>
    public LaundryOrder(int orderId, EPriority priority) : this()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderId);
        OrderId = orderId;
        Priority = priority;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LaundryOrder" /> aggregate
    ///     using a <see cref="ReceiveLaundryOrderCommand" />.
    /// </summary>
    /// <param name="command">
    ///     The <see cref="ReceiveLaundryOrderCommand" /> containing the data for receiving the laundry order.
    /// </param>
    public LaundryOrder(ReceiveLaundryOrderCommand command) : this(command.OrderId, command.Priority)
    {
    }

    /// <summary>
    ///     Parameterless constructor used by the persistence mechanism and by the other constructors.
    ///     Move it to your other partial file if you already have one there.
    /// </summary>
    protected LaundryOrder()
    {
        CurrentStage = EProcessingStage.RECEPTION;
        Priority = EPriority.NORMAL;
        IsReady = false;
    }

    /// <summary>
    ///     Gets the unique identifier of the laundry order.
    /// </summary>
    public LaundryOrderId Id { get; }

    /// <summary>
    ///     Gets the identifier of the related order in the Order Management context.
    /// </summary>
    public int OrderId { get; private set; }

    /// <summary>
    ///     Gets the current processing stage of the laundry order.
    /// </summary>
    public EProcessingStage CurrentStage { get; private set; }

    /// <summary>
    ///     Gets the priority of the laundry order.
    /// </summary>
    public EPriority Priority { get; private set; }

    /// <summary>
    ///     Gets the identifier of the assigned washing cycle, if any.
    /// </summary>
    public int? WashingCycleId { get; private set; }

    /// <summary>
    ///     Gets the identifier of the assigned laundry resource, if any.
    /// </summary>
    public int? ResourceId { get; private set; }

    /// <summary>
    ///     Gets the expected completion date and time, if any.
    /// </summary>
    public DateTimeOffset? ExpectedCompletion { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether the laundry order is ready.
    /// </summary>
    public bool IsReady { get; private set; }
}