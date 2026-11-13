using DevTech.PickupsDeliveries.Application.Errors;
using DevTech.PickupsDeliveries.Domain.Model.Aggregates;
using DevTech.Resources;
using DevTech.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DevTech.PickupsDeliveries.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to schedule a pickup or a delivery
///     in the application layer.
/// </summary>
public static class ActionResultFromSchedulePickupDeliveryResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a schedule pickup or delivery operation.
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <item><description>Success: 201 Created with the location of the new resource.</description></item>
    ///         <item><description>InvalidOrderId, InvalidAddress, InvalidServiceType: 400 Bad Request.</description></item>
    ///         <item><description>OrderNotFound: 404 Not Found.</description></item>
    ///         <item><description>DuplicatePickupDelivery: 409 Conflict.</description></item>
    ///         <item><description>
    ///             OrderDeliveryMethodMismatch, OrderNotReady: 422 Unprocessable Entity
    ///             (the request is valid, but breaks a business rule).
    ///         </description></item>
    ///         <item><description>UnexpectedError: 500 Internal Server Error.</description></item>
    ///     </list>
    /// </remarks>
    /// <param name="result">The application result produced after handling the command to schedule a pickup or delivery.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getPickupDeliveryByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromSchedulePickupDeliveryResult(
        Result<PickupDelivery, SchedulePickupDeliveryError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getPickupDeliveryByIdActionName) =>
        result switch
        {
            Result<PickupDelivery, SchedulePickupDeliveryError>.Success success =>
                controller.CreatedAtAction(
                    getPickupDeliveryByIdActionName,
                    new
                    {
                        id = success.Value.Id
                    },
                    PickupDeliveryResourceFromEntityAssembler.ToResourceFromEntity(success.Value)
                ),

            Result<PickupDelivery, SchedulePickupDeliveryError>.Failure failure =>
                failure.Error switch
                {
                    SchedulePickupDeliveryError.InvalidOrderId =>
                        controller.BadRequest(localizer["InvalidPickupDeliveryOrderId"].Value),

                    SchedulePickupDeliveryError.InvalidAddress =>
                        controller.BadRequest(localizer["InvalidPickupDeliveryAddress"].Value),

                    SchedulePickupDeliveryError.InvalidServiceType =>
                        controller.BadRequest(localizer["InvalidPickupDeliveryServiceType"].Value),

                    SchedulePickupDeliveryError.OrderNotFound =>
                        controller.NotFound(localizer["PickupDeliveryOrderNotFound"].Value),

                    SchedulePickupDeliveryError.DuplicatePickupDelivery =>
                        controller.Conflict(localizer["PickupDeliveryDuplicated"].Value),

                    SchedulePickupDeliveryError.OrderDeliveryMethodMismatch =>
                        controller.UnprocessableEntity(localizer["PickupDeliveryOrderDeliveryMethodMismatch"].Value),

                    SchedulePickupDeliveryError.OrderNotReady =>
                        controller.UnprocessableEntity(localizer["PickupDeliveryOrderNotReady"].Value),

                    SchedulePickupDeliveryError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorSchedulingPickupDelivery"].Value,
                            statusCode: StatusCodes.Status500InternalServerError),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                        statusCode: StatusCodes.Status500InternalServerError)
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: StatusCodes.Status500InternalServerError)
        };
}
