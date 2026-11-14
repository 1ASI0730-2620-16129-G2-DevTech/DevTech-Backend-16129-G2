using DevTech.Shared.Domain.Model.Entities;

namespace DevTech.SubscriptionPayment.Domain.Model.Aggregates;

public partial class Payment : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
