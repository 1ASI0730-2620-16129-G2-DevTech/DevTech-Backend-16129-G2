using DevTech.Shared.Domain.Model.Aggregates.Internal;
using DevTech.TrackingNotifications.Domain.Model.Commands;
using DevTech.TrackingNotifications.Domain.Model.Entities;
using DevTech.TrackingNotifications.Domain.Model.Events;
using DevTech.TrackingNotifications.Domain.Model.ValueObjects;

namespace DevTech.TrackingNotifications.Domain.Model.Aggregates;

/// <summary>
///     Tracking of an order: the stage it is in and the history of the stages it went through.
/// </summary>
/// <remarks>
///     References the order of Order Management by id only. There is one tracking per order.
/// </remarks>
public class OrderTracking : AggregateRoot
{
    private readonly List<TrackingStageRecord> _history = [];

    /// <summary>
    ///     Parameterless constructor required by EF Core.
    /// </summary>
    private OrderTracking()
    {
    }

    /// <summary>
    ///     Starts the tracking of an order at the RECEIVED stage.
    /// </summary>
    /// <exception cref="ArgumentException">When the order identifier is empty.</exception>
    public OrderTracking(StartTrackingCommand command, DateTimeOffset startedAt)
    {
        if (command.OrderId == Guid.Empty)
            throw new ArgumentException("The order identifier is required.", nameof(command));

        OrderId = command.OrderId;
        EstimatedDelivery = command.EstimatedDelivery;
        CurrentStage = ETrackingStage.RECEIVED;
        _history.Add(new TrackingStageRecord(ETrackingStage.RECEIVED, startedAt));
    }

    public int Id { get; private set; }
    public Guid OrderId { get; private set; }
    public ETrackingStage CurrentStage { get; private set; }
    public DateTimeOffset? EstimatedDelivery { get; private set; }
    public IReadOnlyCollection<TrackingStageRecord> History => _history.AsReadOnly();

    /// <summary>
    ///     Records that the order reached a later stage.
    /// </summary>
    /// <remarks>
    ///     The tracking only moves forward: a stage that is not after the current one (for example the same
    ///     event received twice) is ignored, so recording a stage is idempotent.
    /// </remarks>
    /// <returns>True when the stage was recorded.</returns>
    public bool RecordStage(ETrackingStage stage, DateTimeOffset occurredAt)
    {
        if (!Enum.IsDefined(stage) || stage <= CurrentStage) return false;

        CurrentStage = stage;
        _history.Add(new TrackingStageRecord(stage, occurredAt));
        AddDomainEvent(new TrackingStageRecorded(Id, OrderId, stage));
        return true;
    }

    /// <summary>
    ///     Keeps the date promised to the customer, when it is known.
    /// </summary>
    public void UpdateEstimatedDelivery(DateTimeOffset? estimatedDelivery)
    {
        if (estimatedDelivery is not null) EstimatedDelivery = estimatedDelivery;
    }
}
