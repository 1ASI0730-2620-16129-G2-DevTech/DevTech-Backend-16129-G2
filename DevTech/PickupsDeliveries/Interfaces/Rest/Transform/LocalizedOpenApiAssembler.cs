using System.Reflection;
using DevTech.PickupsDeliveries.Interfaces.Rest.Resources;
using DevTech.Resources;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;

namespace DevTech.PickupsDeliveries.Interfaces.Rest.Transform;

/// <summary>
///     Assembles localized OpenAPI metadata (summaries, descriptions, and response texts)
///     for the Pickups and Deliveries REST endpoints and resources.
/// </summary>
public static class LocalizedOpenApiAssembler
{
    private static readonly Dictionary<string, (string Summary, string Description,
        IReadOnlyDictionary<string, string> Responses)> OperationResources =
        new Dictionary<string, (string, string, IReadOnlyDictionary<string, string>)>
        {
            ["SchedulePickupDelivery"] = (
                "SchedulePickupDeliverySummary",
                "SchedulePickupDeliveryDescription",
                new Dictionary<string, string>
                {
                    ["201"] = "SchedulePickupDeliveryCreated",
                    ["400"] = "SchedulePickupDeliveryBadRequest",
                    ["404"] = "SchedulePickupDeliveryNotFound",
                    ["409"] = "SchedulePickupDeliveryConflict",
                    ["422"] = "SchedulePickupDeliveryUnprocessableEntity",
                    ["500"] = "SchedulePickupDeliveryServerError"
                }),
            ["GetPickupDeliveryFromQuery"] = (
                "GetPickupDeliveryQuerySummary",
                "GetPickupDeliveryQueryDescription",
                new Dictionary<string, string>
                {
                    ["200"] = "GetPickupDeliveryQuerySuccess",
                    ["400"] = "GetPickupDeliveryQueryBadRequest",
                    ["404"] = "GetPickupDeliveryQueryNotFound"
                }),
            ["GetPickupDeliveryById"] = (
                "GetPickupDeliveryByIdSummary",
                "GetPickupDeliveryByIdDescription",
                new Dictionary<string, string>
                {
                    ["200"] = "GetPickupDeliveryByIdSuccess",
                    ["404"] = "GetPickupDeliveryByIdNotFound"
                })
        };

    /// <summary>
    ///     Localizes the summary, description, operation id and response descriptions of known operations.
    /// </summary>
    /// <param name="operation">The OpenAPI operation being transformed.</param>
    /// <param name="context">The transformer context, used to resolve the localizer and action descriptor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    public static Task TransformOperationAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor action ||
            !OperationResources.TryGetValue(action.ActionName, out var resources))
            return Task.CompletedTask;

        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<SharedResource>>();
        operation.OperationId = action.ActionName;
        operation.Summary = localizer[resources.Summary].Value;
        operation.Description = localizer[resources.Description].Value;
        if (operation.Responses is null) return Task.CompletedTask;

        foreach (var (statusCode, resourceKey) in resources.Responses)
        {
            if (operation.Responses.TryGetValue(statusCode, out var response) &&
                response is OpenApiResponse openApiResponse)
                openApiResponse.Description = localizer[resourceKey].Value;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Localizes the description of known resource schemas and their properties.
    /// </summary>
    /// <remarks>
    ///     Only the resources of this bounded context are changed: other bounded contexts also have
    ///     properties such as "id" or "status", and their descriptions must stay untouched.
    /// </remarks>
    /// <param name="schema">The OpenAPI schema being transformed.</param>
    /// <param name="context">The transformer context, used to resolve the localizer and type information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    public static Task TransformSchemaAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<SharedResource>>();
        var type = context.JsonTypeInfo.Type;
        if (type == typeof(SchedulePickupDeliveryResource))
            schema.Description = localizer["SchedulePickupDeliverySchema"].Value;
        else if (type == typeof(PickupDeliveryResource))
            schema.Description = localizer["PickupDeliverySchema"].Value;

        var declaringType = (context.JsonPropertyInfo?.AttributeProvider as MemberInfo)?.DeclaringType;
        var isPropertyOfThisBoundedContext = declaringType == typeof(SchedulePickupDeliveryResource)
                                             || declaringType == typeof(PickupDeliveryResource);
        if (!isPropertyOfThisBoundedContext)
            return Task.CompletedTask;

        var propertyResourceKey = context.JsonPropertyInfo?.Name switch
        {
            "id" => "PickupDeliveryId",
            "orderId" => "PickupDeliveryOrderId",
            "street" => "PickupDeliveryStreet",
            "city" => "PickupDeliveryCity",
            "type" => "PickupDeliveryType",
            "status" => "PickupDeliveryStatus",
            _ => null
        };

        if (propertyResourceKey is not null)
            schema.Description = localizer[propertyResourceKey].Value;

        return Task.CompletedTask;
    }
}
