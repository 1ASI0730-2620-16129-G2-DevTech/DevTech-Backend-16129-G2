using System.Net.Mime;
using DevTech.PickupsDeliveries.Application.Services;
using DevTech.PickupsDeliveries.Domain.Model.Queries;
using DevTech.PickupsDeliveries.Domain.Model.ValueObjects;
using DevTech.PickupsDeliveries.Interfaces.Rest.Resources;
using DevTech.PickupsDeliveries.Interfaces.Rest.Transform;
using DevTech.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DevTech.PickupsDeliveries.Interfaces.Rest;

/// <summary>
///     Pickups and deliveries' controller.
/// </summary>
/// <remarks>
///     The route is api/v1/pickups-deliveries, thanks to the kebab case naming convention.
/// </remarks>
/// <param name="pickupDeliveryCommandService">The Pickup Delivery Command Service</param>
/// <param name="pickupDeliveryQueryService">The Pickup Delivery Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Pickups and Deliveries")]
public class PickupsDeliveriesController(
    IPickupDeliveryCommandService pickupDeliveryCommandService,
    IPickupDeliveryQueryService pickupDeliveryQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<PickupsDeliveriesController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Schedules a pickup or a delivery.
    /// </summary>
    /// <param name="resource">SchedulePickupDeliveryResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the scheduled pickup or delivery, or the reason why it was not
    ///     scheduled.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(PickupDeliveryResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SchedulePickupDelivery([FromBody] SchedulePickupDeliveryResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var schedulePickupDeliveryCommand =
                SchedulePickupDeliveryCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await pickupDeliveryCommandService.Handle(schedulePickupDeliveryCommand, cancellationToken);
            return ActionResultFromSchedulePickupDeliveryResultAssembler.ToActionResultFromSchedulePickupDeliveryResult(
                result,
                this,
                localizer,
                nameof(GetPickupDeliveryById));
        }
        catch (ArgumentException exception)
        {
            logger.LogWarning(exception,
                "Validation failed while scheduling {Type} for OrderId {OrderId}",
                resource.Type,
                resource.OrderId);
            return BadRequest(localizer["InvalidPickupDeliveryRequest"].Value);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                "Unexpected error while scheduling {Type} for OrderId {OrderId}",
                resource.Type,
                resource.OrderId);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorSchedulingPickupDelivery"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    ///     Gets all the pickups and deliveries of an order.
    /// </summary>
    /// <param name="orderId">The identifier of the order</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the pickups and deliveries (an empty list if none exist).
    /// </returns>
    private async Task<ActionResult> GetAllPickupDeliveriesByOrderId(Guid orderId,
        CancellationToken cancellationToken)
    {
        var getAllPickupDeliveriesByOrderIdQuery = new GetAllPickupDeliveriesByOrderIdQuery(orderId);
        var result = await pickupDeliveryQueryService.Handle(getAllPickupDeliveriesByOrderIdQuery, cancellationToken);
        var resources = result.Select(PickupDeliveryResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets the pickup or the delivery of an order.
    /// </summary>
    /// <param name="orderId">The identifier of the order</param>
    /// <param name="type">The type of service: Pickup or Delivery</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the pickup or delivery, or not found if it does not exist.
    /// </returns>
    private async Task<ActionResult> GetPickupDeliveryByOrderIdAndType(Guid orderId, ServiceType type,
        CancellationToken cancellationToken)
    {
        var getPickupDeliveryByOrderIdAndTypeQuery = new GetPickupDeliveryByOrderIdAndTypeQuery(orderId, type);
        var result =
            await pickupDeliveryQueryService.Handle(getPickupDeliveryByOrderIdAndTypeQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = PickupDeliveryResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Gets pickups and deliveries according to parameters.
    /// </summary>
    /// <param name="orderId">The identifier of the order (required, not empty)</param>
    /// <param name="type">The type of service: Pickup or Delivery (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response with the pickup or delivery of the order if the type is provided,
    ///     or a response with all the pickups and deliveries of the order if the type is not present.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PickupDeliveryResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetPickupDeliveryFromQuery([FromQuery] Guid orderId,
        [FromQuery] string? type = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("The order identifier cannot be empty.", nameof(orderId));
            return !Request.Query.ContainsKey(nameof(type))
                ? await GetAllPickupDeliveriesByOrderId(orderId, cancellationToken)
                : await GetPickupDeliveryByOrderIdAndType(orderId,
                    ServiceTypeFromStringAssembler.ToServiceTypeFromString(type), cancellationToken);
        }
        catch (ArgumentException exception)
        {
            logger.LogWarning(exception,
                "Validation failed while querying pickups and deliveries for OrderId {OrderId} and Type {Type}",
                orderId,
                type);
            return BadRequest(localizer["InvalidPickupDeliveryQuery"].Value);
        }
    }

    /// <summary>
    ///     Gets a pickup or a delivery by id.
    /// </summary>
    /// <param name="id">The Pickup Delivery ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the pickup or delivery, or not found if it does not exist.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PickupDeliveryResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetPickupDeliveryById(int id, CancellationToken cancellationToken = default)
    {
        var getPickupDeliveryByIdQuery = new GetPickupDeliveryByIdQuery(id);
        var result = await pickupDeliveryQueryService.Handle(getPickupDeliveryByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = PickupDeliveryResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
}
