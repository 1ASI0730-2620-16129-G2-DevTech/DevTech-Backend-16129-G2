using DevTech.OrderManagement.Domain.Model.Entities;
using DevTech.OrderManagement.Interfaces.REST.Resources;

namespace DevTech.OrderManagement.Interfaces.REST.Transform;

public static class GarmentItemResourceFromEntityAssembler
{
    public static GarmentItemResource ToResourceFromEntity(GarmentItem entity)
    {
        return new GarmentItemResource(entity.Id.Value, entity.Type, entity.Quantity, entity.CareInstructions);
    }
}
