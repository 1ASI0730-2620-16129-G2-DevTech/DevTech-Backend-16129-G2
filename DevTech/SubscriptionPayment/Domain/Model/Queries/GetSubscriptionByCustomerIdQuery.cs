namespace DevTech.SubscriptionPayment.Domain.Model.Queries;

/// <summary>
///     Query to get the most recent subscription of a customer, whatever its status.
/// </summary>
/// <param name="CustomerId">The customer identifier.</param>
public record GetSubscriptionByCustomerIdQuery(int CustomerId);
