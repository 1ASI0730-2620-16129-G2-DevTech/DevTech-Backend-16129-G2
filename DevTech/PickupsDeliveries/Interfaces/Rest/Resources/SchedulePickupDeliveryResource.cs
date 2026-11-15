using System.ComponentModel.DataAnnotations;

namespace DevTech.PickupsDeliveries.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to schedule a pickup or a delivery.
/// </summary>
/// <remarks>
///     The ErrorMessage values are keys of the SharedResource files, so the validation messages are localized.
///     An empty OrderId is rejected by the command service with a localized message.
/// </remarks>
/// <param name="OrderId">The identifier of the order in Order Management (required)</param>
/// <param name="Street">The street of the address (required)</param>
/// <param name="City">The city of the address (required)</param>
/// <param name="Type">The type of service: Pickup or Delivery (required)</param>
public record SchedulePickupDeliveryResource(
    Guid OrderId,
    [Required(ErrorMessage = "RequiredStreet")]
    string Street,
    [Required(ErrorMessage = "RequiredCity")]
    string City,
    [Required(ErrorMessage = "RequiredServiceType")]
    string Type);
