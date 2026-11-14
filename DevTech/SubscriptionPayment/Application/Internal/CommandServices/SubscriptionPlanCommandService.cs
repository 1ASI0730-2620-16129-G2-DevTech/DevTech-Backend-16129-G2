using DevTech.Shared.Application.Model;
using DevTech.SubscriptionPayment.Application.CommandServices;
using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Domain.Model.Errors;
using DevTech.SubscriptionPayment.Domain.Repositories;
using DevTech.Shared.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.SubscriptionPayment.Application.Internal.CommandServices;

/// <summary>
///     Subscription plan command service.
/// </summary>
/// <param name="subscriptionPlanRepository">The <see cref="ISubscriptionPlanRepository" /> to use.</param>
/// <param name="unitOfWork">The <see cref="IUnitOfWork" /> to use.</param>
public class SubscriptionPlanCommandService(
    ISubscriptionPlanRepository subscriptionPlanRepository,
    IUnitOfWork unitOfWork)
    : ISubscriptionPlanCommandService
{
    /// <inheritdoc />
    public async Task<Result<SubscriptionPlan>> Handle(CreateSubscriptionPlanCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Name) || command.Price <= 0 ||
            !Money.IsValidCurrency(command.Currency) || command.Credits <= 0 || command.DurationDays <= 0)
            return Result<SubscriptionPlan>.Failure(SubscriptionPaymentErrors.InvalidSubscriptionPlanData,
                SubscriptionPaymentError.InvalidSubscriptionPlanData);

        var subscriptionPlan = new SubscriptionPlan(command);
        await subscriptionPlanRepository.AddAsync(subscriptionPlan, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<SubscriptionPlan>.Success(subscriptionPlan);
    }
}
