using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using DevTech.TrackingNotifications.Domain.Model.Aggregates;
using DevTech.TrackingNotifications.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevTech.TrackingNotifications.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Repository of the order trackings, always loading their stage history.
/// </summary>
public class OrderTrackingRepository(AppDbContext context)
    : BaseRepository<OrderTracking>(context), IOrderTrackingRepository
{
    /// <inheritdoc />
    public async Task<OrderTracking?> FindByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<OrderTracking>()
            .Include(tracking => tracking.History)
            .FirstOrDefaultAsync(tracking => tracking.OrderId == orderId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<OrderTracking?> FindByIdWithHistoryAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<OrderTracking>()
            .AsNoTracking()
            .Include(tracking => tracking.History)
            .FirstOrDefaultAsync(tracking => tracking.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<OrderTracking>> ListByOrderAsync(Guid? orderId,
        CancellationToken cancellationToken = default)
    {
        var trackings = Context.Set<OrderTracking>().AsNoTracking().Include(tracking => tracking.History).AsQueryable();
        if (orderId is not null) trackings = trackings.Where(tracking => tracking.OrderId == orderId);
        return await trackings.OrderByDescending(tracking => tracking.Id).ToListAsync(cancellationToken);
    }
}
