using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Queries;

namespace DevTech.SubscriptionPayment.Application.QueryServices;

/// <summary>
///     Represents the subscription plan query service in the Subscription &amp; Payment bounded context.
/// </summary>
public interface ISubscriptionPlanQueryService
{
    /// <summary>
    ///     Handles the get all subscription plans query.
    /// </summary>
    Task<IEnumerable<SubscriptionPlan>> Handle(GetAllSubscriptionPlansQuery query, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the get subscription plan by id query.
    /// </summary>
    Task<SubscriptionPlan?> Handle(GetSubscriptionPlanByIdQuery query, CancellationToken cancellationToken);
}
