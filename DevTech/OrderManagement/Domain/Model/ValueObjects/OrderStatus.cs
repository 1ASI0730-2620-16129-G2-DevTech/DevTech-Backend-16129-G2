namespace DevTech.OrderManagement.Domain.Model.ValueObjects;

/// <summary>
///     Lifecycle of an order. The numeric values define the only allowed progression:
///     each status can only move to the immediately following one.
/// </summary>
public enum OrderStatus
{
    CREATED = 0,
    RECEIVED = 1,
    CLASSIFIED = 2,
    IN_PROCESS = 3,
    READY = 4,
    DELIVERED = 5
}
