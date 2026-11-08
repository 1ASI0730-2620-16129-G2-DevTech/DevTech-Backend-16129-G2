using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.LaundryOperations.Repositories;

/// <summary>
///     Represents the Laundry Order repository in the application.
/// </summary>
/// <remarks>
///     Extends <see cref="IBaseRepository{TEntity}" /> with queries specific to the
///     <see cref="LaundryOrder" /> aggregate.
/// </remarks>
public interface ILaundryOrderRepository : IBaseRepository<LaundryOrder>
{
    /// <summary>
    ///     Finds the laundry order associated with the given order of the Order Management context.
    /// </summary>
    /// <param name="id">
    ///     The identifier used to look up the laundry order.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A task that resolves to the matching <see cref="LaundryOrder" />,
    ///     or <c>null</c> if no laundry order is found.
    /// </returns>
    Task<LaundryOrder?> FindByOrderIdAsync(LaundryOrderId id, CancellationToken cancellationToken);
}