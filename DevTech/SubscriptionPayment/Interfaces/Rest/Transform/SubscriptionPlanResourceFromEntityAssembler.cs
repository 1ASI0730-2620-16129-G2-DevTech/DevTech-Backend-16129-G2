using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

namespace DevTech.SubscriptionPayment.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming a <see cref="SubscriptionPlan" /> into a
///     <see cref="SubscriptionPlanResource" />.
/// </summary>
public static class SubscriptionPlanResourceFromEntityAssembler
{
    public static SubscriptionPlanResource ToResourceFromEntity(SubscriptionPlan entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "SubscriptionPlan entity cannot be null when converting to resource.");
        return new SubscriptionPlanResource(entity.Id, entity.Name, entity.Price.Amount, entity.Price.Currency,
            entity.Credits, entity.DurationDays);
    }
}
