using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.SubscriptionPayment.Domain.Repositories;

/// <summary>
///     Represents the subscription plan repository in the Subscription &amp; Payment bounded context.
/// </summary>
public interface ISubscriptionPlanRepository : IBaseRepository<SubscriptionPlan>
{
}
