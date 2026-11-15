using DevTech.TrackingNotifications.Application.QueryServices;
using DevTech.TrackingNotifications.Domain.Model.Aggregates;
using DevTech.TrackingNotifications.Domain.Model.Queries;
using DevTech.TrackingNotifications.Domain.Repositories;

namespace DevTech.TrackingNotifications.Application.Internal.QueryServices;

/// <summary>
///     Order tracking query service.
/// </summary>
public class OrderTrackingQueryService(IOrderTrackingRepository orderTrackingRepository) : IOrderTrackingQueryService
{
    /// <inheritdoc />
    public Task<IEnumerable<OrderTracking>> Handle(GetAllTrackingsQuery query,
        CancellationToken cancellationToken = default)
    {
        return orderTrackingRepository.ListByOrderAsync(query.OrderId, cancellationToken);
    }

    /// <inheritdoc />
    public Task<OrderTracking?> Handle(GetTrackingByIdQuery query, CancellationToken cancellationToken = default)
    {
        return orderTrackingRepository.FindByIdWithHistoryAsync(query.TrackingId, cancellationToken);
    }
}
