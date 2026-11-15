using DevTech.LaundryOperations.Application.CommandServices;
using DevTech.LaundryOperations.Domain.Model.Commands;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using DevTech.LaundryOperations.Interfaces.Acl;
using DevTech.Shared.Application.Model;
namespace DevTech.LaundryOperations.Application.Acl;

public class LaundryOperationsContextFacade(ILaundryOrderCommandService commands) : ILaundryOperationsContextFacade
{
    public async Task<Result<Guid>> ReceiveOrderAsync(Guid orderId, Guid laundryId, EPriority priority,
        DateTimeOffset? expectedCompletion, CancellationToken cancellationToken = default)
    {
        var result = await commands.Handle(new ReceiveLaundryOrderCommand(orderId, laundryId, priority, expectedCompletion), cancellationToken);
        return result.Match(id => Result<Guid>.Success(id.Id.Value), Result<Guid>.Failure);
    }
}

