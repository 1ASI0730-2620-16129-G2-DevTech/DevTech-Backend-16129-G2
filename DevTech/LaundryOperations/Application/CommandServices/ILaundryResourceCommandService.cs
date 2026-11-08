using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.Shared.Application.Model;

namespace DevTech.LaundryOperations.Application.CommandServices;

/// <summary>
///     Represents the application service that handles the commands of the <see cref="LaundryResource" /> aggregate.
/// </summary>
public interface ILaundryResourceCommandService
{
    /// <summary>
    ///     Handles the <see cref="ReleaseResourceCommand" /> to make a laundry resource available again.
    /// </summary>
    /// <param name="command">The command containing the id of the laundry resource to release.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to a <see cref="Result{T}" /> with the updated <see cref="LaundryResource" />,
    ///     or a failure carrying the domain error.
    /// </returns>
    Task<Result<LaundryResource>> Handle(ReleaseResourceCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the <see cref="SyncResourceStatusCommand" /> to synchronize the status of a laundry resource
    ///     with the monitored status.
    /// </summary>
    /// <param name="command">The command containing the resource id and the monitored status.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to a <see cref="Result{T}" /> with the updated <see cref="LaundryResource" />,
    ///     or a failure carrying the domain error.
    /// </returns>
    Task<Result<LaundryResource>> Handle(SyncResourceStatusCommand command, CancellationToken cancellationToken);
}
