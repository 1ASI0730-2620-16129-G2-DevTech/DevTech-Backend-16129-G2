using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.LaundryOperations.Repositories;

/// <summary>
///     Represents the Laundry Resource repository in the application.
/// </summary>
/// <remarks>
///     Extends <see cref="IBaseRepository{TEntity}" /> with queries specific to the
///     <see cref="LaundryResource" /> aggregate.
/// </remarks>
public interface ILaundryResourceRepository : IBaseRepository<LaundryResource>
{
    /// <summary>
    ///     Finds all the laundry resources that belong to the given laundry.
    /// </summary>
    /// <param name="laundryId">
    ///     The unique identifier of the laundry that owns the resources.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A task that resolves to the collection of <see cref="LaundryResource" /> of the laundry.
    ///     The collection is empty if the laundry has no resources.
    /// </returns>
    Task<IEnumerable<LaundryResource>> FindByLaundryIdAsync(LaundryId laundryId, CancellationToken cancellationToken);
}