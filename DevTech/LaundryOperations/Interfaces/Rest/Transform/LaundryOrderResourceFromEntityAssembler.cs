using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Interfaces.Rest.Resources;
namespace DevTech.LaundryOperations.Interfaces.Rest.Transform;

/// <summary>Transforms a domain entity into its public REST resource.</summary>
public static class LaundryOrderResourceFromEntityAssembler
{
    public static LaundryOrderResource ToResourceFromEntity(LaundryOrder entity, DateTimeOffset now) => new(entity.Id.Value, entity.OrderId, entity.LaundryId.Value, entity.CurrentStage, entity.Priority, entity.WashingCycleId?.Value, entity.ResourceId?.Value, entity.ExpectedCompletion, entity.IsReady, entity.IsAtRisk(now));
}

