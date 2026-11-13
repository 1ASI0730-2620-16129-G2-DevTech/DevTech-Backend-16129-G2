using DevTech.Shared.Application.Model;
using DevTech.OrderManagement.Domain.Model.Aggregates;
using DevTech.OrderManagement.Domain.Model.Commands;

namespace DevTech.OrderManagement.Application.Services;

/// <summary>
///     Write side of Order Management. Failures are reported with an <c>OrderError</c> inside the result.
/// </summary>
public interface IOrderCommandService
{
    Task<Result<Order>> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<Order>> Handle(AddGarmentItemCommand command, CancellationToken cancellationToken = default);
    Task<Result<Order>> Handle(RemoveGarmentItemCommand command, CancellationToken cancellationToken = default);
    Task<Result<Order>> Handle(ChangeOrderStatusCommand command, CancellationToken cancellationToken = default);
}
