using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

namespace DevTech.SubscriptionPayment.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming a <see cref="CreateSubscriptionPlanResource" /> into a
///     <see cref="CreateSubscriptionPlanCommand" />.
/// </summary>
public static class CreateSubscriptionPlanCommandFromResourceAssembler
{
    public static CreateSubscriptionPlanCommand ToCommandFromResource(CreateSubscriptionPlanResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "CreateSubscriptionPlanResource cannot be null when converting to command.");
        return new CreateSubscriptionPlanCommand(resource.Name, resource.Price, resource.Currency, resource.Credits,
            resource.DurationDays);
    }
}
