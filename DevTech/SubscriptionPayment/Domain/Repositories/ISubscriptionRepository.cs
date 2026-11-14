using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.SubscriptionPayment.Domain.Repositories;

/// <summary>
///     Represents the subscription repository in the Subscription &amp; Payment bounded context.
/// </summary>
public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    /// <summary>
    ///     Finds the active subscription of a customer.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The active <see cref="Subscription" /> if found; otherwise, <c>null</c>.</returns>
    Task<Subscription?> FindActiveByCustomerIdAsync(int customerId, CancellationToken cancellationToken);

    /// <summary>
    ///     Finds the most recent subscription of a customer, whatever its status, without change tracking.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The latest <see cref="Subscription" /> if found; otherwise, <c>null</c>.</returns>
    Task<Subscription?> FindLatestByCustomerIdAsNoTrackingAsync(int customerId, CancellationToken cancellationToken);
}
