using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.OrderManagement.Domain.Model.ValueObjects;

/// <summary>
///     Strongly-typed identifier of a <c>GarmentItem</c> entity.
/// </summary>
/// <param name="Value">The underlying <see cref="Guid" />.</param>
public readonly record struct GarmentItemId(Guid Value) : IEntityId<Guid>
{
    /// <summary>
    ///     Creates a new identifier (time-ordered GUID v7, friendlier to database indexes).
    /// </summary>
    public static GarmentItemId New() => new(Guid.CreateVersion7());
}
