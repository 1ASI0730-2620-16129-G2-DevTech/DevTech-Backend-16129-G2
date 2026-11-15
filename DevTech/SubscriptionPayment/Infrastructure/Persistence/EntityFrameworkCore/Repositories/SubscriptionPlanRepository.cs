using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.SubscriptionPayment.Domain.Repositories;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevTech.SubscriptionPayment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Represents the subscription plan repository in the Subscription &amp; Payment bounded context.
/// </summary>
/// <param name="context">The <see cref="AppDbContext" /> to use.</param>
public class SubscriptionPlanRepository(AppDbContext context)
    : BaseRepository<SubscriptionPlan>(context), ISubscriptionPlanRepository
{
    // The id is a strongly-typed value object, so the lookups compare against SubscriptionPlanId.

    // <inheritdoc />
    public new async Task<SubscriptionPlan?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var planId = new SubscriptionPlanId(id);
        return await Context.Set<SubscriptionPlan>()
            .FirstOrDefaultAsync(plan => plan.Id == planId, cancellationToken);
    }

    // <inheritdoc />
    public new async Task<SubscriptionPlan?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var planId = new SubscriptionPlanId(id);
        return await Context.Set<SubscriptionPlan>()
            .AsNoTracking()
            .FirstOrDefaultAsync(plan => plan.Id == planId, cancellationToken);
    }
}
