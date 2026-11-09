using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.Shared.Application.Model;

namespace DevTech.LaundryOperations.Application.CommandServices;

/// <summary>
/// Represents the laundry resource command service.
/// </summary>
public interface ILaundryResourceCommandService
{
    Task<Result<LaundryResource>> Handle(
        ReleaseResourceCommand command,
        CancellationToken cancellationToken);

    Task<Result<LaundryResource>> Handle(
        SyncResourceStatusCommand command,
        CancellationToken cancellationToken);
}