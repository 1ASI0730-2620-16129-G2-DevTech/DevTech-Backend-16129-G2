using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.SubscriptionPayment.Application.Internal.OutboundServices;

/// <summary>
///     Port used by the application layer to charge payments through an external payment provider.
/// </summary>
/// <remarks>
///     The application layer depends only on this abstraction. The infrastructure layer implements it
///     (PaymentGatewayAdapter) and hides the external provider behind an anti-corruption layer.
/// </remarks>
public interface IPaymentGateway
{
    /// <summary>
    ///     Charges the given amount using the tokenized card.
    /// </summary>
    /// <param name="amount">The amount to charge.</param>
    /// <param name="cardToken">The tokenized card provided by the client.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="PaymentResult" /> of the charge.</returns>
    Task<PaymentResult> ProcessPaymentAsync(Money amount, string cardToken, CancellationToken cancellationToken);
}
