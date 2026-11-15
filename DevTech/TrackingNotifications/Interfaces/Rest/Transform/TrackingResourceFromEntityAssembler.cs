using DevTech.TrackingNotifications.Domain.Model.Aggregates;
using DevTech.TrackingNotifications.Interfaces.Rest.Resources;

namespace DevTech.TrackingNotifications.Interfaces.Rest.Transform;

/// <summary>
///     Transforms an <see cref="OrderTracking" /> into a <see cref="TrackingResource" />.
/// </summary>
public static class TrackingResourceFromEntityAssembler
{
    public static TrackingResource ToResourceFromEntity(OrderTracking entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var history = entity.History
            .OrderBy(record => record.OccurredAt)
            .Select(record => new TrackingStageRecordResource(record.Stage.ToString(), record.OccurredAt));
        return new TrackingResource(entity.Id, entity.OrderId, entity.CurrentStage.ToString(),
            entity.EstimatedDelivery, history);
    }
}
