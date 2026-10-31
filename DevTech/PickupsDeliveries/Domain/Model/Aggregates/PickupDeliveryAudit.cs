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
    public DateTimeOffset? CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
}