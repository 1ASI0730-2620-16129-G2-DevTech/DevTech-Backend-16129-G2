using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.TrackingNotifications.Domain.Model.ValueObjects;

namespace DevTech.TrackingNotifications.Application.Internal.EventHandlers;

/// <summary>
///     Translates the processing stages of Laundry Operations into the tracking stages of this context,
///     so the language of Laundry Operations does not leak into Tracking &amp; Notifications.
/// </summary>
internal static class ProcessingStageTranslator
{
    public static ETrackingStage ToTrackingStage(EProcessingStage stage) => stage switch
    {
        EProcessingStage.RECEPTION => ETrackingStage.RECEIVED,
        EProcessingStage.CLASSIFICATION => ETrackingStage.SORTING,
        EProcessingStage.WASHING => ETrackingStage.WASHING,
        EProcessingStage.DRYING_IRONING => ETrackingStage.DRYING_IRONING,
        EProcessingStage.PACKAGING => ETrackingStage.PACKAGING,
        EProcessingStage.READY => ETrackingStage.READY,
        _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, "Unknown processing stage.")
    };
}
