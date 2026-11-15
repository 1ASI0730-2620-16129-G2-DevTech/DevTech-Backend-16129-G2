namespace DevTech.SubscriptionPayment.Domain.Model.Commands;

/// <summary>
///     Command to validate the active subscription of a customer (marks it as expired when its date has passed).
/// </summary>
/// <param name="CustomerId">The customer identifier.</param>
public record ValidateSubscriptionCommand(int CustomerId);
