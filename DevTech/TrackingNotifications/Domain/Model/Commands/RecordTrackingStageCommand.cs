using DevTech.TrackingNotifications.Domain.Model.ValueObjects;

namespace DevTech.TrackingNotifications.Domain.Model.Commands;

/// <summary>
///     Records that an order reached a new stage (use case "Registrar cambio de etapa").
/// </summary>
/// <param name="OrderId">Reference (by id) to the order in Order Management.</param>
/// <param name="Stage">The stage the order reached.</param>
/// <param name="OccurredAt">When the order reached it.</param>
public record RecordTrackingStageCommand(Guid OrderId, ETrackingStage Stage, DateTimeOffset OccurredAt);
