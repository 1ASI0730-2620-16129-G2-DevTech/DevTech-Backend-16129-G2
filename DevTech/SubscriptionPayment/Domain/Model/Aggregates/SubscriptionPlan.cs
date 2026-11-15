using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Aggregates.Internal;
using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.Aggregates;

/// <summary>
///     Represents the SubscriptionPlan aggregate root within the Subscription &amp; Payment bounded context.
/// </summary>
/// <remarks>
///     A plan defines the price, the number of credits (benefits) and the duration in days
///     that a customer receives when subscribing to it.
/// </remarks>
public class SubscriptionPlan : AggregateRoot
{
    /// <summary>
    ///     Default constructor required by Entity Framework Core.
    /// </summary>
    public SubscriptionPlan()
    {
        Name = string.Empty;
        Price = new Money(0, "PEN");
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SubscriptionPlan" /> aggregate.
    /// </summary>
    /// <param name="name">The name of the plan.</param>
    /// <param name="price">The price of the plan.</param>
    /// <param name="credits">The number of credits granted by the plan. Must be positive.</param>
    /// <param name="durationDays">The duration of the plan in days. Must be positive.</param>
    public SubscriptionPlan(string name, Money price, int credits, int durationDays)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(price);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(credits);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(durationDays);
        Name = name;
        Price = price;
        Credits = credits;
        DurationDays = durationDays;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SubscriptionPlan" /> aggregate using a
    ///     <see cref="CreateSubscriptionPlanCommand" />.
    /// </summary>
    public SubscriptionPlan(CreateSubscriptionPlanCommand command)
        : this(command.Name, new Money(command.Price, command.Currency), command.Credits, command.DurationDays)
    {
    }

    public SubscriptionPlanId Id { get; private set; }
    public string Name { get; private set; }
    public Money Price { get; private set; }
    public int Credits { get; private set; }
    public int DurationDays { get; private set; }
}
