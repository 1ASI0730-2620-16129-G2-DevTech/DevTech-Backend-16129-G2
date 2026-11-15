namespace DevTech.SubscriptionPayment.Domain.Model.Queries;

/// <summary>
///     Query to get the payments, optionally only the ones of a customer.
/// </summary>
/// <param name="CustomerId">The customer identifier; null returns the payments of every customer.</param>
public record GetAllPaymentsQuery(int? CustomerId = null);
