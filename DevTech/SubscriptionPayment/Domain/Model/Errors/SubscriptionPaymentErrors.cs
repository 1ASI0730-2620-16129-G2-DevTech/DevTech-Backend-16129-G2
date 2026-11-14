using DevTech.Shared.Domain.Model.Errors;

namespace DevTech.SubscriptionPayment.Domain.Model.Errors;

/// <summary>
///     Domain errors of the Subscription &amp; Payment bounded context.
/// </summary>
public static class SubscriptionPaymentErrors
{
    public static readonly Error SubscriptionPlanNotFound =
        Error.NotFound("SubscriptionPayment.SubscriptionPlanNotFound", "The specified subscription plan was not found.");

    public static readonly Error InvalidSubscriptionPlanData =
        Error.Validation("SubscriptionPayment.InvalidSubscriptionPlanData",
            "The subscription plan requires a name, a positive price, a valid 3-letter currency, and positive credits and duration.");

    public static readonly Error SubscriptionNotFound =
        Error.NotFound("SubscriptionPayment.SubscriptionNotFound", "The specified subscription was not found.");

    public static readonly Error ActiveSubscriptionNotFound =
        Error.NotFound("SubscriptionPayment.ActiveSubscriptionNotFound", "The customer does not have an active subscription.");

    public static readonly Error CustomerSubscriptionNotFound =
        Error.NotFound("SubscriptionPayment.CustomerSubscriptionNotFound", "The customer does not have any subscription.");

    public static readonly Error InvalidSubscriptionData =
        Error.Validation("SubscriptionPayment.InvalidSubscriptionData", "The subscription requires a valid customer id.");

    public static readonly Error ActiveSubscriptionAlreadyExists =
        Error.Conflict("SubscriptionPayment.ActiveSubscriptionAlreadyExists", "The customer already has an active subscription.");

    public static readonly Error SubscriptionNotActive =
        Error.Conflict("SubscriptionPayment.SubscriptionNotActive", "The subscription is not active.");

    public static readonly Error InsufficientCredits =
        Error.Conflict("SubscriptionPayment.InsufficientCredits", "The subscription has no credits left.");

    public static readonly Error PaymentNotFound =
        Error.NotFound("SubscriptionPayment.PaymentNotFound", "The specified payment was not found.");

    public static readonly Error InvalidPaymentData =
        Error.Validation("SubscriptionPayment.InvalidPaymentData",
            "The payment requires a positive amount, a valid 3-letter currency and a card token.");

    public static readonly Error PaymentRejected =
        Error.Failure("SubscriptionPayment.PaymentRejected", "The payment was rejected by the payment provider.");

    public static readonly Error PaymentGatewayUnavailable =
        Error.Unexpected("SubscriptionPayment.PaymentGatewayUnavailable", "The payment provider is currently unavailable.");

    public static readonly Error InvalidPaymentStatus =
        Error.Conflict("SubscriptionPayment.InvalidPaymentStatus",
            "Only an approved payment in PENDING status can be confirmed.");
}
