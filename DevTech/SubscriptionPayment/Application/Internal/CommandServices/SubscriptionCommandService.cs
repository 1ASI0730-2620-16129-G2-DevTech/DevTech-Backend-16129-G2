using Acme.Center.Platform.Shared.Application.Model;
using DevTech.SubscriptionPayment.Application.CommandServices;
using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Domain.Model.Errors;
using DevTech.SubscriptionPayment.Domain.Repositories;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.SubscriptionPayment.Application.Internal.CommandServices;

/// <summary>
///     Subscription command service.
/// </summary>
/// <param name="subscriptionRepository">The <see cref="ISubscriptionRepository" /> to use.</param>
/// <param name="subscriptionPlanRepository">The <see cref="ISubscriptionPlanRepository" /> to use.</param>
/// <param name="unitOfWork">The <see cref="IUnitOfWork" /> to use.</param>
public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    ISubscriptionPlanRepository subscriptionPlanRepository,
    IUnitOfWork unitOfWork)
    : ISubscriptionCommandService
{
    /// <inheritdoc />
    public async Task<Result<Subscription>> Handle(CreateSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        if (command.CustomerId <= 0)
            return Result<Subscription>.Failure(SubscriptionPaymentErrors.InvalidSubscriptionData,
                SubscriptionPaymentError.InvalidSubscriptionData);

        var plan = await subscriptionPlanRepository.FindByIdAsync(command.PlanId, cancellationToken);
        if (plan is null)
            return Result<Subscription>.Failure(SubscriptionPaymentErrors.SubscriptionPlanNotFound,
                SubscriptionPaymentError.SubscriptionPlanNotFound);

        // A customer can only have one active subscription. Expired ones are updated first.
        var current = await subscriptionRepository.FindActiveByCustomerIdAsync(command.CustomerId, cancellationToken);
        if (current is not null && current.Validate())
            return Result<Subscription>.Failure(SubscriptionPaymentErrors.ActiveSubscriptionAlreadyExists,
                SubscriptionPaymentError.ActiveSubscriptionAlreadyExists);

        var subscription = new Subscription(command, plan);
        await subscriptionRepository.AddAsync(subscription, cancellationToken);
        // Save first: the database generates the id that the domain event must carry.
        await unitOfWork.CompleteAsync(cancellationToken);

        subscription.Activate(plan);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Subscription>.Success(subscription);
    }

    /// <inheritdoc />
    public async Task<Result<Subscription>> Handle(ValidateSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        var subscription =
            await subscriptionRepository.FindActiveByCustomerIdAsync(command.CustomerId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionPaymentErrors.ActiveSubscriptionNotFound,
                SubscriptionPaymentError.SubscriptionNotFound);

        subscription.Validate();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Subscription>.Success(subscription);
    }

    /// <inheritdoc />
    public async Task<Result<Subscription>> Handle(ApplyBenefitCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionPaymentErrors.SubscriptionNotFound,
                SubscriptionPaymentError.SubscriptionNotFound);

        if (!subscription.Validate())
        {
            // Persist the EXPIRED status if the validation changed it.
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Subscription>.Failure(SubscriptionPaymentErrors.SubscriptionNotActive,
                SubscriptionPaymentError.SubscriptionNotActive);
        }

        if (subscription.Credits <= 0)
            return Result<Subscription>.Failure(SubscriptionPaymentErrors.InsufficientCredits,
                SubscriptionPaymentError.InsufficientCredits);

        subscription.ApplyBenefit();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Subscription>.Success(subscription);
    }

    /// <inheritdoc />
    public async Task<Result<Subscription>> Handle(CancelSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionPaymentErrors.SubscriptionNotFound,
                SubscriptionPaymentError.SubscriptionNotFound);

        if (!subscription.IsActive)
            return Result<Subscription>.Failure(SubscriptionPaymentErrors.SubscriptionNotActive,
                SubscriptionPaymentError.SubscriptionNotActive);

        subscription.Cancel();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Subscription>.Success(subscription);
    }
}
