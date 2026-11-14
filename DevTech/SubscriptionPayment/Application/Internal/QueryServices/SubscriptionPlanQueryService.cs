using DevTech.SubscriptionPayment.Application.QueryServices;
using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Queries;
using DevTech.SubscriptionPayment.Domain.Repositories;

namespace DevTech.SubscriptionPayment.Application.Internal.QueryServices;

/// <summary>
///     Subscription plan query service.
/// </summary>
/// <param name="subscriptionPlanRepository">The <see cref="ISubscriptionPlanRepository" /> to use.</param>
public class SubscriptionPlanQueryService(ISubscriptionPlanRepository subscriptionPlanRepository)
    : ISubscriptionPlanQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<SubscriptionPlan>> Handle(GetAllSubscriptionPlansQuery query,
        CancellationToken cancellationToken)
    {
        return await subscriptionPlanRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<SubscriptionPlan?> Handle(GetSubscriptionPlanByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await subscriptionPlanRepository.FindByIdAsNoTrackingAsync(query.SubscriptionPlanId,
            cancellationToken);
    }
}
