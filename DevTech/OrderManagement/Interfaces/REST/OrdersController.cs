using System.Net.Mime;
using DevTech.OrderManagement.Application.Services;
using DevTech.OrderManagement.Domain.Model.Aggregates;
using DevTech.OrderManagement.Domain.Model.Commands;
using DevTech.OrderManagement.Domain.Model.Queries;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.OrderManagement.Interfaces.REST.Resources;
using DevTech.OrderManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace DevTech.OrderManagement.Interfaces.REST;

/// <summary>
///     Endpoints of the Order Management bounded context. The route is <c>api/v1/orders</c>.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Orders")]
public class OrdersController(
    IOrderCommandService orderCommandService,
    IOrderQueryService orderQueryService,
    ActionResultFromOrderResultAssembler actionResultAssembler) : ControllerBase
{
    /// <summary>
    ///     Creates an order with its initial garment items.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await orderCommandService.Handle(command, cancellationToken);
        if (result.IsFailure) return actionResultAssembler.ToActionResultFromOrderResult(result, this);

        var order = OrderResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
    }

    /// <summary>
    ///     Gets an order, with its garment items, by id.
    /// </summary>
    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(OrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await orderQueryService.Handle(new GetOrderByIdQuery(new OrderId(orderId)), cancellationToken);
        if (order is null)
            return Problem(
                $"The order {orderId} was not found.",
                statusCode: StatusCodes.Status404NotFound,
                title: "Order not found");

        return Ok(OrderResourceFromEntityAssembler.ToResourceFromEntity(order));
    }

    /// <summary>
    ///     Lists orders by customer and/or laundry. At least one filter is required.
    /// </summary>
    /// <param name="customerId">Only orders of this customer.</param>
    /// <param name="laundryId">Only orders handled by this laundry.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrders([FromQuery] Guid? customerId, [FromQuery] Guid? laundryId,
        CancellationToken cancellationToken)
    {
        IEnumerable<Order> orders;

        if (customerId is not null)
        {
            orders = await orderQueryService.Handle(new GetAllOrdersByCustomerIdQuery(customerId.Value),
                cancellationToken);
            // Both filters: the customer's orders narrowed to a single laundry.
            if (laundryId is not null) orders = orders.Where(order => order.LaundryId == laundryId.Value);
        }
        else if (laundryId is not null)
        {
            orders = await orderQueryService.Handle(new GetAllOrdersByLaundryIdQuery(laundryId.Value),
                cancellationToken);
        }
        else
        {
            return Problem(
                "Provide at least one filter: customerId or laundryId.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Missing filter");
        }

        return Ok(orders.Select(OrderResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>
    ///     Adds a garment to an order. Only allowed while the order is CREATED.
    /// </summary>
    [HttpPost("{orderId:guid}/items")]
    [ProducesResponseType(typeof(OrderResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddGarmentItem(Guid orderId, [FromBody] AddGarmentItemResource resource,
        CancellationToken cancellationToken)
    {
        var command = AddGarmentItemCommandFromResourceAssembler.ToCommandFromResource(orderId, resource);
        var result = await orderCommandService.Handle(command, cancellationToken);
        if (result.IsFailure) return actionResultAssembler.ToActionResultFromOrderResult(result, this);

        var order = OrderResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
    }

    /// <summary>
    ///     Removes a garment from an order. Only allowed while the order is CREATED.
    /// </summary>
    [HttpDelete("{orderId:guid}/items/{garmentItemId:guid}")]
    [ProducesResponseType(typeof(OrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RemoveGarmentItem(Guid orderId, Guid garmentItemId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveGarmentItemCommand(new OrderId(orderId), new GarmentItemId(garmentItemId));
        var result = await orderCommandService.Handle(command, cancellationToken);
        return actionResultAssembler.ToActionResultFromOrderResult(result, this);
    }

    /// <summary>
    ///     Moves an order to its next status (CREATED → RECEIVED → CLASSIFIED → IN_PROCESS → READY → DELIVERED).
    /// </summary>
    [HttpPut("{orderId:guid}/status")]
    [ProducesResponseType(typeof(OrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeOrderStatus(Guid orderId, [FromBody] ChangeOrderStatusResource resource,
        CancellationToken cancellationToken)
    {
        var command = ChangeOrderStatusCommandFromResourceAssembler.ToCommandFromResource(orderId, resource);
        var result = await orderCommandService.Handle(command, cancellationToken);
        return actionResultAssembler.ToActionResultFromOrderResult(result, this);
    }
}
