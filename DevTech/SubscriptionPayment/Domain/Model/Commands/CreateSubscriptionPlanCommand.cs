namespace DevTech.SubscriptionPayment.Domain.Model.Commands;

/// <summary>
///     Command to create a subscription plan.
/// </summary>
/// <param name="Name">The name of the plan.</param>
/// <param name="Price">The price of the plan.</param>
/// <param name="Currency">The ISO 4217 currency code of the price.</param>
/// <param name="Credits">The number of credits (benefits) granted by the plan.</param>
/// <param name="DurationDays">The duration of the plan in days.</param>
public record CreateSubscriptionPlanCommand(string Name, decimal Price, string Currency, int Credits, int DurationDays);
