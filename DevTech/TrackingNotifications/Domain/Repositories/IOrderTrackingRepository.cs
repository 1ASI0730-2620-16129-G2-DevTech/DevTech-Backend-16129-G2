using DevTech.Shared.Domain.Repositories;
using DevTech.TrackingNotifications.Domain.Model.Aggregates;

namespace DevTech.TrackingNotifications.Domain.Repositories;

/// <summary>
///     Repository of the order trackings in the Tracking &amp; Notifications bounded context.
/// </summary>
public interface IOrderTrackingRepository : IBaseRepository<OrderTracking>
{
    /// <summary>
    ///     Finds the tracking of an order, with its history (tracked, to change it).
    /// </summary>
    Task<OrderTracking?> FindByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds a tracking by its id, with its history (read only).
    /// </summary>
    Task<OrderTracking?> FindByIdWithHistoryAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Lists the trackings with their history, newest first, optionally only the one of an order (read only).
    /// </summary>
    Task<IEnumerable<OrderTracking>> ListByOrderAsync(Guid? orderId, CancellationToken cancellationToken = default);
}
