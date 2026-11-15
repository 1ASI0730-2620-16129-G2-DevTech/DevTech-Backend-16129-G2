using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

namespace DevTech.SubscriptionPayment.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming a <see cref="ValidateSubscriptionResource" /> into a
///     <see cref="ValidateSubscriptionCommand" />.
/// </summary>
public static class ValidateSubscriptionCommandFromResourceAssembler
{
    public static ValidateSubscriptionCommand ToCommandFromResource(ValidateSubscriptionResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "ValidateSubscriptionResource cannot be null when converting to command.");
        return new ValidateSubscriptionCommand(resource.CustomerId);
    }
}
