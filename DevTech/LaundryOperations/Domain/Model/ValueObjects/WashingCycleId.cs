using Acme.Center.Platform.Shared.Domain.Model.ValueObjects;

namespace DevTech.LaundryOperations.Domain.Model.ValueObjects;

public readonly record struct WashingCycleId(Guid Value) : IEntityId<Guid>
{
    
}