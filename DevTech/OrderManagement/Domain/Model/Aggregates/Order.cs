using DevTech.Shared.Domain.Model.Entities;
using DevTech.OrderManagement.Domain.Model.Commands;
using DevTech.OrderManagement.Domain.Model.Entities;
using DevTech.OrderManagement.Domain.Model.Events;
using DevTech.OrderManagement.Domain.Model.Exceptions;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Aggregates.Internal;
using DevTech.Shared.Domain.Model.Events;

namespace DevTech.OrderManagement.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root of the Order Management bounded context.
/// </summary>
/// <remarks>
///     Domain rules:
///     <list type="bullet">
///         <item><description>An order is created with at least one garment item and never drops below one.</description></item>
///         <item><description>Garment items can only be added or removed while the order is <c>CREATED</c>.</description></item>
///         <item>
///             <description>
///                 The status only moves forward, one step at a time:
///                 CREATED → RECEIVED → CLASSIFIED → IN_PROCESS → READY → DELIVERED.
///             </description>
///         </item>
///         <item><description><see cref="CustomerId" /> and <see cref="LaundryId" /> are references by id only.</description></item>
///     </list>
/// </remarks>
public class Order : AggregateRoot, IAuditableEntity
{
    private readonly List<GarmentItem> _items = [];

    /// <summary>
    ///     Parameterless constructor required by EF Core.
    /// </summary>
    private Order()
    {
    }

    /// <summary>
    ///     Creates a new order in <see cref="OrderStatus.CREATED" /> status.
    /// </summary>
    /// <exception cref="OrderDomainException">When any creation rule is violated.</exception>
    public Order(CreateOrderCommand command)
    {
        if (command.CustomerId == Guid.Empty)
            throw new OrderDomainException(OrderRule.InvalidCustomer, "The customer id is required.");
        if (command.LaundryId == Guid.Empty)
            throw new OrderDomainException(OrderRule.InvalidLaundry, "The laundry id is required.");

        // One day of tolerance so that a date that is "today" in the customer's time zone
        // (e.g. Lima, UTC-5) is not rejected when the server clock (UTC) has already moved on.
        var oldestAcceptedDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);
        if (command.EstimatedDeliveryDate < oldestAcceptedDate)
            throw new OrderDomainException(OrderRule.InvalidEstimatedDeliveryDate,
                "The estimated delivery date cannot be in the past.");

        var itemCommands = command.Items?.ToList() ?? [];
        if (itemCommands.Count == 0)
            throw new OrderDomainException(OrderRule.InvalidGarmentItem,
                "An order requires at least one garment item.");

        var special = string.IsNullOrWhiteSpace(command.SpecialCareInstructions)
            ? null
            : command.SpecialCareInstructions.Trim();
        if (special is { Length: > MaxSpecialCareInstructionsLength })
            throw new OrderDomainException(OrderRule.InvalidSpecialCareInstructions,
                $"The special care instructions cannot exceed {MaxSpecialCareInstructionsLength} characters.");

        // Items are built first so that an invalid one aborts creation before any state is assigned.
        var orderId = OrderId.New();
        var items = itemCommands
            .Select(item => new GarmentItem(item with { OrderId = orderId }))
            .ToList();

        Id = orderId;
        CustomerId = command.CustomerId;
        LaundryId = command.LaundryId;
        Status = OrderStatus.CREATED;
        DeliveryMethod = command.DeliveryMethod;
        ServiceType = command.ServiceType;
        EstimatedDeliveryDate = command.EstimatedDeliveryDate;
        SpecialCareInstructions = special;
        _items.AddRange(items);

        AddDomainEvent(new OrderCreated(Id.Value));
    }

    public const int MaxSpecialCareInstructionsLength = 500;

    public OrderId Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid LaundryId { get; private set; }
    public OrderStatus Status { get; private set; }
    public DeliveryMethod DeliveryMethod { get; private set; }
    public ServiceType ServiceType { get; private set; }
    public DateOnly EstimatedDeliveryDate { get; private set; }
    public string? SpecialCareInstructions { get; private set; }

    /// <summary>
    ///     Garments of the order. Mutated only through <see cref="AddGarmentItem" /> and <see cref="RemoveGarmentItem" />.
    /// </summary>
    public IReadOnlyCollection<GarmentItem> Items => _items.AsReadOnly();

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    ///     Adds a garment to the order.
    /// </summary>
    /// <exception cref="OrderDomainException">When the order is no longer <c>CREATED</c> or the garment is invalid.</exception>
    public void AddGarmentItem(AddGarmentItemCommand command)
    {
        EnsureItemsAreEditable();

        var item = new GarmentItem(command with { OrderId = Id });
        _items.Add(item);

        AddDomainEvent(new GarmentItemAdded(Id.Value, item.Id.Value));
    }

    /// <summary>
    ///     Removes a garment from the order.
    /// </summary>
    /// <exception cref="OrderDomainException">
    ///     When the order is no longer <c>CREATED</c>, the garment does not exist, or it is the last one.
    /// </exception>
    public void RemoveGarmentItem(GarmentItemId garmentItemId)
    {
        EnsureItemsAreEditable();

        var item = _items.FirstOrDefault(i => i.Id == garmentItemId);
        if (item is null)
            throw new OrderDomainException(OrderRule.GarmentItemNotFound,
                $"The garment item {garmentItemId.Value} does not belong to this order.");
        if (_items.Count == 1)
            throw new OrderDomainException(OrderRule.InvalidGarmentItem,
                "An order must keep at least one garment item.");

        _items.Remove(item);

        AddDomainEvent(new GarmentItemRemoved(Id.Value, garmentItemId.Value));
    }

    /// <summary>
    ///     Moves the order to the status that immediately follows the current one.
    /// </summary>
    /// <param name="newStatus">The requested status. Must be exactly the next one.</param>
    /// <exception cref="OrderDomainException">When <paramref name="newStatus" /> is not the next status.</exception>
    public void AdvanceStatus(OrderStatus newStatus)
    {
        if (newStatus != Status + 1)
            throw new OrderDomainException(OrderRule.InvalidStatusTransition,
                $"The order cannot move from {Status} to {newStatus}. Statuses only advance one step at a time.");

        Status = newStatus;
        AddDomainEvent(CreateEventFor(newStatus));
    }

    private void EnsureItemsAreEditable()
    {
        if (Status != OrderStatus.CREATED)
            throw new OrderDomainException(OrderRule.GarmentItemsLocked,
                $"Garment items can only be changed while the order is {OrderStatus.CREATED}. Current status: {Status}.");
    }

    private IEvent CreateEventFor(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.RECEIVED => new OrderReceived(Id.Value),
            OrderStatus.CLASSIFIED => new OrderClassified(Id.Value),
            OrderStatus.IN_PROCESS => new OrderInProcess(Id.Value),
            OrderStatus.READY => new OrderReady(Id.Value, CustomerId),
            OrderStatus.DELIVERED => new OrderDelivered(Id.Value),
            _ => throw new InvalidOperationException($"No domain event is defined for status {status}.")
        };
    }
}
