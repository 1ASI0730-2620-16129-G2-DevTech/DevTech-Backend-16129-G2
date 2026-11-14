using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Domain.Model.Events;
using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Aggregates.Internal;
using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Domain.Model.Aggregates;

/// <summary>
///     Represents the Payment aggregate root within the Subscription &amp; Payment bounded context.
/// </summary>
/// <remarks>
///     Lifecycle: PENDING, then (approved by the provider) PENDING with a transaction id,
///     then COMPLETED once confirmed. A payment rejected by the provider becomes FAILED.
///     The customer and the order are referenced by id only.
/// </remarks>
public partial class Payment : AggregateRoot
{
    /// <summary>
    ///     Default constructor required by Entity Framework Core.
    /// </summary>
    public Payment()
    {
        Amount = new Money(0, "PEN");
        Status = EPaymentStatus.PENDING;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Payment" /> aggregate in PENDING status.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="amount">The amount to charge. Must be greater than zero.</param>
    public Payment(int customerId, int orderId, Money amount) : this()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderId);
        ArgumentNullException.ThrowIfNull(amount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount.Amount);
        CustomerId = customerId;
        OrderId = orderId;
        Amount = amount;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Payment" /> aggregate using a <see cref="ProcessPaymentCommand" />.
    /// </summary>
    public Payment(ProcessPaymentCommand command)
        : this(command.CustomerId, command.OrderId, new Money(command.Amount, command.Currency))
    {
    }

    public PaymentId Id { get; private set; }
    public int CustomerId { get; private set; }
    public int OrderId { get; private set; }
    public Money Amount { get; private set; }
    public EPaymentStatus Status { get; private set; }
    public string? TransactionId { get; private set; }
    public DateTimeOffset? ProcessedAt { get; private set; }

    /// <summary>
    ///     Indicates whether the payment was approved by the provider and is waiting for confirmation.
    /// </summary>
    public bool IsAwaitingConfirmation => Status == EPaymentStatus.PENDING && TransactionId is not null;

    /// <summary>
    ///     Records the result of charging the payment through the payment gateway.
    /// </summary>
    /// <remarks>
    ///     Must be called after the payment is persisted, because the events carry the database-generated id.
    /// </remarks>
    /// <param name="result">The <see cref="PaymentResult" /> returned by the payment gateway.</param>
    /// <exception cref="InvalidOperationException">Thrown when the payment was already processed.</exception>
    public void Process(PaymentResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        if (Status != EPaymentStatus.PENDING || TransactionId is not null)
            throw new InvalidOperationException("The payment was already processed.");

        TransactionId = result.TransactionId;
        ProcessedAt = DateTimeOffset.UtcNow;

        if (result.IsApproved)
        {
            AddDomainEvent(new PaymentProcessedEvent(Id, OrderId, result.TransactionId));
            return;
        }

        Status = EPaymentStatus.FAILED;
        AddDomainEvent(new PaymentFailedEvent(Id, OrderId));
    }

    /// <summary>
    ///     Confirms an approved payment, moving it to COMPLETED status.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the payment is not awaiting confirmation.</exception>
    public void Confirm()
    {
        if (!IsAwaitingConfirmation)
            throw new InvalidOperationException("Only an approved payment in PENDING status can be confirmed.");
        Status = EPaymentStatus.COMPLETED;
        AddDomainEvent(new PaymentConfirmedEvent(Id, OrderId, CustomerId));
    }
}
