using DevTech.Shared.Domain.Model.ValueObjects;

namespace DevTech.CustomerManagement.Domain.Model.ValueObjects;

public readonly record struct CustomerId(Guid Value) : IEntityId<Guid>;