using DevTech.Shared.Domain.Model.Errors;
using DevTech.Shared.Resources.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
namespace DevTech.Shared.Interfaces.Rest.ProblemDetails;

/// <summary>Creates localized HTTP problems using the framework problem-details factory.</summary>
public class ProblemDetailsFactory(
    IStringLocalizer<ErrorMessages> errorLocalizer,
    Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory aspNetCoreProblemDetailsFactory)
{
    public IActionResult CreateProblemDetails(ControllerBase controller, int statusCode, Error error)
    {
        var message = errorLocalizer[error.Code];
        var problem = aspNetCoreProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext, statusCode, error.Code,
            detail: message.ResourceNotFound ? error.Message : message.Value,
            instance: controller.HttpContext.Request.Path);
        problem.Extensions["code"] = error.Code;
        return controller.StatusCode(statusCode, problem);
    }
}

