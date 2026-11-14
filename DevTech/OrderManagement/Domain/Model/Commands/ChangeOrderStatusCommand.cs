using DevTech.OrderManagement.Domain.Model.ValueObjects;

namespace DevTech.OrderManagement.Domain.Model.Commands;

/// <summary>
///     Moves an order to the next status of its lifecycle.
/// </summary>
public record ChangeOrderStatusCommand(OrderId OrderId, OrderStatus NewStatus);
