using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.SubscriptionPayment.Domain.Repositories;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevTech.SubscriptionPayment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Represents the subscription repository in the Subscription &amp; Payment bounded context.
/// </summary>
/// <param name="context">The <see cref="AppDbContext" /> to use.</param>
public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    // <inheritdoc />
    public async Task<Subscription?> FindActiveByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
    {
        return await Context.Set<Subscription>()
            .Where(subscription => subscription.CustomerId == customerId &&
                                   subscription.Status == ESubscriptionStatus.ACTIVE)
            .OrderByDescending(subscription => subscription.StartDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    // <inheritdoc />
    public async Task<Subscription?> FindLatestByCustomerIdAsNoTrackingAsync(int customerId,
        CancellationToken cancellationToken)
    {
        return await Context.Set<Subscription>()
            .AsNoTracking()
            .Where(subscription => subscription.CustomerId == customerId)
            .OrderByDescending(subscription => subscription.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    // <inheritdoc />
    public new async Task<Subscription?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var subscriptionId = new SubscriptionId(id);
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(subscription => subscription.Id == subscriptionId, cancellationToken);
    }

    // <inheritdoc />
    public new async Task<Subscription?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var subscriptionId = new SubscriptionId(id);
        return await Context.Set<Subscription>()
            .AsNoTracking()
            .FirstOrDefaultAsync(subscription => subscription.Id == subscriptionId, cancellationToken);
    }
}
