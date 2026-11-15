namespace DevTech.TrackingNotifications.Interfaces.Rest.Resources;

/// <summary>
///     Tracking of an order for the REST API.
/// </summary>
public record TrackingResource(
    int Id,
    Guid OrderId,
    string CurrentStage,
    DateTimeOffset? EstimatedDelivery,
    IEnumerable<TrackingStageRecordResource> History);

/// <summary>
///     A stage of the history of a tracking.
/// </summary>
public record TrackingStageRecordResource(string Stage, DateTimeOffset OccurredAt);
