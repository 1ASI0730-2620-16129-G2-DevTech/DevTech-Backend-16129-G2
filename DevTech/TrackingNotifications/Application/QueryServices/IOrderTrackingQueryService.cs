using DevTech.TrackingNotifications.Domain.Model.Aggregates;
using DevTech.TrackingNotifications.Domain.Model.Queries;

namespace DevTech.TrackingNotifications.Application.QueryServices;

/// <summary>
///     Queries over the trackings of the orders.
/// </summary>
public interface IOrderTrackingQueryService
{
    Task<IEnumerable<OrderTracking>> Handle(GetAllTrackingsQuery query, CancellationToken cancellationToken = default);

    Task<OrderTracking?> Handle(GetTrackingByIdQuery query, CancellationToken cancellationToken = default);
}
