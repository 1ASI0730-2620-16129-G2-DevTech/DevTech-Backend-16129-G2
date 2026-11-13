using DevTech.Shared.Domain.Model.Entities;

namespace DevTech.PickupsDeliveries.Domain.Model.Aggregates;

/// <summary>
///     Audit extension for PickupDelivery aggregate.
/// </summary>
/// <remarks>
///     This partial class extends PickupDelivery with audit trail properties.
///     CreatedAt and UpdatedAt are automatically managed by the persistence layer
///     via the AuditableEntityInterceptor in the Shared bounded context.
///     Implements the IAuditableEntity interface to provide these properties.
/// </remarks>
public partial class PickupDelivery : IAuditableEntity
{
    /// <summary>
    ///     Gets the timestamp when this pickup or delivery was created.
    /// </summary>
    /// <remarks>
    ///     Automatically set once by the persistence layer on first insert. Maps to column: created_at.
    /// </remarks>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Gets the timestamp when this pickup or delivery was last updated.
    /// </summary>
    /// <remarks>
    ///     Automatically refreshed by the persistence layer on every save. Maps to column: updated_at.
    /// </remarks>
    public DateTimeOffset? UpdatedAt { get; set; }
}