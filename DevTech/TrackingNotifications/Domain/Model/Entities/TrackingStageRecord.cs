using DevTech.TrackingNotifications.Domain.Model.ValueObjects;

namespace DevTech.TrackingNotifications.Domain.Model.Entities;

/// <summary>
///     A stage the order reached and when it reached it (status history of an <c>OrderTracking</c>).
/// </summary>
/// <remarks>
///     Child entity of the <c>OrderTracking</c> aggregate: it is only created through the aggregate root.
/// </remarks>
public class TrackingStageRecord
{
    /// <summary>
    ///     Parameterless constructor required by EF Core.
    /// </summary>
    private TrackingStageRecord()
    {
    }

    internal TrackingStageRecord(ETrackingStage stage, DateTimeOffset occurredAt)
    {
        Stage = stage;
        OccurredAt = occurredAt;
    }

    public int Id { get; private set; }
    public int OrderTrackingId { get; private set; }
    public ETrackingStage Stage { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
}
