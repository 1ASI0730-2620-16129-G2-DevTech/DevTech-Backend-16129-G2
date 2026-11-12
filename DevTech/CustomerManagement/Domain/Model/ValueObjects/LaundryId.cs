using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.CustomerManagement.Domain.Model.ValueObjects;

public readonly record struct LaundryId(Guid Value) : IEntityId<Guid>;