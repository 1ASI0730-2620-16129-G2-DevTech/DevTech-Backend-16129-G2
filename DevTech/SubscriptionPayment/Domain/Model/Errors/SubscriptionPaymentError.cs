namespace DevTech.SubscriptionPayment.Domain.Model.Errors;

/// <summary>
///     Error codes of the Subscription &amp; Payment bounded context.
/// </summary>
public enum SubscriptionPaymentError
{
    None,
    SubscriptionPlanNotFound,
    InvalidSubscriptionPlanData,
    InvalidSubscriptionData,
    SubscriptionNotFound,
    ActiveSubscriptionAlreadyExists,
    SubscriptionNotActive,
    InsufficientCredits,
    PaymentNotFound,
    InvalidPaymentData,
    PaymentRejected,
    InvalidPaymentStatus,
    PaymentGatewayUnavailable
}
