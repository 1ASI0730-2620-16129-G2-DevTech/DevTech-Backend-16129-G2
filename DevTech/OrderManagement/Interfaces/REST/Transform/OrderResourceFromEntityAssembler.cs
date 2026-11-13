using DevTech.OrderManagement.Domain.Model.Aggregates;
using DevTech.OrderManagement.Interfaces.REST.Resources;

namespace DevTech.OrderManagement.Interfaces.REST.Transform;

public static class OrderResourceFromEntityAssembler
{
    public static OrderResource ToResourceFromEntity(Order entity)
    {
        return new OrderResource(
            entity.Id.Value,
            entity.CustomerId,
            entity.LaundryId,
            entity.Status.ToString(),
            entity.DeliveryMethod.ToString(),
            entity.ServiceType.ToString(),
            entity.EstimatedDeliveryDate,
            entity.SpecialCareInstructions,
            entity.CreatedAt ?? default,
            entity.Items.Select(GarmentItemResourceFromEntityAssembler.ToResourceFromEntity).ToList());
    }
}
