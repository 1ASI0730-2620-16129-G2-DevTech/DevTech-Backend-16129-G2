using DevTech.OrderManagement.Domain.Model.Commands;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.OrderManagement.Interfaces.REST.Resources;

namespace DevTech.OrderManagement.Interfaces.REST.Transform;

/// <remarks>
///     Enum names were already validated by <c>[EnumString]</c> on the resource, so parsing cannot fail here.
///     The garments are created with an empty <c>OrderId</c>: the order does not exist yet and the
///     <c>Order</c> aggregate stamps its own id on every item.
/// </remarks>
public static class CreateOrderCommandFromResourceAssembler
{
    public static CreateOrderCommand ToCommandFromResource(CreateOrderResource resource)
    {
        var items = (resource.Items ?? Enumerable.Empty<AddGarmentItemResource>())
            .Select(item => new AddGarmentItemCommand(
                new OrderId(Guid.Empty), item.Type, item.Quantity, item.CareInstructions))
            .ToList();

        return new CreateOrderCommand(
            resource.CustomerId,
            resource.LaundryId,
            Enum.Parse<DeliveryMethod>(resource.DeliveryMethod, true),
            Enum.Parse<ServiceType>(resource.ServiceType, true),
            resource.EstimatedDeliveryDate,
            resource.SpecialCareInstructions,
            items);
    }
}
