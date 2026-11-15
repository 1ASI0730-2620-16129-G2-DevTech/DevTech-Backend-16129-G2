using DevTech.Shared.Interfaces.Rest.ProblemDetails;
using DevTech.LaundryOperations.Application.CommandServices;
using DevTech.LaundryOperations.Application.QueryServices;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.Queries;
using DevTech.LaundryOperations.Domain.Model.Errors;
using DevTech.LaundryOperations.Interfaces.Rest.Resources;
using DevTech.LaundryOperations.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
namespace DevTech.LaundryOperations.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Tags("Laundry resources")]
public class LaundryResourcesController(ILaundryResourceCommandService commandService,
    ILaundryResourceQueryService queryService, ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get resources belonging to a laundry")]
    [ProducesResponseType(typeof(IEnumerable<LaundryResourceResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] Guid laundryId, CancellationToken cancellationToken)
    {
        if (laundryId == Guid.Empty) return Problem(statusCode: 400, title: "laundryId is required.");
        var resources = await queryService.Handle(new GetAllLaundryResourcesByLaundryIdQuery(laundryId), cancellationToken);
        return Ok(resources.Select(LaundryResourceResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("{id:guid}/release")]
    [EndpointSummary("Release an unreserved busy resource")]
    [ProducesResponseType(typeof(LaundryResourceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Release(Guid id, CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new ReleaseResourceCommand(id), cancellationToken);
        return LaundryOperationsActionResultAssembler.ToActionResultFromReleaseResourceResult(this, result, problemDetailsFactory,
            resource => Ok(LaundryResourceResourceFromEntityAssembler.ToResourceFromEntity(resource)));
    }

    [HttpPut("{id:guid}/status")]
    [EndpointSummary("Synchronize the monitored resource status")]
    [ProducesResponseType(typeof(LaundryResourceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Sync(Guid id, SynchronizeResourceResource resource, CancellationToken cancellationToken)
    {
        var command = SyncResourceStatusCommandFromResourceAssembler.ToCommandFromResource(resource, id);
        var result = await commandService.Handle(command, cancellationToken);
        return LaundryOperationsActionResultAssembler.ToActionResultFromSyncResourceStatusResult(this, result, problemDetailsFactory,
            foundResource => Ok(LaundryResourceResourceFromEntityAssembler.ToResourceFromEntity(foundResource)));
    }
}

