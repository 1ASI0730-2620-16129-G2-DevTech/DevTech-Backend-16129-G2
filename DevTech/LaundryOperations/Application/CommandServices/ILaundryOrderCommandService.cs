using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.Shared.Application.Model;

namespace DevTech.LaundryOperations.Application.CommandServices;

public interface ILaundryOrderCommandService
{
    Task<Result<LaundryOrder>> Handle(
        ReceiveLaundryOrderCommand command,
        CancellationToken cancellationToken);

    Task<Result<LaundryOrder>> Handle(
        PrioritizeLaundryOrderCommand command,
        CancellationToken cancellationToken);

    Task<Result<LaundryOrder>> Handle(
        AssignWashingCycleCommand command,
        CancellationToken cancellationToken);

    Task<Result<LaundryOrder>> Handle(
        AssignResourceCommand command,
        CancellationToken cancellationToken);

    Task<Result<LaundryOrder>> Handle(
        ClassifyLaundryOrderCommand command,
        CancellationToken cancellationToken);

    Task<Result<LaundryOrder>> Handle(
        AdvanceStageCommand command,
        CancellationToken cancellationToken);
}