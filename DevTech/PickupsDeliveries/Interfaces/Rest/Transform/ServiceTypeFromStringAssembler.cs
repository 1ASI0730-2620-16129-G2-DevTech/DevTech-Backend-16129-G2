using DevTech.PickupsDeliveries.Domain.Model.ValueObjects;

namespace DevTech.PickupsDeliveries.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a ServiceType from the text received in a request.
/// </summary>
/// <remarks>
///     It is used by the schedule command assembler and by the query endpoint,
///     so the text is converted in only one place.
/// </remarks>
public static class ServiceTypeFromStringAssembler
{
    /// <summary>
    ///     Converts a text such as "Pickup" or "delivery" into a ServiceType.
    /// </summary>
    /// <remarks>
    ///     The text is compared with the name of each ServiceType value, ignoring case and outer spaces.
    ///     Numbers such as "0" are not accepted, only the names.
    /// </remarks>
    /// <param name="type">The text received in the request.</param>
    /// <returns>The matching ServiceType.</returns>
    /// <exception cref="ArgumentException">Thrown when the text is not Pickup or Delivery.</exception>
    public static ServiceType ToServiceTypeFromString(string? type)
    {
        foreach (var serviceType in Enum.GetValues<ServiceType>())
        {
            if (string.Equals(serviceType.ToString(), type?.Trim(), StringComparison.OrdinalIgnoreCase))
                return serviceType;
        }

        throw new ArgumentException($"The service type '{type}' is not valid.", nameof(type));
    }
}
