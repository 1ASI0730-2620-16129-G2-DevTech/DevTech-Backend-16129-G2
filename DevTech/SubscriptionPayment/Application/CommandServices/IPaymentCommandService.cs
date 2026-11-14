using DevTech.Shared.Application.Model;
using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Commands;

namespace DevTech.SubscriptionPayment.Application.CommandServices;

/// <summary>
///     Represents the payment command service in the Subscription &amp; Payment bounded context.
/// </summary>
public interface IPaymentCommandService
{
    /// <summary>
    ///     Handles the process payment command: creates the payment and charges it through the payment gateway.
    /// </summary>
    /// <param name="command">The <see cref="ProcessPaymentCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The processed <see cref="Payment" />.</returns>
    Task<Result<Payment>> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the confirm payment command.
    /// </summary>
    /// <param name="command">The <see cref="ConfirmPaymentCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The confirmed <see cref="Payment" />.</returns>
    Task<Result<Payment>> Handle(ConfirmPaymentCommand command, CancellationToken cancellationToken);
}
