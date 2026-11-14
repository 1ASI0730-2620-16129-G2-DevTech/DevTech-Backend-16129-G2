using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Interfaces.Rest.Resources;

namespace DevTech.SubscriptionPayment.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming a <see cref="CreatePaymentResource" /> into a
///     <see cref="ProcessPaymentCommand" />.
/// </summary>
public static class ProcessPaymentCommandFromResourceAssembler
{
    public static ProcessPaymentCommand ToCommandFromResource(CreatePaymentResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "CreatePaymentResource cannot be null when converting to command.");
        return new ProcessPaymentCommand(resource.CustomerId, resource.OrderId, resource.Amount, resource.Currency,
            resource.CardToken);
    }
}
