using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Interfaces.Rest.Resources;
namespace DevTech.LaundryOperations.Interfaces.Rest.Transform;

/// <summary>Transforms an incoming REST resource into a command.</summary>
public static class AssignResourceCommandFromResourceAssembler
{
    public static AssignResourceCommand ToCommandFromResource(AssignLaundryResourceResource resource, Guid id)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new(id, resource.ResourceId);
    }
}

