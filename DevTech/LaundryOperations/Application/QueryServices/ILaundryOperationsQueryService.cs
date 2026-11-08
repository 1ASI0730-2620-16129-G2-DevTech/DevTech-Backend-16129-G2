using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Domain.Model.Queries;

namespace DevTech.LaundryOperations.Application.QueryServices;

/// <summary>
///     Represents the application service that handles the queries of the Laundry Operations context.
/// </summary>
public interface ILaundryOperationsQueryService
{
    /// <summary>
    ///     Handles the <see cref="GetLaundryOrderByIdQuery" /> to get a laundry order by its id.
    /// </summary>
    /// <param name="query">The query containing the id of the laundry order.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to the <see cref="LaundryOrder" />, or <c>null</c> if it is not found.
    /// </returns>
    Task<LaundryOrder?> Handle(GetLaundryOrderByIdQuery query, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the <see cref="GetAllLaundryOrdersQuery" /> to get all the laundry orders.
    /// </summary>
    /// <param name="query">The query to get all the laundry orders.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to the collection of <see cref="LaundryOrder" />.
    /// </returns>
    Task<IEnumerable<LaundryOrder>> Handle(GetAllLaundryOrdersQuery query, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the <see cref="GetAllLaundryResourcesByLaundryIdQuery" /> to get the resources of a laundry.
    /// </summary>
    /// <param name="query">The query containing the id of the laundry.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to the collection of <see cref="LaundryResource" /> of the laundry.
    /// </returns>
    Task<IEnumerable<LaundryResource>> Handle(GetAllLaundryResourcesByLaundryIdQuery query,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the <see cref="GetAllWashingCyclesQuery" /> to get all the washing cycles.
    /// </summary>
    /// <param name="query">The query to get all the washing cycles.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to the collection of <see cref="WashingCycle" />.
    /// </returns>
    Task<IEnumerable<WashingCycle>> Handle(GetAllWashingCyclesQuery query, CancellationToken cancellationToken);
}
