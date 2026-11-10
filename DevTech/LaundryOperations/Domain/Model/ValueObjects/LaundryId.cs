using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.ValueObjects;

public readonly record struct LaundryId(Guid Value) : IEntityId<Guid>;