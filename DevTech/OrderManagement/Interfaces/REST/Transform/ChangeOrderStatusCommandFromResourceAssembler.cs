using DevTech.OrderManagement.Domain.Model.Commands;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.OrderManagement.Interfaces.REST.Resources;

namespace DevTech.OrderManagement.Interfaces.REST.Transform;

public static class ChangeOrderStatusCommandFromResourceAssembler
{
    public static ChangeOrderStatusCommand ToCommandFromResource(Guid orderId, ChangeOrderStatusResource resource)
    {
        return new ChangeOrderStatusCommand(new OrderId(orderId), Enum.Parse<OrderStatus>(resource.Status, true));
    }
}
