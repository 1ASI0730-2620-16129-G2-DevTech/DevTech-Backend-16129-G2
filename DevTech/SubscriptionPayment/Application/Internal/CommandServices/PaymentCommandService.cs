using DevTech.Shared.Application.Model;
using DevTech.SubscriptionPayment.Application.CommandServices;
using DevTech.SubscriptionPayment.Application.Internal.OutboundServices;
using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Domain.Model.Errors;
using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.SubscriptionPayment.Domain.Repositories;
using DevTech.Shared.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.SubscriptionPayment.Application.Internal.CommandServices;

/// <summary>
///     Payment command service.
/// </summary>
/// <param name="paymentRepository">The <see cref="IPaymentRepository" /> to use.</param>
/// <param name="paymentGateway">The <see cref="IPaymentGateway" /> port used to charge payments.</param>
/// <param name="unitOfWork">The <see cref="IUnitOfWork" /> to use.</param>
public class PaymentCommandService(
    IPaymentRepository paymentRepository,
    IPaymentGateway paymentGateway,
    IUnitOfWork unitOfWork)
    : IPaymentCommandService
{
    /// <inheritdoc />
    public async Task<Result<Payment>> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken)
    {
        if (command.CustomerId <= 0 || command.OrderId <= 0 || command.Amount <= 0 ||
            !Money.IsValidCurrency(command.Currency) || string.IsNullOrWhiteSpace(command.CardToken))
            return Result<Payment>.Failure(SubscriptionPaymentErrors.InvalidPaymentData,
                SubscriptionPaymentError.InvalidPaymentData);

        var payment = new Payment(command);

        PaymentResult paymentResult;
        try
        {
            paymentResult = await paymentGateway.ProcessPaymentAsync(payment.Amount, command.CardToken,
                cancellationToken);
        }
        catch (HttpRequestException)
        {
            return Result<Payment>.Failure(SubscriptionPaymentErrors.PaymentGatewayUnavailable,
                SubscriptionPaymentError.PaymentGatewayUnavailable);
        }

        await paymentRepository.AddAsync(payment, cancellationToken);
        // Save first: the database generates the id that the domain events must carry.
        await unitOfWork.CompleteAsync(cancellationToken);

        payment.Process(paymentResult);
        await unitOfWork.CompleteAsync(cancellationToken);

        // The rejected payment is stored as FAILED, but the request is reported as an error.
        return paymentResult.IsApproved
            ? Result<Payment>.Success(payment)
            : Result<Payment>.Failure(SubscriptionPaymentErrors.PaymentRejected,
                SubscriptionPaymentError.PaymentRejected);
    }

    /// <inheritdoc />
    public async Task<Result<Payment>> Handle(ConfirmPaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.FindByIdAsync(command.PaymentId, cancellationToken);
        if (payment is null)
            return Result<Payment>.Failure(SubscriptionPaymentErrors.PaymentNotFound,
                SubscriptionPaymentError.PaymentNotFound);

        if (!payment.IsAwaitingConfirmation)
            return Result<Payment>.Failure(SubscriptionPaymentErrors.InvalidPaymentStatus,
                SubscriptionPaymentError.InvalidPaymentStatus);

        payment.Confirm();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Payment>.Success(payment);
    }
}
