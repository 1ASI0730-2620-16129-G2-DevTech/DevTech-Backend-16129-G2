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
[Tags("Laundry orders")]
public class LaundryOrdersController(ILaundryOrderCommandService commandService,
    ILaundryOrderQueryService queryService, TimeProvider clock, ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get laundry orders")]
    [ProducesResponseType(typeof(IEnumerable<LaundryOrderResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] Guid? laundryId, CancellationToken cancellationToken)
    {
        var orders = await queryService.Handle(new GetAllLaundryOrdersQuery(), cancellationToken);
        return Ok(orders.Where(order => laundryId is null || order.LaundryId.Value == laundryId)
            .Select(order => LaundryOrderResourceFromEntityAssembler.ToResourceFromEntity(order, clock.GetUtcNow())));
    }

    [HttpGet("{id:guid}")]
    [EndpointSummary("Get a laundry order by id")]
    [ProducesResponseType(typeof(LaundryOrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var order = await queryService.Handle(new GetLaundryOrderByIdQuery(id), cancellationToken);
        return order is null
            ? LaundryOperationsActionResultAssembler.ToActionResultFromError(this, LaundryOperationsErrors.LaundryOrderNotFound, problemDetailsFactory)
            : Ok(LaundryOrderResourceFromEntityAssembler.ToResourceFromEntity(order, clock.GetUtcNow()));
    }

    [HttpPost]
    [EndpointSummary("Receive an order from Order Management")]
    [ProducesResponseType(typeof(LaundryOrderResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Receive(ReceiveLaundryOrderResource resource, CancellationToken cancellationToken)
    {
        var command = ReceiveLaundryOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);
        return LaundryOperationsActionResultAssembler.ToActionResultFromReceiveLaundryOrderResult(this, result, problemDetailsFactory,
            order => CreatedAtAction(nameof(GetById), new { id = order.Id.Value },
                LaundryOrderResourceFromEntityAssembler.ToResourceFromEntity(order, clock.GetUtcNow())));
    }

    [HttpPost("{id:guid}/classification")]
    [EndpointSummary("Classify a received order")]
    [ProducesResponseType(typeof(LaundryOrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Classify(Guid id, CancellationToken cancellationToken)
    {
        var command = new ClassifyLaundryOrderCommand(id);
        var result = await commandService.Handle(command, cancellationToken);
        return LaundryOperationsActionResultAssembler.ToActionResultFromClassifyLaundryOrderResult(this, result, problemDetailsFactory,
            order => Ok(LaundryOrderResourceFromEntityAssembler.ToResourceFromEntity(order, clock.GetUtcNow())));
    }

    [HttpPut("{id:guid}/priority")]
    [EndpointSummary("Change order priority")]
    [ProducesResponseType(typeof(LaundryOrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Prioritize(Guid id, PrioritizeLaundryOrderResource resource, CancellationToken cancellationToken)
    {
        var command = PrioritizeLaundryOrderCommandFromResourceAssembler.ToCommandFromResource(resource, id);
        var result = await commandService.Handle(command, cancellationToken);
        return LaundryOperationsActionResultAssembler.ToActionResultFromPrioritizeLaundryOrderResult(this, result, problemDetailsFactory,
            order => Ok(LaundryOrderResourceFromEntityAssembler.ToResourceFromEntity(order, clock.GetUtcNow())));
    }

    [HttpPut("{id:guid}/washing-cycle")]
    [EndpointSummary("Assign a washing cycle")]
    [ProducesResponseType(typeof(LaundryOrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignCycle(Guid id, AssignWashingCycleResource resource, CancellationToken cancellationToken)
    {
        var command = AssignWashingCycleCommandFromResourceAssembler.ToCommandFromResource(resource, id);
        var result = await commandService.Handle(command, cancellationToken);
        return LaundryOperationsActionResultAssembler.ToActionResultFromAssignWashingCycleResult(this, result, problemDetailsFactory,
            order => Ok(LaundryOrderResourceFromEntityAssembler.ToResourceFromEntity(order, clock.GetUtcNow())));
    }

    [HttpPut("{id:guid}/resource")]
    [EndpointSummary("Reserve a laundry resource")]
    [ProducesResponseType(typeof(LaundryOrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignResource(Guid id, AssignLaundryResourceResource resource, CancellationToken cancellationToken)
    {
        var command = AssignResourceCommandFromResourceAssembler.ToCommandFromResource(resource, id);
        var result = await commandService.Handle(command, cancellationToken);
        return LaundryOperationsActionResultAssembler.ToActionResultFromAssignResourceResult(this, result, problemDetailsFactory,
            order => Ok(LaundryOrderResourceFromEntityAssembler.ToResourceFromEntity(order, clock.GetUtcNow())));
    }

    [HttpPost("{id:guid}/stage-advancements")]
    [EndpointSummary("Advance to the next processing stage")]
    [ProducesResponseType(typeof(LaundryOrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Advance(Guid id, CancellationToken cancellationToken)
    {
        var command = new AdvanceStageCommand(id);
        var result = await commandService.Handle(command, cancellationToken);
        return LaundryOperationsActionResultAssembler.ToActionResultFromAdvanceStageResult(this, result, problemDetailsFactory,
            order => Ok(LaundryOrderResourceFromEntityAssembler.ToResourceFromEntity(order, clock.GetUtcNow())));
    }
}

