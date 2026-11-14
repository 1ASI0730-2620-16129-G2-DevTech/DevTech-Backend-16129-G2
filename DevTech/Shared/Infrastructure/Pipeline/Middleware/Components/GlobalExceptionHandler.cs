using DevTech.Shared.Resources.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MySql.Data.MySqlClient;
namespace DevTech.Shared.Infrastructure.Pipeline.Middleware.Components;

/// <summary>Writes localized ProblemDetails through the ASP.NET Core problem-details service.</summary>
public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService, IStringLocalizer<ErrorMessages> errorLocalizer) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is OperationCanceledException && context.RequestAborted.IsCancellationRequested)
        {
            context.Response.StatusCode = 499;
            return true;
        }
        var (status, key) = exception switch
        {
            DbUpdateConcurrencyException => (409, "ConcurrentUpdate"),
            DbUpdateException { InnerException: MySqlException { Number: 1062 } } => (409, "UniqueConflict"),
            ArgumentException => (400, "InvalidInput"),
            OperationCanceledException => (409, "OperationCancelled"),
            _ => (500, "GenericError")
        };
        logger.LogError(exception, "Request failed with status {Status}", status);
        context.Response.StatusCode = status;
        var problem = new ProblemDetails
        {
            Status = status, Title = errorLocalizer[key], Detail = errorLocalizer[key],
            Instance = context.Request.Path, Extensions = { ["traceId"] = context.TraceIdentifier }
        };
        if (!await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            { HttpContext = context, ProblemDetails = problem, Exception = exception }))
            await context.Response.WriteAsJsonAsync(problem, cancellationToken: ct);
        return true;
    }
}

