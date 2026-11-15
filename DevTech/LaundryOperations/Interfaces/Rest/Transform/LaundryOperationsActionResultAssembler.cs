using DevTech.Shared.Application.Model;
using DevTech.Shared.Domain.Model.Errors;
using DevTech.Shared.Interfaces.Rest.ProblemDetails;
using DevTech.LaundryOperations.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
namespace DevTech.LaundryOperations.Interfaces.Rest.Transform;

public static class LaundryOperationsActionResultAssembler
{
    public static IActionResult ToActionResultFromError(ControllerBase controller, Error error, ProblemDetailsFactory factory)
    {
        var status = error.Type switch
        {
            EErrorType.Validation => StatusCodes.Status400BadRequest,
            EErrorType.NotFound => StatusCodes.Status404NotFound,
            EErrorType.Conflict => StatusCodes.Status409Conflict,
            EErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            EErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
        return factory.CreateProblemDetails(controller, status, error);
    }
    public static IActionResult ToActionResultFromReceiveLaundryOrderResult(ControllerBase controller,
        Result<LaundryOrder> result, ProblemDetailsFactory factory, Func<LaundryOrder, IActionResult> successAction)
        => result.Match(successAction, error => ToActionResultFromError(controller, error, factory));

    public static IActionResult ToActionResultFromClassifyLaundryOrderResult(ControllerBase controller,
        Result<LaundryOrder> result, ProblemDetailsFactory factory, Func<LaundryOrder, IActionResult> successAction)
        => result.Match(successAction, error => ToActionResultFromError(controller, error, factory));

    public static IActionResult ToActionResultFromPrioritizeLaundryOrderResult(ControllerBase controller,
        Result<LaundryOrder> result, ProblemDetailsFactory factory, Func<LaundryOrder, IActionResult> successAction)
        => result.Match(successAction, error => ToActionResultFromError(controller, error, factory));

    public static IActionResult ToActionResultFromAssignWashingCycleResult(ControllerBase controller,
        Result<LaundryOrder> result, ProblemDetailsFactory factory, Func<LaundryOrder, IActionResult> successAction)
        => result.Match(successAction, error => ToActionResultFromError(controller, error, factory));

    public static IActionResult ToActionResultFromAssignResourceResult(ControllerBase controller,
        Result<LaundryOrder> result, ProblemDetailsFactory factory, Func<LaundryOrder, IActionResult> successAction)
        => result.Match(successAction, error => ToActionResultFromError(controller, error, factory));

    public static IActionResult ToActionResultFromAdvanceStageResult(ControllerBase controller,
        Result<LaundryOrder> result, ProblemDetailsFactory factory, Func<LaundryOrder, IActionResult> successAction)
        => result.Match(successAction, error => ToActionResultFromError(controller, error, factory));

    public static IActionResult ToActionResultFromReleaseResourceResult(ControllerBase controller,
        Result<LaundryResource> result, ProblemDetailsFactory factory, Func<LaundryResource, IActionResult> successAction)
        => result.Match(successAction, error => ToActionResultFromError(controller, error, factory));

    public static IActionResult ToActionResultFromSyncResourceStatusResult(ControllerBase controller,
        Result<LaundryResource> result, ProblemDetailsFactory factory, Func<LaundryResource, IActionResult> successAction)
        => result.Match(successAction, error => ToActionResultFromError(controller, error, factory));

}

