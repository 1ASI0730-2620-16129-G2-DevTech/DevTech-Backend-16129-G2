namespace DevTech.SubscriptionPayment.Domain.Model.ValueObjects;

/// <summary>
///     Represents the result of charging a payment through the payment gateway.
/// </summary>
/// <remarks>
///     This value object belongs to the Subscription &amp; Payment model. The anti-corruption layer
///     translates the external provider response into this object, so the domain never depends
///     on the external provider's data structures.
/// </remarks>
/// <param name="TransactionId">The transaction identifier assigned by the payment provider.</param>
/// <param name="Status">The result status: <see cref="Approved" /> or <see cref="Rejected" />.</param>
public record PaymentResult(string TransactionId, string Status)
{
    public const string Approved = "APPROVED";
    public const string Rejected = "REJECTED";

    /// <summary>
    ///     Indicates whether the charge was approved by the payment provider.
    /// </summary>
    public bool IsApproved => Status == Approved;
}
