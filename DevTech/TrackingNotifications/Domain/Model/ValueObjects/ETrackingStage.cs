namespace DevTech.TrackingNotifications.Domain.Model.ValueObjects;

/// <summary>
///     Stages of an order as the customer follows them, in the order they occur.
/// </summary>
/// <remarks>
///     It is the language of Tracking &amp; Notifications; Laundry Operations has its own stages
///     (<c>EProcessingStage</c>), translated when its <c>StageAdvanced</c> event is received.
/// </remarks>
public enum ETrackingStage
{
    RECEIVED,
    SORTING,
    WASHING,
    DRYING_IRONING,
    PACKAGING,
    READY
}
