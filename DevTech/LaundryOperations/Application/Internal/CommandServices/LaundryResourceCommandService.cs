using DevTech.LaundryOperations.Application.CommandServices;
using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.Errors;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.LaundryOperations.Domain.Repositories;
using DevTech.LaundryOperations.Domain.Repositories;
using DevTech.Shared.Application.Model;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.LaundryOperations.Application.Internal.CommandServices;

public class LaundryResourceCommandService(
    ILaundryResourceRepository laundryResourceRepository,
    IUnitOfWork unitOfWork)
    : ILaundryResourceCommandService
{
    /// <summary>
    /// Releases a laundry resource.
    /// </summary>
    public async Task<Result<LaundryResource>> Handle(
        ReleaseResourceCommand command,
        CancellationToken cancellationToken)
    {
        var resource = await laundryResourceRepository.FindByIdAsync(
            command.ResourceId,
            cancellationToken);

        if (resource is null)
            return Result<LaundryResource>.Failure(
                LaundryOperationsErrors.LaundryResourceNotFound);

        if (!resource.Release())
            return Result<LaundryResource>.Failure(
                LaundryOperationsErrors.ResourceNotBusy);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<LaundryResource>.Success(resource);
    }

    /// <summary>
    /// Synchronizes the status of a laundry resource.
    /// </summary>
    public async Task<Result<LaundryResource>> Handle(
        SyncResourceStatusCommand command,
        CancellationToken cancellationToken)
    {
        var resource = await laundryResourceRepository.FindByIdAsync(
            command.ResourceId,
            cancellationToken);

        if (resource is null)
            return Result<LaundryResource>.Failure(
                LaundryOperationsErrors.LaundryResourceNotFound);

        if (!resource.SynchronizeStatus(command.Status))
            return Result<LaundryResource>.Failure(
                LaundryOperationsErrors.InvalidResourceStatus);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<LaundryResource>.Success(resource);
    }
}