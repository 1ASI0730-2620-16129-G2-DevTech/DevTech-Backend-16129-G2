using DevTech.Shared.Application.Model;
using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Commands;

namespace DevTech.SubscriptionPayment.Application.CommandServices;

/// <summary>
///     Represents the subscription command service in the Subscription &amp; Payment bounded context.
/// </summary>
public interface ISubscriptionCommandService
{
    /// <summary>
    ///     Handles the create subscription command (subscribes a customer to a plan).
    /// </summary>
    /// <param name="command">The <see cref="CreateSubscriptionCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created <see cref="Subscription" />.</returns>
    Task<Result<Subscription>> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the validate subscription command.
    /// </summary>
    /// <param name="command">The <see cref="ValidateSubscriptionCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The validated <see cref="Subscription" /> (ACTIVE or EXPIRED).</returns>
    Task<Result<Subscription>> Handle(ValidateSubscriptionCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the apply benefit command, consuming one credit of the subscription.
    /// </summary>
    /// <param name="command">The <see cref="ApplyBenefitCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated <see cref="Subscription" />.</returns>
    Task<Result<Subscription>> Handle(ApplyBenefitCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the cancel subscription command.
    /// </summary>
    /// <param name="command">The <see cref="CancelSubscriptionCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The cancelled <see cref="Subscription" />.</returns>
    Task<Result<Subscription>> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken);
}
