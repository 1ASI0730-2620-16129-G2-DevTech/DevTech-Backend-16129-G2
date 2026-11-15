using DevTech.Shared.Application.Model;
using DevTech.Shared.Domain.Model.Errors;
using DevTech.Shared.Interfaces.Rest.ProblemDetails;
using DevTech.SubscriptionPayment.Domain.Model.Errors;
using DevTech.SubscriptionPayment.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DevTech.SubscriptionPayment.Interfaces.Rest.Transform;

/// <summary>
///     Transforms application results of the Subscription &amp; Payment bounded context into HTTP responses.
/// </summary>
public static class SubscriptionPaymentActionResultAssembler
{
    // --- Helper for transforming SubscriptionPaymentError to StatusCode ---
    private static int ToStatusCodeFromSubscriptionPaymentError(SubscriptionPaymentError error)
    {
        return error switch
        {
            SubscriptionPaymentError.SubscriptionPlanNotFound => StatusCodes.Status404NotFound,
            SubscriptionPaymentError.SubscriptionNotFound => StatusCodes.Status404NotFound,
            SubscriptionPaymentError.PaymentNotFound => StatusCodes.Status404NotFound,
            SubscriptionPaymentError.InvalidSubscriptionPlanData => StatusCodes.Status400BadRequest,
            SubscriptionPaymentError.InvalidSubscriptionData => StatusCodes.Status400BadRequest,
            SubscriptionPaymentError.InvalidPaymentData => StatusCodes.Status400BadRequest,
            SubscriptionPaymentError.ActiveSubscriptionAlreadyExists => StatusCodes.Status409Conflict,
            SubscriptionPaymentError.SubscriptionNotActive => StatusCodes.Status409Conflict,
            SubscriptionPaymentError.InsufficientCredits => StatusCodes.Status409Conflict,
            SubscriptionPaymentError.InvalidPaymentStatus => StatusCodes.Status409Conflict,
            SubscriptionPaymentError.PaymentRejected => StatusCodes.Status402PaymentRequired,
            SubscriptionPaymentError.PaymentGatewayUnavailable => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status400BadRequest // Default
        };
    }

    // --- Helper for building a ProblemDetails response with texts in the language of the request ---
    private static IActionResult ToLocalizedProblemDetails(
        ControllerBase controller,
        SubscriptionPaymentError error,
        Error domainError,
        IStringLocalizer<SubscriptionPaymentMessages> localizer,
        ProblemDetailsFactory problemDetailsFactory)
    {
        // The detail is the text of this context in the language of the request.
        var localizedError = domainError with { Message = localizer[domainError.Code].Value };
        var actionResult = problemDetailsFactory.CreateProblemDetails(
            controller,
            ToStatusCodeFromSubscriptionPaymentError(error),
            localizedError);

        // The shared factory only knows the titles of ErrorMessages; this context keeps its own texts.
        if (actionResult is ObjectResult { Value: Microsoft.AspNetCore.Mvc.ProblemDetails problemDetails })
            problemDetails.Title = localizer[$"Title.{error}"].Value;

        return actionResult;
    }

    /// <summary>
    ///     Transforms a command <see cref="Result{T}" /> into an HTTP response.
    /// </summary>
    public static IActionResult ToActionResultFromResult<T>(
        ControllerBase controller,
        Result<T> result,
        IStringLocalizer<SubscriptionPaymentMessages> localizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        return ToLocalizedProblemDetails(controller, (SubscriptionPaymentError)result.Error!, result.DomainError,
            localizer, problemDetailsFactory);
    }

    /// <summary>
    ///     Transforms a query result (entity or null) into an HTTP response.
    /// </summary>
    public static IActionResult ToActionResultFromQueryResult<T>(
        ControllerBase controller,
        T? entity,
        SubscriptionPaymentError notFoundError,
        Error notFoundDomainError,
        IStringLocalizer<SubscriptionPaymentMessages> localizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction) where T : class
    {
        if (entity is null)
            return ToLocalizedProblemDetails(controller, notFoundError, notFoundDomainError, localizer,
                problemDetailsFactory);

        return successAction(entity);
    }
}
