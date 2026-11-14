namespace DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

/// <summary>
///     Represents the status of a payment.
/// </summary>
public enum EPaymentStatus
{
    PENDING,
    COMPLETED,
    FAILED
}
