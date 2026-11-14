using DevTech.SubscriptionPayment.Application.QueryServices;
using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.Queries;
using DevTech.SubscriptionPayment.Domain.Repositories;

namespace DevTech.SubscriptionPayment.Application.Internal.QueryServices;

/// <summary>
///     Payment query service.
/// </summary>
/// <param name="paymentRepository">The <see cref="IPaymentRepository" /> to use.</param>
public class PaymentQueryService(IPaymentRepository paymentRepository) : IPaymentQueryService
{
    /// <inheritdoc />
    public async Task<Payment?> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken)
    {
        return await paymentRepository.FindByIdAsNoTrackingAsync(query.PaymentId, cancellationToken);
    }
}
