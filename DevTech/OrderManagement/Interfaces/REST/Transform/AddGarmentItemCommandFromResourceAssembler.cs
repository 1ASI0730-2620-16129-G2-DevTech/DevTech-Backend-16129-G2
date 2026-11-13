using DevTech.OrderManagement.Domain.Model.Commands;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.OrderManagement.Interfaces.REST.Resources;

namespace DevTech.OrderManagement.Interfaces.REST.Transform;

public static class AddGarmentItemCommandFromResourceAssembler
{
    public static AddGarmentItemCommand ToCommandFromResource(Guid orderId, AddGarmentItemResource resource)
    {
        return new AddGarmentItemCommand(
            new OrderId(orderId), resource.Type, resource.Quantity, resource.CareInstructions);
    }
}
