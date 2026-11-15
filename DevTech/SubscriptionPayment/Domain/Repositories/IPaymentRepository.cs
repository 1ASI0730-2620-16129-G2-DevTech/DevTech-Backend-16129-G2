using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.SubscriptionPayment.Domain.Repositories;

/// <summary>
///     Represents the payment repository in the Subscription &amp; Payment bounded context.
/// </summary>
public interface IPaymentRepository : IBaseRepository<Payment>
{
    /// <summary>
    ///     Lists the payments, newest first, optionally only the ones of a customer.
    /// </summary>
    /// <param name="customerId">The customer identifier; null lists the payments of every customer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<IEnumerable<Payment>> ListByCustomerAsync(int? customerId, CancellationToken cancellationToken = default);
}
