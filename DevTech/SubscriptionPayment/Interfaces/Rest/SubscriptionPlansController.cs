using System.Net.Mime;
using Acme.Center.Platform.Shared.Interfaces.Rest.ProblemDetails;
using DevTech.SubscriptionPayment.Application.CommandServices;
using DevTech.SubscriptionPayment.Application.QueryServices;
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
[Tags("Available Subscription Plan endpoints")]
public class SubscriptionPlansController(
    ISubscriptionPlanCommandService subscriptionPlanCommandService,
    ISubscriptionPlanQueryService subscriptionPlanQueryService,
    IStringLocalizer<SubscriptionPaymentMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all subscription plans")]
    [EndpointDescription("Get all the subscription plans a customer can subscribe to")]
    [ProducesResponseType(typeof(IEnumerable<SubscriptionPlanResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSubscriptionPlans(CancellationToken cancellationToken)
    {
        var getAllSubscriptionPlansQuery = new GetAllSubscriptionPlansQuery();
        var subscriptionPlans = await subscriptionPlanQueryService.Handle(getAllSubscriptionPlansQuery, cancellationToken);
        var subscriptionPlanResources =
            subscriptionPlans.Select(SubscriptionPlanResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(subscriptionPlanResources);
    }

    [HttpGet("{subscriptionPlanId:int}")]
    [EndpointSummary("Get a subscription plan by its id")]
    [EndpointDescription("Get a subscription plan by its id")]
    [ProducesResponseType(typeof(SubscriptionPlanResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionPlanById([FromRoute] int subscriptionPlanId,
        CancellationToken cancellationToken)
    {
        var getSubscriptionPlanByIdQuery = new GetSubscriptionPlanByIdQuery(subscriptionPlanId);
        var subscriptionPlan = await subscriptionPlanQueryService.Handle(getSubscriptionPlanByIdQuery, cancellationToken);

        return SubscriptionPaymentActionResultAssembler.ToActionResultFromQueryResult(
            this,
            subscriptionPlan,
            SubscriptionPaymentError.SubscriptionPlanNotFound,
            SubscriptionPaymentErrors.SubscriptionPlanNotFound,
            localizer,
            problemDetailsFactory,
            foundPlan => Ok(SubscriptionPlanResourceFromEntityAssembler.ToResourceFromEntity(foundPlan))
        );
    }

    [HttpPost]
    [EndpointSummary("Create a subscription plan")]
    [EndpointDescription("Create a subscription plan with its price, credits and duration in days")]
    [ProducesResponseType(typeof(SubscriptionPlanResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSubscriptionPlan([FromBody] CreateSubscriptionPlanResource resource,
        CancellationToken cancellationToken)
    {
        var createSubscriptionPlanCommand =
            CreateSubscriptionPlanCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await subscriptionPlanCommandService.Handle(createSubscriptionPlanCommand, cancellationToken);

        return SubscriptionPaymentActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            localizer,
            problemDetailsFactory,
            createdPlan => CreatedAtAction(nameof(GetSubscriptionPlanById),
                new { subscriptionPlanId = createdPlan.Id.Value },
                SubscriptionPlanResourceFromEntityAssembler.ToResourceFromEntity(createdPlan))
        );
    }
}
