using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.LaundryOperations.Domain.Repositories;

/// <summary>
///     Represents the Washing Cycle repository in the application.
/// </summary>
/// <remarks>
///     Extends <see cref="IBaseRepository{TEntity,TKey}" /> for the <see cref="WashingCycle" /> entity,
///     using <see cref="WashingCycleId" /> as its key.
///     Washing cycles are catalog data, so the base operations are enough.
/// </remarks>
public interface IWashingCycleRepository : IBaseRepository<WashingCycle, WashingCycleId>
{
}
