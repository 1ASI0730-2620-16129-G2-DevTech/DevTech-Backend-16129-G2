using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Queries;

namespace DevTech.SubscriptionPayment.Application.QueryServices;

/// <summary>
///     Represents the payment query service in the Subscription &amp; Payment bounded context.
/// </summary>
public interface IPaymentQueryService
{
    /// <summary>
    ///     Handles the get payment by id query.
    /// </summary>
    Task<Payment?> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken);
}
