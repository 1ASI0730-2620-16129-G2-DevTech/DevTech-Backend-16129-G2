using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Model.Aggregates.Internal;

namespace DevTech.LaundryOperations.Domain.Model.Aggregates;

/// <summary>
///     Represents the LaundryResource aggregate root within the Laundry Operations Bounded Context.
/// </summary>
/// <remarks>
///     This class encapsulates the core data and behavior of a laundry resource (e.g. a washing machine
///     or a dryer), acting as an aggregate root to ensure consistency of its state.
/// </remarks>
public partial class LaundryResource : AggregateRoot
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LaundryResource" /> aggregate with its full state.
    /// </summary>
    /// <param name="id">The unique identifier of the laundry resource.</param>
    /// <param name="laundryId">The unique identifier of the laundry that owns this resource.</param>
    /// <param name="name">The name of the laundry resource.</param>
    /// <param name="status">The current status of the laundry resource.</param>
    /// <param name="capacity">The capacity of the laundry resource.</param>
    public LaundryResource(LaundryResourceId id, LaundryId laundryId, string name, EResourceStatus status,
        int capacity) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);

        Id = id;
        LaundryId = laundryId;
        Name = name;
        Status = status;
        Capacity = capacity;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LaundryResource" /> aggregate.
    ///     A new resource starts as available.
    /// </summary>
    /// <param name="laundryId">The unique identifier of the laundry that owns this resource.</param>
    /// <param name="name">The name of the laundry resource.</param>
    /// <param name="capacity">The capacity of the laundry resource.</param>
    public LaundryResource(LaundryId laundryId, string name, int capacity) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);

        LaundryId = laundryId;
        Name = name;
        Capacity = capacity;
    }

    public LaundryResource(Guid laundryId, string name, int capacity)
        : this(new LaundryId(laundryId), name, capacity)
    {
    }

    /// <summary>
    ///     Parameterless constructor used by the persistence mechanism and by the other constructors.
    ///     Move it to your other partial file if you already have one there.
    /// </summary>
    protected LaundryResource()
    {
        Name = string.Empty;
        Status = EResourceStatus.AVAIABLE;
    }

    /// <summary>
    ///     Gets the unique identifier of the laundry resource.
    /// </summary>
    public LaundryResourceId Id { get; }

    /// <summary>
    ///     Gets the identifier of the laundry that owns this resource
    ///     (reference by id to the Customer &amp; Business Management context).
    /// </summary>
    public LaundryId LaundryId { get; private set; }

    /// <summary>
    ///     Gets the name of the laundry resource.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     Gets the current status of the laundry resource.
    /// </summary>
    public EResourceStatus Status { get; private set; }

    /// <summary>
    ///     Gets the capacity of the laundry resource.
    /// </summary>
    public int Capacity { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether the laundry resource can be assigned to a laundry order.
    /// </summary>
    public bool IsAvailable => Status == EResourceStatus.AVAIABLE;
}