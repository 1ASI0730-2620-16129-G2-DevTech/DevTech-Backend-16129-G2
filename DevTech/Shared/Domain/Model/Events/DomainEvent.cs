namespace DevTech.Shared.Domain.Model.Events;

/// <summary>
///     Base record for domain events, carrying the data common to every event.
/// </summary>
/// <remarks>
///     Concrete events are declared as records that inherit from this type and add their own data.
/// </remarks>
public abstract record DomainEvent : IEvent
{
    /// <summary>
    ///     Gets the unique identifier of this event occurrence.
    /// </summary>
    public Guid EventId { get; } = Guid.NewGuid();

    /// <summary>
    ///     Gets the UTC timestamp when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}
