using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Interfaces.Rest.Resources;
namespace DevTech.LaundryOperations.Interfaces.Rest.Transform;

/// <summary>Transforms a domain entity into its public REST resource.</summary>
public static class WashingCycleResourceFromEntityAssembler
{
    public static WashingCycleResource ToResourceFromEntity(WashingCycle entity) => new(entity.Id.Value, entity.Name, entity.DurationMinutes, entity.CompatibleType);
}

