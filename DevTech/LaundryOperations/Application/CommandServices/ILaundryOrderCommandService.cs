using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.Shared.Application.Model;

namespace DevTech.LaundryOperations.Application.CommandServices;

/// <summary>
///     Represents the application service that handles the commands of the <see cref="LaundryOrder" /> aggregate.
/// </summary>
public interface ILaundryOrderCommandService
{
    /// <summary>
    ///     Handles the <see cref="ReceiveLaundryOrderCommand" /> to create a laundry order.
    /// </summary>
    /// <param name="command">The command containing the data of the order to receive.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to a <see cref="Result{T}" /> with the created <see cref="LaundryOrder" />,
    ///     or a failure carrying the domain error.
    /// </returns>
    Task<Result<LaundryOrder>> Handle(ReceiveLaundryOrderCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the <see cref="ClassifyLaundryOrderCommand" /> to classify a laundry order.
    /// </summary>
    /// <param name="command">The command containing the id of the laundry order to classify.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to a <see cref="Result{T}" /> with the updated <see cref="LaundryOrder" />,
    ///     or a failure carrying the domain error.
    /// </returns>
    Task<Result<LaundryOrder>> Handle(ClassifyLaundryOrderCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the <see cref="AssignWashingCycleCommand" /> to assign a washing cycle to a laundry order.
    /// </summary>
    /// <param name="command">The command containing the laundry order id and the washing cycle id.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to a <see cref="Result{T}" /> with the updated <see cref="LaundryOrder" />,
    ///     or a failure carrying the domain error.
    /// </returns>
    Task<Result<LaundryOrder>> Handle(AssignWashingCycleCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the <see cref="AssignResourceCommand" /> to assign a laundry resource to a laundry order.
    /// </summary>
    /// <param name="command">The command containing the laundry order id and the resource id.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to a <see cref="Result{T}" /> with the updated <see cref="LaundryOrder" />,
    ///     or a failure carrying the domain error.
    /// </returns>
    Task<Result<LaundryOrder>> Handle(AssignResourceCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the <see cref="AdvanceStageCommand" /> to move a laundry order to its next processing stage.
    /// </summary>
    /// <param name="command">The command containing the id of the laundry order to advance.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to a <see cref="Result{T}" /> with the updated <see cref="LaundryOrder" />,
    ///     or a failure carrying the domain error.
    /// </returns>
    Task<Result<LaundryOrder>> Handle(AdvanceStageCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the <see cref="PrioritizeLaundryOrderCommand" /> to change the priority of a laundry order.
    /// </summary>
    /// <param name="command">The command containing the laundry order id and the new priority.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that resolves to a <see cref="Result{T}" /> with the updated <see cref="LaundryOrder" />,
    ///     or a failure carrying the domain error.
    /// </returns>
    Task<Result<LaundryOrder>> Handle(PrioritizeLaundryOrderCommand command, CancellationToken cancellationToken);
}
