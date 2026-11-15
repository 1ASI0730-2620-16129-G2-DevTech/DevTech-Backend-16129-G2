using System.Net.Mime;
using DevTech.TrackingNotifications.Application.CommandServices;
using DevTech.TrackingNotifications.Application.QueryServices;
using DevTech.TrackingNotifications.Domain.Model.Commands;
using DevTech.TrackingNotifications.Domain.Model.Queries;
using DevTech.TrackingNotifications.Interfaces.Rest.Resources;
using DevTech.TrackingNotifications.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace DevTech.TrackingNotifications.Interfaces.Rest;

/// <summary>
///     Tracking of the orders. The stages are recorded by the system when Laundry Operations advances
///     an order (StageAdvanced event), so this controller only starts and reads trackings.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Tracking endpoints")]
public class TrackingsController(
    IOrderTrackingCommandService orderTrackingCommandService,
    IOrderTrackingQueryService orderTrackingQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all trackings")]
    [EndpointDescription("Get the trackings with their stage history, newest first. Use orderId to get only the tracking of an order")]
    [ProducesResponseType(typeof(IEnumerable<TrackingResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTrackings([FromQuery] Guid? orderId, CancellationToken cancellationToken)
    {
        var trackings = await orderTrackingQueryService.Handle(new GetAllTrackingsQuery(orderId), cancellationToken);
        return Ok(trackings.Select(TrackingResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{trackingId:int}")]
    [EndpointSummary("Get a tracking by its id")]
    [ProducesResponseType(typeof(TrackingResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTrackingById([FromRoute] int trackingId, CancellationToken cancellationToken)
    {
        var tracking = await orderTrackingQueryService.Handle(new GetTrackingByIdQuery(trackingId), cancellationToken);
        return tracking is null
            ? NotFound()
            : Ok(TrackingResourceFromEntityAssembler.ToResourceFromEntity(tracking));
    }

    [HttpPost]
    [EndpointSummary("Start the tracking of an order")]
    [EndpointDescription("Start the tracking of an order at the RECEIVED stage. If the order already has a tracking, it is returned (and its estimated delivery is kept up to date)")]
    [ProducesResponseType(typeof(TrackingResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartTracking([FromBody] StartTrackingResource resource,
        CancellationToken cancellationToken)
    {
        if (resource.OrderId == Guid.Empty)
            return Problem("The order identifier is required.", statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid tracking data");

        var tracking = await orderTrackingCommandService.Handle(
            new StartTrackingCommand(resource.OrderId, resource.EstimatedDelivery), cancellationToken);
        return CreatedAtAction(nameof(GetTrackingById), new { trackingId = tracking.Id },
            TrackingResourceFromEntityAssembler.ToResourceFromEntity(tracking));
    }
}
