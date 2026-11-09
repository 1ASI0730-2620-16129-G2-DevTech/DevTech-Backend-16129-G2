using DevTech.LaundryOperations.Application.CommandServices;
using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.Errors;
using DevTech.LaundryOperations.Domain.Repositories;
using DevTech.LaundryOperations.Domain.Repositories;
using DevTech.Shared.Application.Model;
using DevTech.Shared.Domain.Repositories;

namespace DevTech.LaundryOperations.Application.Internal.CommandServices;

public class LaundryOrderCommandService(
    ILaundryOrderRepository laundryOrderRepository,
    IUnitOfWork unitOfWork)
    : ILaundryOrderCommandService
{
    public async Task<Result<LaundryOrder>> Handle(
        ReceiveLaundryOrderCommand command,
        CancellationToken cancellationToken)
    {
        var existingOrder = await laundryOrderRepository
            .FindByOrderIdAsync(command.OrderId, cancellationToken);

        if (existingOrder is not null)
            return Result<LaundryOrder>.Failure(
                LaundryOperationsErrors.OrderAlreadyReceived);

        var laundryOrder = new LaundryOrder(command);

        await laundryOrderRepository.AddAsync(
            laundryOrder,
            cancellationToken);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<LaundryOrder>.Success(laundryOrder);
    }

    public Task<Result<LaundryOrder>> Handle(PrioritizeLaundryOrderCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<LaundryOrder>> Handle(AssignWashingCycleCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<LaundryOrder>> Handle(AssignResourceCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<LaundryOrder>> Handle(ClassifyLaundryOrderCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<LaundryOrder>> Handle(AdvanceStageCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}