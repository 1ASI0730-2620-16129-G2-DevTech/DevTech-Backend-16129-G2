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
[Tags("Available Subscription endpoints")]
public class SubscriptionsController(
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryService subscriptionQueryService,
    IStringLocalizer<SubscriptionPaymentMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpGet("{subscriptionId:int}")]
    [EndpointSummary("Get a subscription by its id")]
    [EndpointDescription("Get a subscription by its id")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionById([FromRoute] int subscriptionId,
        CancellationToken cancellationToken)
    {
        var getSubscriptionByIdQuery = new GetSubscriptionByIdQuery(subscriptionId);
        var subscription = await subscriptionQueryService.Handle(getSubscriptionByIdQuery, cancellationToken);

        return SubscriptionPaymentActionResultAssembler.ToActionResultFromQueryResult(
            this,
            subscription,
            SubscriptionPaymentError.SubscriptionNotFound,
            SubscriptionPaymentErrors.SubscriptionNotFound,
            localizer,
            problemDetailsFactory,
            foundSubscription => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(foundSubscription))
        );
    }

    [HttpPost]
    [EndpointSummary("Subscribe a customer to a plan")]
    [EndpointDescription("Create an active subscription for a customer using the credits and duration of the plan")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Subscribe([FromBody] CreateSubscriptionResource resource,
        CancellationToken cancellationToken)
    {
        var createSubscriptionCommand = CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await subscriptionCommandService.Handle(createSubscriptionCommand, cancellationToken);

        return SubscriptionPaymentActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            localizer,
            problemDetailsFactory,
            createdSubscription => CreatedAtAction(nameof(GetSubscriptionById),
                new { subscriptionId = createdSubscription.Id.Value },
                SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(createdSubscription))
        );
    }

    [HttpPost("validations")]
    [EndpointSummary("Validate the active subscription of a customer")]
    [EndpointDescription("Validate the active subscription of a customer. It becomes EXPIRED if its expiration date has passed")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ValidateSubscription([FromBody] ValidateSubscriptionResource resource,
        CancellationToken cancellationToken)
    {
        var validateSubscriptionCommand =
            ValidateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await subscriptionCommandService.Handle(validateSubscriptionCommand, cancellationToken);

        return SubscriptionPaymentActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            localizer,
            problemDetailsFactory,
            validatedSubscription => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(validatedSubscription))
        );
    }

    [HttpPost("{subscriptionId:int}/cancellation")]
    [EndpointSummary("Cancel a subscription")]
    [EndpointDescription("Cancel an active subscription")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelSubscription([FromRoute] int subscriptionId,
        CancellationToken cancellationToken)
    {
        var cancelSubscriptionCommand = new CancelSubscriptionCommand(subscriptionId);
        var result = await subscriptionCommandService.Handle(cancelSubscriptionCommand, cancellationToken);

        return SubscriptionPaymentActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            localizer,
            problemDetailsFactory,
            cancelledSubscription => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(cancelledSubscription))
        );
    }
}
