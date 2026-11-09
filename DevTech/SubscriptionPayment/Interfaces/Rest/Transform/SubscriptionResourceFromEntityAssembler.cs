using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

namespace DevTech.SubscriptionPayment.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming a <see cref="Subscription" /> into a <see cref="SubscriptionResource" />.
/// </summary>
public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "Subscription entity cannot be null when converting to resource.");
        return new SubscriptionResource(entity.Id, entity.CustomerId, entity.PlanId, entity.StartDate,
            entity.ExpirationDate, entity.Status.ToString(), entity.Credits);
    }
}
