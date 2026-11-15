using DevTech.Shared.Domain.Repositories;
using DevTech.TrackingNotifications.Application.CommandServices;
using DevTech.TrackingNotifications.Domain.Model.Aggregates;
using DevTech.TrackingNotifications.Domain.Model.Commands;
using DevTech.TrackingNotifications.Domain.Repositories;

namespace DevTech.TrackingNotifications.Application.Internal.CommandServices;

/// <summary>
///     Order tracking command service.
/// </summary>
public class OrderTrackingCommandService(
    IOrderTrackingRepository orderTrackingRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : IOrderTrackingCommandService
{
    /// <inheritdoc />
    public async Task<OrderTracking> Handle(StartTrackingCommand command, CancellationToken cancellationToken = default)
    {
        var tracking = await orderTrackingRepository.FindByOrderIdAsync(command.OrderId, cancellationToken);
        if (tracking is not null)
        {
            tracking.UpdateEstimatedDelivery(command.EstimatedDelivery);
        }
        else
        {
            tracking = new OrderTracking(command, timeProvider.GetUtcNow());
            await orderTrackingRepository.AddAsync(tracking, cancellationToken);
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        return tracking;
    }

    /// <inheritdoc />
    public async Task<OrderTracking> Handle(RecordTrackingStageCommand command,
        CancellationToken cancellationToken = default)
    {
        var tracking = await orderTrackingRepository.FindByOrderIdAsync(command.OrderId, cancellationToken);
        if (tracking is null)
        {
            tracking = new OrderTracking(new StartTrackingCommand(command.OrderId, null), command.OccurredAt);
            await orderTrackingRepository.AddAsync(tracking, cancellationToken);
        }

        tracking.RecordStage(command.Stage, command.OccurredAt);
        await unitOfWork.CompleteAsync(cancellationToken);
        return tracking;
    }
}
