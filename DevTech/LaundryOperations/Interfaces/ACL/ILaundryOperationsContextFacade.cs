using DevTech.Shared.Application.Model;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
namespace DevTech.LaundryOperations.Interfaces.Acl;

// Contract for the Order Management adapter. Caller supplies identifiers only.
public interface ILaundryOperationsContextFacade
{
    Task<Result<Guid>> ReceiveOrderAsync(Guid orderId, Guid laundryId, EPriority priority,
        DateTimeOffset? expectedCompletion, CancellationToken cancellationToken = default);
}

