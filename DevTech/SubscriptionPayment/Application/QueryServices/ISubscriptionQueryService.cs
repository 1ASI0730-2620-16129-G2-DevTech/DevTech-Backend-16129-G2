using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Queries;

namespace DevTech.SubscriptionPayment.Application.QueryServices;

/// <summary>
///     Represents the subscription query service in the Subscription &amp; Payment bounded context.
/// </summary>
public interface ISubscriptionQueryService
{
    /// <summary>
    ///     Handles the get subscription by id query.
    /// </summary>
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the get subscription by customer id query.
    /// </summary>
    Task<Subscription?> Handle(GetSubscriptionByCustomerIdQuery query, CancellationToken cancellationToken);
}
