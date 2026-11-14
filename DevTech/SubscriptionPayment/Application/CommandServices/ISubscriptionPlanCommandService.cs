using DevTech.Shared.Application.Model;
using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Commands;

namespace DevTech.SubscriptionPayment.Application.CommandServices;

/// <summary>
///     Represents the subscription plan command service in the Subscription &amp; Payment bounded context.
/// </summary>
public interface ISubscriptionPlanCommandService
{
    /// <summary>
    ///     Handles the create subscription plan command.
    /// </summary>
    /// <param name="command">The <see cref="CreateSubscriptionPlanCommand" /> command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created <see cref="SubscriptionPlan" />.</returns>
    Task<Result<SubscriptionPlan>> Handle(CreateSubscriptionPlanCommand command, CancellationToken cancellationToken);
}
