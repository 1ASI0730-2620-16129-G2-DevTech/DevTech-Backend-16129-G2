using DevTech.Shared.Application.Model;
using DevTech.Shared.Interfaces.Rest.ProblemDetails;
using DevTech.OrderManagement.Application.Errors;
using DevTech.OrderManagement.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;

namespace DevTech.OrderManagement.Interfaces.REST.Transform;

/// <summary>
///     Translates the outcome of an Order Management use case into an HTTP response.
/// </summary>
/// <param name="problemDetailsFactory">Shared factory that builds localized RFC 7807 responses.</param>
public class ActionResultFromOrderResultAssembler(ProblemDetailsFactory problemDetailsFactory)
{
    /// <summary>
    ///     Success becomes <c>200 OK</c> with the order; a failure becomes a problem-details response whose status
    ///     code depends on the <see cref="OrderError" />.
    /// </summary>
public IActionResult ToActionResultFromOrderResult(Result<Order> result, ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.Ok(OrderResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));

        var error = result.Error as OrderError?;
        return problemDetailsFactory.CreateProblemDetails(
            controller, ToStatusCode(error), result.DomainError);
    }

    private static int ToStatusCode(OrderError? error)
    {
        return error switch
        {
            OrderError.InvalidCustomer
                or OrderError.InvalidLaundry
                or OrderError.InvalidEstimatedDeliveryDate
                or OrderError.InvalidSpecialCareInstructions
                or OrderError.InvalidGarmentItem => StatusCodes.Status400BadRequest,
            OrderError.GarmentItemsLockedAfterCreation
                or OrderError.InvalidStatusTransition => StatusCodes.Status409Conflict,
            OrderError.OrderNotFound
                or OrderError.GarmentItemNotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
