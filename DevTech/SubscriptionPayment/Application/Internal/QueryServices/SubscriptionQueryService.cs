using DevTech.SubscriptionPayment.Application.QueryServices;
using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Queries;
using DevTech.SubscriptionPayment.Domain.Repositories;

namespace DevTech.SubscriptionPayment.Application.Internal.QueryServices;

/// <summary>
///     Subscription query service.
/// </summary>
/// <param name="subscriptionRepository">The <see cref="ISubscriptionRepository" /> to use.</param>
public class SubscriptionQueryService(ISubscriptionRepository subscriptionRepository) : ISubscriptionQueryService
{
    /// <inheritdoc />
    public async Task<Subscription?> Handle(GetSubscriptionByIdQuery query, CancellationToken cancellationToken)
    {
        return await subscriptionRepository.FindByIdAsNoTrackingAsync(query.SubscriptionId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Subscription?> Handle(GetSubscriptionByCustomerIdQuery query, CancellationToken cancellationToken)
    {
        return await subscriptionRepository.FindLatestByCustomerIdAsNoTrackingAsync(query.CustomerId, cancellationToken);
    }
}
