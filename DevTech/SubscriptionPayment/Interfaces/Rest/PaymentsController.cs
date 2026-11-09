using System.Net.Mime;
using Acme.Center.Platform.Shared.Interfaces.Rest.ProblemDetails;
using DevTech.SubscriptionPayment.Application.CommandServices;
using DevTech.SubscriptionPayment.Application.QueryServices;
using DevTech.SubscriptionPayment.Domain.Model.Commands;
using DevTech.SubscriptionPayment.Domain.Model.Errors;
using DevTech.SubscriptionPayment.Domain.Model.Queries;
using DevTech.SubscriptionPayment.Interfaces.Rest.Resources;
using DevTech.SubscriptionPayment.Interfaces.Rest.Transform;
using DevTech.SubscriptionPayment.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DevTech.SubscriptionPayment.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Payment endpoints")]
public class PaymentsController(
    IPaymentCommandService paymentCommandService,
    IPaymentQueryService paymentQueryService,
    IStringLocalizer<SubscriptionPaymentMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpGet("{paymentId:int}")]
    [EndpointSummary("Get a payment by its id")]
    [EndpointDescription("Get a payment by its id")]
    [ProducesResponseType(typeof(PaymentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentById([FromRoute] int paymentId, CancellationToken cancellationToken)
    {
        var getPaymentByIdQuery = new GetPaymentByIdQuery(paymentId);
        var payment = await paymentQueryService.Handle(getPaymentByIdQuery, cancellationToken);

        return SubscriptionPaymentActionResultAssembler.ToActionResultFromQueryResult(
            this,
            payment,
            SubscriptionPaymentError.PaymentNotFound,
            SubscriptionPaymentErrors.PaymentNotFound,
            localizer,
            problemDetailsFactory,
            foundPayment => Ok(PaymentResourceFromEntityAssembler.ToResourceFromEntity(foundPayment))
        );
    }

    [HttpPost]
    [EndpointSummary("Create a payment")]
    [EndpointDescription("Create a payment for an order and charge it through the payment gateway. An approved payment stays PENDING until it is confirmed")]
    [ProducesResponseType(typeof(PaymentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status402PaymentRequired)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentResource resource,
        CancellationToken cancellationToken)
    {
        var processPaymentCommand = ProcessPaymentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await paymentCommandService.Handle(processPaymentCommand, cancellationToken);

        return SubscriptionPaymentActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            localizer,
            problemDetailsFactory,
            createdPayment => CreatedAtAction(nameof(GetPaymentById), new { paymentId = createdPayment.Id.Value },
                PaymentResourceFromEntityAssembler.ToResourceFromEntity(createdPayment))
        );
    }

    [HttpPost("{paymentId:int}/confirmation")]
    [EndpointSummary("Confirm a payment")]
    [EndpointDescription("Confirm an approved payment, moving it to COMPLETED status")]
    [ProducesResponseType(typeof(PaymentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ConfirmPayment([FromRoute] int paymentId, CancellationToken cancellationToken)
    {
        var confirmPaymentCommand = new ConfirmPaymentCommand(paymentId);
        var result = await paymentCommandService.Handle(confirmPaymentCommand, cancellationToken);

        return SubscriptionPaymentActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            localizer,
            problemDetailsFactory,
            confirmedPayment => Ok(PaymentResourceFromEntityAssembler.ToResourceFromEntity(confirmedPayment))
        );
    }
}
