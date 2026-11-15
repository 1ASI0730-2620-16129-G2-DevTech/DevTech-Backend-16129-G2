using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

namespace DevTech.SubscriptionPayment.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming a <see cref="CreateSubscriptionResource" /> into a
///     <see cref="CreateSubscriptionCommand" />.
/// </summary>
public static class CreateSubscriptionCommandFromResourceAssembler
{
    public static CreateSubscriptionCommand ToCommandFromResource(CreateSubscriptionResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "CreateSubscriptionResource cannot be null when converting to command.");
        return new CreateSubscriptionCommand(resource.CustomerId, resource.PlanId);
    }
}
