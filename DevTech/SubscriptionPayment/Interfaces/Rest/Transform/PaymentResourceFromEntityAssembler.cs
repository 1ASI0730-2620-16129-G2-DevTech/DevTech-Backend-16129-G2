using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

namespace DevTech.SubscriptionPayment.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming a <see cref="Payment" /> into a <see cref="PaymentResource" />.
/// </summary>
public static class PaymentResourceFromEntityAssembler
{
    public static PaymentResource ToResourceFromEntity(Payment entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "Payment entity cannot be null when converting to resource.");
        return new PaymentResource(entity.Id, entity.CustomerId, entity.OrderId, entity.Amount.Amount,
            entity.Amount.Currency, entity.Status.ToString(), entity.TransactionId, entity.ProcessedAt);
    }
}
