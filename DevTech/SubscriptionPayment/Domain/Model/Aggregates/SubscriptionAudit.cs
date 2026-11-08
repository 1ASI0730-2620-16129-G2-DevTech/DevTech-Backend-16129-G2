using Acme.Center.Platform.Shared.Domain.Model.Entities;

namespace DevTech.SubscriptionPayment.Domain.Model.Aggregates;

public partial class Subscription : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
