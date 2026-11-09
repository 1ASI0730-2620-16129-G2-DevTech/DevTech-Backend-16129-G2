using System.Net.Mime;
using Acme.Center.Platform.Shared.Interfaces.Rest.ProblemDetails;
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
[Route("api/v1/customers/{customerId:int}/subscription")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Subscription endpoints")]
public class CustomerSubscriptionController(
    ISubscriptionQueryService subscriptionQueryService,
    IStringLocalizer<SubscriptionPaymentMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get the subscription of a customer")]
    [EndpointDescription("Get the most recent subscription of a customer, whatever its status")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerSubscription([FromRoute] int customerId,
        CancellationToken cancellationToken)
    {
        var getSubscriptionByCustomerIdQuery = new GetSubscriptionByCustomerIdQuery(customerId);
        var subscription = await subscriptionQueryService.Handle(getSubscriptionByCustomerIdQuery, cancellationToken);

        return SubscriptionPaymentActionResultAssembler.ToActionResultFromQueryResult(
            this,
            subscription,
            SubscriptionPaymentError.SubscriptionNotFound,
            SubscriptionPaymentErrors.CustomerSubscriptionNotFound,
            localizer,
            problemDetailsFactory,
            foundSubscription => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(foundSubscription))
        );
    }
}
