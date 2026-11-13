using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Interfaces.Rest.Resources;
namespace DevTech.LaundryOperations.Interfaces.Rest.Transform;

/// <summary>Transforms a domain entity into its public REST resource.</summary>
public static class LaundryResourceResourceFromEntityAssembler
{
    public static LaundryResourceResource ToResourceFromEntity(LaundryResource entity) => new(entity.Id.Value, entity.LaundryId.Value, entity.Name, entity.Status, entity.Capacity, entity.AssignedOrderId);
}

