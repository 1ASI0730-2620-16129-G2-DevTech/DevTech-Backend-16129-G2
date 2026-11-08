using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Domain.Model.Events;
using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Aggregates.Internal;

namespace DevTech.SubscriptionPayment.Domain.Model.Aggregates;

/// <summary>
///     Represents the Subscription aggregate root within the Subscription &amp; Payment bounded context.
/// </summary>
/// <remarks>
///     A subscription links a customer (referenced by id only) to a <see cref="SubscriptionPlan" />
///     (referenced by <see cref="SubscriptionPlanId" />) and keeps the credits the customer can still use.
/// </remarks>
public partial class Subscription : AggregateRoot
{
    /// <summary>
    ///     Default constructor required by Entity Framework Core.
    /// </summary>
    public Subscription()
    {
        Status = ESubscriptionStatus.ACTIVE;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Subscription" /> aggregate for the given plan.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="plan">The plan the customer subscribes to.</param>
    public Subscription(int customerId, SubscriptionPlan plan) : this()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId);
        ArgumentNullException.ThrowIfNull(plan);
        CustomerId = customerId;
        PlanId = plan.Id;
        ApplyPlanTerms(plan);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Subscription" /> aggregate using a
    ///     <see cref="CreateSubscriptionCommand" />.
    /// </summary>
    public Subscription(CreateSubscriptionCommand command, SubscriptionPlan plan) : this(command.CustomerId, plan)
    {
    }

    public SubscriptionId Id { get; private set; }
    public int CustomerId { get; private set; }
    public SubscriptionPlanId PlanId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly ExpirationDate { get; private set; }
    public ESubscriptionStatus Status { get; private set; }
    public int Credits { get; private set; }

    /// <summary>
    ///     Indicates whether the subscription is active.
    /// </summary>
    public bool IsActive => Status == ESubscriptionStatus.ACTIVE;

    private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

    /// <summary>
    ///     Activates the subscription starting today, using the duration and credits of the plan.
    /// </summary>
    /// <remarks>
    ///     Must be called after the subscription is persisted, because the event carries the database-generated id.
    /// </remarks>
    /// <param name="plan">The plan that defines the duration and credits.</param>
    public void Activate(SubscriptionPlan plan)
    {
        ApplyPlanTerms(plan);
        Status = ESubscriptionStatus.ACTIVE;
        AddDomainEvent(new SubscriptionActivatedEvent(Id, CustomerId, PlanId));
    }

    private void ApplyPlanTerms(SubscriptionPlan plan)
    {
        StartDate = Today;
        ExpirationDate = StartDate.AddDays(plan.DurationDays);
        Credits = plan.Credits;
    }

    /// <summary>
    ///     Validates the subscription. An active subscription whose expiration date has passed becomes expired.
    /// </summary>
    /// <returns><c>true</c> if the subscription is still active; otherwise, <c>false</c>.</returns>
    public bool Validate()
    {
        if (IsActive && Today > ExpirationDate)
            Status = ESubscriptionStatus.EXPIRED;
        AddDomainEvent(new SubscriptionValidatedEvent(Id, Status));
        return IsActive;
    }

    /// <summary>
    ///     Applies a subscription benefit, consuming one credit.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the subscription is not active or has no credits left.
    /// </exception>
    public void ApplyBenefit()
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot apply a benefit to a subscription that is not active.");
        if (Credits <= 0)
            throw new InvalidOperationException("The subscription has no credits left.");
        Credits--;
        AddDomainEvent(new BenefitAppliedEvent(Id, Credits));
    }

    /// <summary>
    ///     Cancels the subscription.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the subscription is not active.</exception>
    public void Cancel()
    {
        if (!IsActive)
            throw new InvalidOperationException("Only an active subscription can be cancelled.");
        Status = ESubscriptionStatus.CANCELLED;
        AddDomainEvent(new SubscriptionCancelledEvent(Id));
    }
}
