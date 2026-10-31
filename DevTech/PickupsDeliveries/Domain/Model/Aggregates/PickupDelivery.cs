using DevTech.PickupsDeliveries.Domain.Model.Commands;
using DevTech.PickupsDeliveries.Domain.Model.ValueObjects;

namespace DevTech.PickupsDeliveries.Domain.Model.Aggregates;

/// <summary>
///     Represents the pickup or the delivery of a laundry order at the customer's address.
/// </summary>
/// <remarks>
///     This aggregate root protects the pickup and delivery business rules:
///     <list type="bullet">
///         <item><description>It always belongs to a valid order.</description></item>
///         <item><description>It always has a valid address.</description></item>
///         <item><description>Its type is always a defined <see cref="ServiceType" /> (Pickup or Delivery).</description></item>
///         <item><description>It always starts with the <see cref="PickupDeliveryStatus.Scheduled" /> status.</description></item>
///         <item><description>Only a scheduled or in transit service can be marked as completed.</description></item>
///     </list>
///     The combination of (OrderId, Type) must be unique within the system.
///     Properties have private setters so their values can only change through the aggregate behavior.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class PickupDelivery
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected PickupDelivery()
    {
        Address = null!;
    }

    /// <summary>
    ///     Creates a new PickupDelivery from a schedule command.
    /// </summary>
    /// <remarks>
    ///     This constructor acts as the command handler for SchedulePickupDeliveryCommand.
    ///     It initializes the aggregate with the order identifier, the address, the service type
    ///     and the initial Scheduled status.
    /// </remarks>
    /// <param name="command">The SchedulePickupDeliveryCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when command or its address is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the order identifier is zero or negative, or when the service type is not defined.
    /// </exception>
    public PickupDelivery(SchedulePickupDeliveryCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Address);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.OrderId);

        if (!Enum.IsDefined(command.Type))
            throw new ArgumentOutOfRangeException(nameof(command), command.Type, "The service type is not defined.");

        OrderId = command.OrderId;
        Address = command.Address;
        Type = command.Type;
        Status = PickupDeliveryStatus.Scheduled;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this pickup or delivery.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the related order.
    /// </summary>
    public int OrderId { get; private set; }

    /// <summary>
    ///     Gets the address where the service takes place.
    /// </summary>
    public DeliveryAddress Address { get; private set; }

    /// <summary>
    ///     Gets the type of service: Pickup or Delivery.
    /// </summary>
    public ServiceType Type { get; private set; }

    /// <summary>
    ///     Gets the current status of the pickup or delivery.
    /// </summary>
    public PickupDeliveryStatus Status { get; private set; }

    /// <summary>
    ///     Indicates whether the pickup or delivery can still be marked as completed.
    /// </summary>
    /// <returns>True when the status is Scheduled or InTransit; otherwise, false.</returns>
    public bool CanBeMarkedAsCompleted() =>
        Status is PickupDeliveryStatus.Scheduled or PickupDeliveryStatus.InTransit;

    /// <summary>
    ///     Marks the pickup or delivery as completed.
    /// </summary>
    /// <remarks>
    ///     Completed and Failed are final statuses, so a service in one of them cannot be marked as completed again.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown when the status is already Completed or Failed.</exception>
    public void MarkAsCompleted()
    {
        if (!CanBeMarkedAsCompleted())
            throw new InvalidOperationException(
                $"Pickup or delivery {Id} cannot be marked as completed because its status is {Status}.");

        Status = PickupDeliveryStatus.Completed;
    }
}