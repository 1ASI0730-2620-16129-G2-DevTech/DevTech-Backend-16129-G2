using DevTech.Shared.Domain.Model.Entities;
namespace DevTech.LaundryOperations.Domain.Model.Aggregates;

/// <summary>Audit timestamps maintained by AuditableEntityInterceptor.</summary>
public partial class LaundryResource : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

